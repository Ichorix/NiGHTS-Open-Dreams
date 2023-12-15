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
    private Rigidbody rigidbody;

    [Header("Current Level Information")]
    public PathCreator[] ActiveLevelPaths = new PathCreator[4]; //Creates the level with the appropriate amount of paths. Paths assigned in Inspector
    public AnimationCurve[] ActiveLevelGrading = new AnimationCurve[4]; //Score defined in Inspector. Mapped as Grade over Score. Grade 5 = A, Grade 0 = F
    public float[] ActiveLevelTimes = new float[4]; //Time defined in Inspector.
    public PathCreator currentPath;
    public EndOfPathInstruction endOfPathInstruction;

    public float distanceTravelled;
    private int levelSegment;
    private bool continueLevel;
    public float levelTimeLeft;
    private int chipRequirement;

    public float currentChips;
    public float currentScore;

    // Links
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
    

    private Vector3 pathPosition;
    private Vector3 pathRotation;

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

        levelSegment = 0;
        currentPath = ActiveLevelPaths[levelSegment];
        //chipRequirement = 50;
        //score = 0;

        continueLevel = false;
        distanceTravelled = 0;
        
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        
        /*
        if(true) // TODO: Sort through later
        {
            pathS11.gameObject.SetActive(true);
            currentPath = pathS11;
            levelTimeLeft = pathS11time + bonusTime;
            if(pathMusic != null) pathMusic.SetActive(true);
            if(openLevel != null) openLevel.SetActive(false);

            chipCounter = 0;
            growthPalace.freedIdeas = 0;
            growthPalace.UpdateStuff();
            transform.localPosition = Vector3.zero;
        }
        */
    }
    void OnDisable()
    {
        /*
        if(pathMusic != null) pathMusic.SetActive(false);
        if(openLevel != null) openLevel.SetActive(true);

        growthPalace.ReturnAllIdeyas();
        growthPalace.UpdateStuff();
        currentPath.gameObject.SetActive(false);
        */
    }

    void Update()
    {
        SpeedLogic(); // Mostly copied from NPlayerOpenControl.MovePlayer()
        MovePlayer();
        BoostStuff();
        LevelLogic(); //Counting time and logic for when Time is up
        //ParaloopLogic(); //Paraloop
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

    void ParaloopLogic()
    {
        //if(speed >= nonBoostingSpeed)
        //    paraloopInstantiator.SetActive(true);
        //else paraloopInstantiator.SetActive(false);
    }

    void LevelLogic()
    {
        levelTimeLeft += Time.deltaTime;
        if(linkActive) LinkTimeLeft -= Time.deltaTime;

        if(levelTimeLeft < 0)
        {
            //sc.Activate1();
            //currentPath.gameObject.SetActive(false);
            //if(pathMusic != null) pathMusic.SetActive(false);
            //if(openLevel != null) openLevel.SetActive(true);
            
            //growthPalace.freedIdeas = 0;
            //growthPalace.UpdateStuff();
            //growthPalace.ReturnAllIdeyas();
        }
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
            _animations.BoostAnimationOverride(true);
            if(t >= _stats.boostAttemptTime)
            {
                boostAttempt = false;
                boostAttemptCooldown = true;
            }
            yield return null;
        }
        t = 0;
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
}
