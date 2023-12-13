using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using PathCreation;

namespace PathCreation.Examples
{
    public class NewLevelFollow : MonoBehaviour
    {
        [SerializeField] private Rigidbody playerRb;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public BoostBar boostBar;
        [SerializeField] private int maxBoost;
        public float speed = 5;
        public float distanceTravelled, maxDistance;
        public bool isClosedPath;
        public Vector3 pathPosition, playerPosition;
        public Vector3 pathRotation, playerRotation;
        public Quaternion setRotation;

        void Start()
        {
            BoostGauge = 100;
            if (pathCreator != null)
            {

                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }
        void OnEnable()
        {
            playerRb = GetComponent<Rigidbody>();
            boostBar.SetMaxBoost(maxBoost);
            SetPath11();

            BoostGauge = 100;
            maxBoost = 100;

            playerRb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            score = 0;
            scoreText = GameObject.Find("/Canvas/Score/ScoreText").GetComponent<TextMeshProUGUI>();
            timeText = GameObject.Find("/Canvas/Time/TimeText").GetComponent<TextMeshProUGUI>();
            chipText = GameObject.Find("/Canvas/Chips/ChipsText").GetComponent<TextMeshProUGUI>();
        }

        public bool isBoosting, isMoving, BoostAttempt;
        public float BoostGauge, BoostTime;
        public void Boosting(InputAction.CallbackContext context)
        {
            if(0 <= BoostGauge && context.started)
            {
                isBoosting = true;
                speed = 20;
                BoostGauge -= 1;
                //Debug.Log("Boosting");
                //Gamepad.current.SetMotorSpeeds(.5f, .6f);
                Sounds.PlayOneShot(BoostStart, 1.0f);
                //Sounds.PlayScheduled(AudioSettings.dspTime + BoostStart.length);
            }
            if(context.canceled)
            {
                isBoosting = false;
                speed = 5f;
                //Debug.Log("NotBoosting , canceled, still moving");
                //Gamepad.current.SetMotorSpeeds(.25f, .25f);
                Sounds.PlayOneShot(BoostEnd, 1.0f);
            }
            if(0 >= BoostGauge && context.started)
            {
                isBoosting = false;
                BoostAttempt = true;
                speed = 5f;
                //Debug.Log("Out of Boost");
                //Gamepad.current.SetMotorSpeeds(.25f, .25f);
            }
        }
        public float movingVertical, movingHorizontal, rotatingVertical, rotatingHorizontal, playerInput;
        public void Rotating(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            movingHorizontal = direction.x;
            movingVertical = -direction.y;
            Debug.Log(movingVertical + "," + movingHorizontal);
        }
        
        void Update()
        {
            scoreText.text = score.ToString();
            timeText.text = levelTimeInt.ToString();
            chipText.text = chipCounter.ToString();
            boostBar.SetBoost((int)BoostGauge);

            levelTimeLeft -= Time.deltaTime;
            levelTimeInt = (int)levelTimeLeft;

            //BoostStuffs
            if(speed >= 20f)
            {
                BoostGauge -= Time.deltaTime * 10;
            }
            if(BoostGauge <= 0 && isMoving == true)
            {
                speed = 5f;
                isBoosting = false;
            }
            if(BoostGauge <= 0 && isMoving == false)
            {
                speed = 5f;
                isBoosting = false;
            }
            //BoostAttempts
            if(BoostAttempt == true)
            {
                speed = 10f;
                BoostTime += Time.deltaTime;
            }
            if(BoostTime >= 0.33f && isMoving == true)
            {
                BoostAttempt = false;
                speed = 5f;
                BoostTime = 0;
            }
            if(BoostTime >= 0.33f && isMoving == false)
            {
                BoostAttempt = false;
                speed = 5;
                BoostTime = 0;
            }
            //Links
            if(linkTimeLeft <= 0)
            {
                LinkEmpty();
            }
        }
        void FixedUpdate()
        {
            rotatingVertical = movingVertical;
            rotatingHorizontal = movingHorizontal;
            if(rotatingVertical <= 0)
            {
                rotatingHorizontal *= -1;
            }
            if(rotatingVertical == 0)
            {
                rotatingHorizontal *= 4;
                if(rotatingHorizontal <= 0)
                {
                    rotatingHorizontal = 0;
                }
            }
            
            playerRb.AddForce(Vector3.up * -movingVertical * 0.5f, ForceMode.VelocityChange);

            distanceTravelled += movingHorizontal * speed * Time.deltaTime;

            pathRotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;

            playerInput = (rotatingVertical * 90) + (-rotatingHorizontal * 45);

            playerRotation.x = playerInput;
            playerRotation.y = pathRotation.y;
            playerRotation.z = pathRotation.z;

            setRotation.eulerAngles = playerRotation;
            
            transform.rotation = setRotation;


            if(isClosedPath && distanceTravelled >= maxDistance)
            {
                distanceTravelled -= maxDistance;
            }
            
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
        void SetPath11()
        {
            maxDistance = 28.85f;
            pathCreator = GameObject.Find("1-1/Path (28.85)").GetComponent<PathCreator>();
            endOfPathInstruction = (EndOfPathInstruction)0; //Loop
            isClosedPath = true;
            levelTimeLeft = 120;
        }
        void SetPath12()
        {
            maxDistance = 144.66f;
            pathCreator = GameObject.Find("1-2/Path (144.66)").GetComponent<PathCreator>();
            endOfPathInstruction = (EndOfPathInstruction)2; //Stop
            isClosedPath = false;
            
        }

        public TextMeshProUGUI scoreText, timeText, chipText;
        public int score, link, chipCounter, levelTimeInt;
        public float linkTimeLeft, levelTimeLeft;
        public bool power, linkActive;
        public bool isInGround;
        public AudioSource Sounds;
        public AudioClip BoostStart, BoostIng, BoostEnd;
        public AudioClip BlueChipSFX, YellowRingSFX, GreenRingSFX, HalfRingSFX, PowerRingSFX, SpikeRingSFX;
        void OnTriggerEnter(Collider other)
        { 
            Debug.Log("Collide");
            if(other.gameObject.CompareTag("BlueChip"))
            {
                //Debug.Log("Blue Chip");
                Sounds.PlayOneShot(BlueChipSFX, 1.0f);
                Destroy(other.gameObject);
                score += 20;
                chipCounter += 1;

                LinkIncrease();
            }
            if(other.gameObject.CompareTag("Star"))
            {
                Sounds.PlayOneShot(BlueChipSFX, 1.0f);
                Destroy(other.gameObject);
                score += 20;
                LinkIncrease();
            }
            if(other.gameObject.CompareTag("YellowRing"))
            {
                //Debug.Log("YellowRing");
                Sounds.PlayOneShot(YellowRingSFX, 1.0f);
                if(BoostGauge <= 90)
                {
                    BoostGauge += 10;
                }
                if(BoostGauge >= 90)
                {
                    BoostGauge = 100;
                }
                score += 100;
                LinkIncrease();
            }
            if(other.gameObject.CompareTag("GreenRing"))
            {
                //Debug.Log("GreenRing");
                Sounds.PlayOneShot(GreenRingSFX, 1.0f);
                if(BoostGauge <= 90)
                {
                    BoostGauge += 10;
                }
                if(BoostGauge >= 90)
                {
                    BoostGauge = 100;
                }
            }
            if(other.gameObject.CompareTag("HalfRing"))
            {
                //Debug.Log("HalfRing");
                Sounds.PlayOneShot(HalfRingSFX, 1.0f);
                if(BoostGauge <= 90)
                {
                    BoostGauge += 10;
                }
                if(BoostGauge >= 90)
                {
                    BoostGauge = 100;
                }
                score += 60;
                LinkIncrease();
            }
            if(other.gameObject.CompareTag("PowerRing"))
            {
                //Debug.Log("PowerRing");
                Sounds.PlayOneShot(PowerRingSFX, 1.0f);
                BoostGauge = 100;
                score += 250;
                power = true;
                LinkIncrease();
            }
            if(other.gameObject.CompareTag("SpikeRing"))
            {
                //Debug.Log("SpikeRing");
                Sounds.PlayOneShot(SpikeRingSFX, 1.0f);
                BoostGauge -= 5;
                score -= 100;
            }
        }
        void LinkIncrease()
        {
            linkTimeLeft = 10;
            link += 1;
            linkActive = true;
        }
        void LinkEmpty()
        {
            linkTimeLeft = 0;
            link = 0;
            linkActive = false;
        }
    }
}