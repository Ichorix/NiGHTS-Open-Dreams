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
    public PathCreator[] Level1Paths = new PathCreator[4]; //Creates the level with the appropriate amount of paths. Paths assigned in Inspector
    public AnimationCurve[] Level1Grading = new AnimationCurve[4]; //Score defined in Inspector. Mapped as Grade over Score. Grade 5 = A, Grade 0 = F
    public float[] Level1Times = new float[4]; //Time defined in Inspector.
    public PathCreator currentPath;
    public EndOfPathInstruction endOfPathInstruction;

    public float distanceTravelled;
    private int levelSegment;
    private bool continueLevel;
    public float levelTimeLeft;
    private int chipRequirement;

    public float currentChips;
    public float currentScore;

    public LinkControl linkControl;
    public int link;
    public bool linkActive;
    public float linkTimeLeft;
    

    private Vector3 pathPosition;
    private Vector3 pathRotation;

    [SerializeField] private bool canBoost;
    [SerializeField] private bool boostAttempt;
    [SerializeField] private bool boostAttemptCooldown;
    [SerializeField] private float _speed;


    void Start()    
    {
        rigidbody = GetComponent<Rigidbody>();
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
        _stats.boostGauge = _stats.maxBoost;

        levelSegment = 0;
        currentPath = Level1Paths[levelSegment];
        //chipRequirement = 50;
        //score = 0;

        continueLevel = false;
        distanceTravelled = 0;
        
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
        //UpdateUI(); //Updates Score, Time, Chip, and Boost Bar
        //LevelLogic(); //Counting time and logic for when Time is up
        //ParaloopLogic(); //Paraloop
    }
    void MovePlayer()
    {
        distanceTravelled += _stats.MoveDirection.x * _speed * Time.deltaTime;

        pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;

        rigidbody.MovePosition(new Vector3(pathPosition.x, transform.position.y, pathPosition.z));
        rigidbody.AddForce(Vector3.up * _stats.MoveDirection.y * _speed, ForceMode.VelocityChange);

        transform.eulerAngles = new Vector3(0, pathRotation.y, 0);

        if(new Vector2(_stats.MoveDirection.x, _stats.MoveDirection.y) != Vector2.zero)
            _stats.isMoving = true;
        else _stats.isMoving = false;  
    }  

    void SpeedLogic()
    {
        canBoost = false;
        if(_stats.isBoosting) canBoost = true;

        if(_stats.boostGauge <= 0)
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

        ////TODO remove this acceleration code if i dont like it
        /*
        //Checks if you are going faster than the target speed (true when target speed is 0 or when target speed is normal speed after boosting)
        //And if your speed is greater than the normal speed after boosting
        if(_speed >= targetSpeed && _speed > _stats.speedABoostingLevel) // Will return true if you are decelerating after boosting.
            targetSpeed = _stats.speedABoostingLevel;                    
        // If the speed change rate is too high or if performance is bad, it will sometimes jump past speedABoosting resulting in decelerating to normal speed
        // This can be fixed by increasing the speedOffset.

        if(!_stats.isMoving)
            targetSpeed = 0;
        
        float speedChangeRate = _stats.isBoosting ? _stats.boostingAccelerationRate : _stats.normalAccelerationRate;
        float speedOffset = 0.5f; //Default 0.5f

        if(_speed < targetSpeed - speedOffset) //Accelerate
        {
            _speed += speedChangeRate * Time.deltaTime;

            // round speed to 2 decimal places
            _speed = Mathf.Round(_speed * 100f) / 100f;
        }
        else if(_speed > targetSpeed + speedOffset) //Decelerate
        {
            if(targetSpeed == 0) speedChangeRate = _stats.decelerationRate;

            _speed -= speedChangeRate * Time.deltaTime;

            _speed = Mathf.Round(_speed * 100f) / 100f;
        }
        if(_speed <= speedOffset) _speed = 0;
        */
    }

    private void BoostStuff()
    {
        if(canBoost && _stats.isMoving)
            _stats.boostGauge -= _stats.boostDepletionRate * Time.deltaTime;
        _stats.boostGauge = Mathf.Clamp(_stats.boostGauge, 0, _stats.maxBoost);
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

    void UpdateUI()
    {
        //scoreText.text = score.ToString();
        //timeText.text = (int)levelTime.ToString();
        //chipText.text = chipCounter.ToString() + " / " + chipReq.ToString();
        //boostBar.SetBoost((int)_stats.boostGauge);
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
        linkTimeLeft = 1;
        link += 1;
        linkActive = true;
        if(linkControl != null)
            linkControl.RunLinkIncrease(link);
    }
    public void LinkEmpty()
    {
        linkTimeLeft = 0;
        link = 0;
        linkActive = false;
        if(linkControl != null)
            linkControl.RunLinkIncrease(link);
    }
}
