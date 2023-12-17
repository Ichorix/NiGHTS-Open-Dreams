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
    [SerializeField] private NPlayerStateController _playerStates;
    [SerializeField] private NotATrailScript trailInstantiator;
    private Rigidbody rigidbody;
    private Vector3 pathPosition;
    private Vector3 pathRotation;

    [Header("Current Level Information")]
    public PathCreator[] ActiveLevelPaths = new PathCreator[4]; //Creates the level with the appropriate amount of paths. Paths assigned in Inspector
    public AnimationCurve[] ActiveLevelGrading = new AnimationCurve[4]; //Score defined in Inspector. Mapped as Grade over Score. Grade 5 = A, Grade 0 = F
    public float[] ActiveLevelTimes = new float[4]; //Time defined in Inspector.
    public int[] TopGrades = new int[4];
    public float[] TopScore = new float[4];
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
                currentPath = ActiveLevelPaths[levelSegment];
                LevelTimeLeft = ActiveLevelTimes[levelSegment];
                // And sets it active again
                currentPath.gameObject.SetActive(true);
                currentChips = 0;
                currentScore = 0;
            }
        }
    }
    

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
                    ExitLevel();
            }
            levelTimeLeft = value;
        }
    }
    private bool lowOnTime;
    public int chipRequirement;
    public int currentChips;
    public int currentScore;

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
    [Header("Movement Information")]
    [SerializeField] private bool canBoost;
    [SerializeField] private bool boostAttempt;
    [SerializeField] private bool boostAttemptCooldown;
    [SerializeField] private float _speed;


    void Start()    
    {
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
        LevelTimeLeft = ActiveLevelTimes[levelSegment];
        
        ContinueLevel = false;
        distanceTravelled = 0;
        transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    void Update()
    {
        SpeedLogic(); // Mostly copied from NPlayerOpenControl.MovePlayer()
        MovePlayer();
        BoostStuff();
        LevelLogic(); //Counting time and logic for when Time is up
    }
    void MovePlayer()
    {
        distanceTravelled += _stats.MoveDirection.x * _speed * Time.deltaTime;

        pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;
        
        float yMovement = _stats.MoveDirection.y * _speed * Time.deltaTime;
        rigidbody.MovePosition(new Vector3(pathPosition.x, transform.position.y + yMovement, pathPosition.z));
        //rigidbody.AddForce(Vector3.up * _stats.MoveDirection.y * _speed, ForceMode.VelocityChange);

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
            _stats.BoostGauge -= _stats.boostDepletionRate * Time.deltaTime;
        _stats.BoostGauge = Mathf.Clamp(_stats.BoostGauge, 0, _stats.maxBoost);
    }

    void LevelLogic()
    {
        LevelTimeLeft -= Time.deltaTime;
        if(linkActive) LinkTimeLeft -= Time.deltaTime;
    }

    //////////FUNCTIONS//////////

    private bool IsMoving(Vector2 direction)
    {
        if(direction != Vector2.zero)
             return true;
        else
        {
            _stats.isBoosting = false;
            return false;
        }
    }
    private void ExitLevel()
    {
        currentPath.gameObject.SetActive(false);
        _playerStates.ActivateOpenPlayer();
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
            t += Time.deltaTime;
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
            t += Time.deltaTime;
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
}
