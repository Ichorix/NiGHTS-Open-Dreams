using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NPlayerLevelFollow : MonoBehaviour
{
    public NPlayerScriptableObject _stats;
    public NPlayerAnimations _animations;
    private Rigidbody rigidbody;
    private int chipRequirement;
    public float score;
    private int levelSegment;
    [SerializeField] private bool continueLevel;
    [SerializeField] private float distanceTravelled;
    public PathCreator[] IdeyaLevel1 = new PathCreator[4]; //Creates the level with the appropriate amount of paths. Paths assigned in Inspector
    private PathCreator currentPath;
    public EndOfPathInstruction endOfPathInstruction;
    [SerializeField] private float levelTimeLeft;
    private int levelTimeInt;
    private Vector3 pathPosition;
    private Vector3 pathRotation;

    //Rotation Stuff
    private Vector3 playerRotation;
    private bool isBackwards;
    public bool isUpsideDown;
    public float upsideDownTime;
    public float flipTimeThreshold;
    public float flip = 180;
    public float oppositeFlip = 180;
    public bool flipped;

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
        currentPath = IdeyaLevel1[levelSegment];
        chipRequirement = 50;
        score = 0;

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
        MovePlayer();
        //UpdateUI(); //Updates Score, Time, Chip, and Boost Bar
        //LevelLogic(); //Counting time and logic for when Time is up
        //ParaloopLogic(); //Paraloop
        //BoostLogic();
    }
    void UpdateUI()
    {
        /*
        scoreText.text = score.ToString();
        timeText.text = levelTimeInt.ToString();
        chipText.text = chipCounter.ToString() + " / " + chipReq.ToString();
        boostBar.SetBoost((int)BoostGauge);
        */
    }
    void LevelLogic()
    {
        levelTimeLeft += Time.deltaTime;
        levelTimeInt = (int)levelTimeLeft;
        
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
    void ParaloopLogic()
    {
        //if(speed >= nonBoostingSpeed)
        //    paraloopInstantiator.SetActive(true);
        //else paraloopInstantiator.SetActive(false);
    }
    void BoostLogic()
    {
        /*
        if(isMoving(new Vector2(movingHorizontal, movingVertical)) && isBoosting)
            BoostGauge -= Time.deltaTime * 10;
        if(BoostGauge <= 0)
        {
            speed = nonBoostingSpeed;
            isBoosting = false;
        }

        if(BoostAttempt)
        {
            speed = BoostAttemptSpeed;
            BoostTime += Time.deltaTime;
        }
        if(BoostTime >= 0.33f)
        {
            BoostAttempt = false;
            speed = nonBoostingSpeed;
            BoostTime = 0;
        }
        */
    }

    void MovePlayer()
    {
        distanceTravelled += _stats.MoveDirection.x * _stats.normalSpeed * Time.deltaTime;

        pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;

        rigidbody.MovePosition(new Vector3(pathPosition.x, transform.position.y, pathPosition.z));
        rigidbody.AddForce(Vector3.up * _stats.MoveDirection.y * _stats.normalSpeed, ForceMode.VelocityChange);
        
        playerRotation.x = transform.rotation.x; 
        playerRotation.y = pathRotation.y;          // Gets the Left/Right rotation of the track
        playerRotation.z = transform.rotation.z;

        transform.rotation = Quaternion.Euler(pathRotation);

        Vector3 lookDirection = new Vector3(Mathf.Abs(_stats.MoveDirection.x), _stats.MoveDirection.y, 0).normalized; //Set Z to either 0 or 180 depending on if forward or backwards, then change the value through the IEnumerator for lerping
        transform.forward += lookDirection * 5; // Whatever dark magic I used, this fixes it from being 45 to -45 to now be closer to 90 to -90
        

        if(_stats.MoveDirection.x > 0) //Checks if moving forwards
            isBackwards = false;
        else if(_stats.MoveDirection.x < 0) //Checks if moving backwards
            isBackwards = true;

        if(isBackwards)
        {
            //Rotates the player properly. Negative Y flips left vs right, and z+flip (lerp between 0 and 180) flips over his head. Lerped for more dynamic effect in the Enumerator below
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z + flip);
        }
        else if(flipped)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + oppositeFlip);
        }

        // Returns true when the player is upside Down. Used to determine whether to lerp for flip
        if(transform.eulerAngles.z == 180)
            upsideDownTime += Time.deltaTime;
        else upsideDownTime = 0;
        if(upsideDownTime >= flipTimeThreshold)
        {
            if(!flipped)
                StartCoroutine(FlipPlayer(180, 0));
            else
                StartCoroutine(OppositeFlipPlayer(180, 0));
        }
        

    }

    IEnumerator FlipPlayer(int From, int To)
    {
        float t = 0;
        flipped = true;
        upsideDownTime = 0;
        while(t < 1)
        {
            t += Time.deltaTime * _stats.recenterSpeed;
            flip = Mathf.Lerp(From, To, t);
            oppositeFlip = Mathf.Lerp(To, From, t);
            yield return null;
        }
    }
    IEnumerator OppositeFlipPlayer(int From, int To)
    {
        float t = 0;
        flipped = false;
        upsideDownTime = 0;
        while(t < 1)
        {
            t += Time.deltaTime * _stats.recenterSpeed;
            oppositeFlip = Mathf.Lerp(From, To, t);
            flip = Mathf.Lerp(To, From, t);
            yield return null;
        }
    }
    
}
