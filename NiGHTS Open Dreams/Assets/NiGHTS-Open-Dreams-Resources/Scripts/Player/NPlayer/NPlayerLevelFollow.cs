using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

using UnityEngine.UI;
using TMPro;

public class NPlayerLevelFollow : MonoBehaviour
{
    [SerializeField] private NPlayerScriptableObject _stats;
    [SerializeField] private NPlayerAnimations _animations;
    [SerializeField] private NPlayerLevelRotations _rotationAnimation;
    [SerializeField] private NPlayerLevelRotations _skinRotationAnimation;
    [SerializeField] private NPlayerStateController _playerStates;
    [SerializeField] private ParaloopEmitter trailInstantiator;
    public Material blueChipMaterial;
    public CollectablesData blueChipData;
    private Rigidbody rigidbody;
    private Vector3 pathPosition;
    private Vector3 pathRotation;

    [Header("Current Level Information")]
    public EnterLevelScript ActiveLevelPalace;
    public PathCreator[] ActiveLevelPaths = new PathCreator[4]; //Creates the level with the appropriate amount of paths. Paths assigned in Inspector
    public float[] ActiveLevelTimes = new float[4]; //Time defined in Inspector.
    public int[] ActiveLevelChipRequirement = new int[4];
    public PathCreator currentPath;
    public EndOfPathInstruction endOfPathInstruction;
    public float distanceTravelled;
    public int levelSegment;
    public IdeyaChase recoveredIdeya;
    [SerializeField] private bool continueLevel;
    public bool ContinueLevel
    {
        get{ return continueLevel; }
        set
        {
            continueLevel = value;
            if(!continueLevel)
            {
                // Makes the previous path inactive
                currentPath.gameObject.SetActive(false);
                // Then sets the current path to the proper path
                distanceTravelled = 0;
                currentPath = ActiveLevelPaths[Mathf.Clamp(levelSegment, 0, ActiveLevelPaths.Length-1)];
                LevelTimeLeft = ActiveLevelTimes[Mathf.Clamp(levelSegment, 0, ActiveLevelTimes.Length-1)] + bonusTime;
                // And sets it active again
                currentPath.gameObject.SetActive(true);
                // Resets the necessary values
                currentChips = 0;
                currentScore = 0;
                blueChipMaterial.SetFloat("_EmissionOn", 0f);
                blueChipData.Score = 10;
            }
        }
    }
    
    [Space]
    [Header("Player Level Data")]
    [SerializeField] private float levelTimeLeft;
    public float LevelTimeLeft
    {
        get{ return levelTimeLeft; }
        set
        {
            if(value <= 5)
            {
                lowOnTime = true;
                if(value <= 0)
                    DoExitLevel();
            }
            levelTimeLeft = value;
        }
    }
    private bool lowOnTime;
    public float bonusTime;
    public int currentChips;
    public int currentScore;
    [Space]
    [Header("Links Data")]
    [SerializeField] private LinkControl linkControl;
    public int link;
    [SerializeField] private bool linkActive;
    [SerializeField] private float linkTimeLeft;
    public float LinkTimeLeft
    {
        get{ return linkTimeLeft; }
        set
        {
            if(value <= 0) LinkEmpty();
            linkTimeLeft = value;
        }
    }
    [Space]
    [Header("Movement Information")]
    [SerializeField] private bool canBoost;
    [SerializeField] private bool boostAttempt;
    [SerializeField] private bool boostAttemptCooldown;
    [SerializeField] private float _speed;
    public bool specialBehaviourActive;


    void Start()    
    {
        Application.targetFrameRate = 60;
        if (currentPath != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            currentPath.pathUpdated += OnPathChanged;
        }
    }
    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged() {
        distanceTravelled = currentPath.path.GetClosestDistanceAlongPath(transform.position);
    }
    void OnEnable() //Resets all applicable values to start the level.
    {
        //Physics.gravity *= gravityModifier;
        _stats.BoostGauge = _stats.maxBoost;
        currentChips = 0;
        currentScore = 0;

        levelSegment = 0;
        currentPath = ActiveLevelPaths[levelSegment];
        LevelTimeLeft = ActiveLevelTimes[levelSegment] + bonusTime;
        
        ContinueLevel = false;
        distanceTravelled = 0;
        transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

        ActiveLevelPalace.ResetIdeyas();

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
    void OnDisable()
    {
        trailInstantiator.enabled = false;
    }

    void Update()
    {
        SpeedLogic(); // Mostly copied from NPlayerOpenControl.MovePlayer()
        MovePlayer(); // The unique movement code for the level player
        BoostStuff(); // Boost Stuff
        LevelLogic(); // Counting time and logic for when Time is up
    }
    void MovePlayer()
    {
        distanceTravelled += _stats.MoveDirection.x * _speed * _playerStates.UsableDeltaTime;

        pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;

        //float xMovement = _stats.MoveDirection.x * _speed * _playerStates.UsableDeltaTime;
        float yMovement = _stats.MoveDirection.y * _speed * _playerStates.UsableDeltaTime;
        Debug.Log("Horizontal: " + _stats.MoveDirection.x * _speed * _playerStates.UsableDeltaTime + " Vertical: " + _stats.MoveDirection.y * _speed * _playerStates.UsableDeltaTime);
        transform.position = new Vector3(pathPosition.x, transform.position.y + yMovement, pathPosition.z);

        //rigidbody.MovePosition(new Vector3(pathPosition.x, transform.position.y + yMovement, pathPosition.z));
        //rigidbody.AddForce(Vector3.up * _stats.MoveDirection.y * _speed, ForceMode.VelocityChange);
        // Something similar to the old vertical method using rigidbody forces to check for collisions
        // Makes movement inconsistent though and since there is a new ground check in NPlayerCollisionController I think this just makes it a bit better with controller.
        // Keyboard movement is snappy now as a consequence

        transform.eulerAngles = new Vector3(0, pathRotation.y, 0);

        if(new Vector2(_stats.MoveDirection.x, _stats.MoveDirection.y) != Vector2.zero)
            _stats.isMoving = true;
        else _stats.isMoving = false;

        trailInstantiator.enabled = _stats.isMoving;
    }  

    void SpeedLogic()
    {
        canBoost = false;
        if(_stats.isBoosting) canBoost = true;

        if(_stats.BoostGauge <= 0)
        {
            canBoost = false;
            if(_stats.isMoving && _stats.runBoostAttempt && !boostAttemptCooldown)
            {
                _stats.runBoostAttempt = false;
                boostAttempt = true;
                RunBoostAttempt();
            }
            else _stats.runBoostAttempt = false;
        }

        // Compares if it is boosting to use the boosting speed, if it is running the boost attempt to use the boost attempt speed, and if neither then the normal speed
        _speed = canBoost ? _stats.boostingSpeedLevel : boostAttempt ? _stats.boostAttemptSpeedLevel : _stats.normalSpeedLevel;
    }

    private void BoostStuff()
    {
        if(canBoost && _stats.isMoving)
            _stats.BoostGauge -= _stats.boostDepletionRate * _playerStates.UsableDeltaTime;
        _stats.BoostGauge = Mathf.Clamp(_stats.BoostGauge, 0, _stats.maxBoost);
    }

    void LevelLogic()
    {
        LevelTimeLeft -= _playerStates.UsableDeltaTime;
        if(linkActive) LinkTimeLeft -= _playerStates.UsableDeltaTime;
    }

    //////////FUNCTIONS//////////
    public void DoExitLevel()
    {
        StartCoroutine(ExitLevel());
    }
    IEnumerator ExitLevel()
    {
        // Prevent the level from ending while you are flipping
        // If that happens then a whole lot of ugly bugs pop up with the rotations
        // There's probably another workaround but this is the simplest.
        while(_rotationAnimation.flipping || _skinRotationAnimation.flipping)
            yield return null;
        
        // If you want to be generous you can put an if() here to check if the time is still < 0
        // Which would allow the players a maximum of one second extra (based on current flip speed) to pick up stars if they time it perfectly
        ActiveLevelPalace.ResetIdeyas();
        currentPath.gameObject.SetActive(false);
        _playerStates.ActivateOpenPlayer();
        blueChipMaterial.SetFloat("_EmissionOn", 0f);
        blueChipData.Score = 10;
    }
    public IEnumerator BeatLevel()
    {
        while(_rotationAnimation.flipping || _skinRotationAnimation.flipping)
            yield return null;

        currentPath.gameObject.SetActive(false);
        _playerStates.ActivateOpenPlayer();
        blueChipMaterial.SetFloat("_EmissionOn", 0f);
        blueChipData.Score = 10;
    }
    
    public void RunBoostAttempt()
    {
        StartCoroutine(BoostAttempt());
    }
    IEnumerator BoostAttempt()
    {
        float t = 0;
        while(boostAttempt)
        {
            t += _playerStates.UsableDeltaTime;
            _stats.BoostAttempt = true;
            _animations.BoostAnimationOverride(true);
            if(t >= _stats.boostAttemptTime)
            {
                boostAttempt = false;
                boostAttemptCooldown = true;
            }
            yield return null;
        }

        t = 0;
        _stats.BoostAttempt = false;
        while(boostAttemptCooldown)
        {
            t += _playerStates.UsableDeltaTime;
            _animations.BoostAnimationOverride(false);
            if(t >= _stats.boostAttemptCooldown)
                boostAttemptCooldown = false;
            yield return null;
        }
        _stats.runBoostAttempt = false;
    }

    public void LinkIncrease()
    {
        LinkTimeLeft = 1;
        link += 1;
        linkActive = true;
        if(linkControl != null)
            linkControl.RunLinkIncrease(link);
    }
    public void LinkEmpty()
    {
        link = 0;
        linkActive = false;
        if(linkControl != null)
            linkControl.RunLinkIncrease(link);
    }

    public int CalculateGrade(AnimationCurve GradingCurve, float ScoreToBeGraded)
    {
        float OutputGrade = GradingCurve.Evaluate(ScoreToBeGraded);
        return (int)OutputGrade;
    }

    //////////BEHAVIOUR//////////

    public IEnumerator Behaviour_DashBall(float launchTime, float launchSpeed)
    {
        specialBehaviourActive = true;
        float behaviourTime = launchTime;
        while(behaviourTime > 0)
        {
            distanceTravelled += launchSpeed * Time.deltaTime;
            behaviourTime -= Time.deltaTime;
            yield return null;
        }
        specialBehaviourActive = false;
    }
}
