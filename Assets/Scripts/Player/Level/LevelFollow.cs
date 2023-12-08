using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using PathCreation.Examples;

namespace PathCreation.Examples
{
    public class LevelFollow : MonoBehaviour
    {
        public bool BossFight;
        [SerializeField] private Rigidbody playerRb;
        public LinkControl linkControl;
        public EvaluateScore evaluateScore;
        public InstantiatePointItem pointItemScript;
        public LevelAnims anims;
        public GrowthPalace growthPalace;
        public float levelTimeLeft;
        public float pathS11time, pathS12time, pathS13time, pathS14time;
        public GameObject levelModal;
        private float bonusTime;
        public GameObject pathMusic;
        public GameObject openLevel;
        public PathCreator currentPath;
        public PathCreator pathS11, pathS12, pathS13, pathS14;
        public EndOfPathInstruction endOfPathInstruction;
        public BoostBar boostBar;
        public GameObject paraloopInstantiator;
        [SerializeField] private int maxBoost;
        [SerializeField] private float speed = 5, gravityModifier = 0.2f;
        [SerializeField] private float nonBoostingSpeed, boostingSpeed, BoostAttemptSpeed;
        public float distanceTravelled, maxDistance;
        public bool continueLevel;
        [SerializeField] private bool isClosedPath;
        public StateController sc;
        public CameraFollow camera;
        [SerializeField] private Vector3 pathPosition, playerPosition;
        [SerializeField] private Vector3 pathRotation, playerRotation;
        [SerializeField] private Quaternion setRotation;

        public int grade;
        public int fullGrade;

        
                        ///Copied from the example project, probably useful to have
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


        void OnEnable()
        {
            bonusTime = levelModal.GetComponent<CustomModal>().timeBonus;
            playerRb = GetComponent<Rigidbody>();
            Physics.gravity *= gravityModifier;
            boostBar.SetMaxBoost(maxBoost);
            BoostGauge = 100;
            maxBoost = 100;

            levelSegmentNum = 1;
            chipReq = 50;
            score = 0;

            continueLevel = false;
            distanceTravelled = 0;
            
            speed = nonBoostingSpeed;
            playerRb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

            if(!BossFight)
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
        }
        void OnDisable()
        {
            if(pathMusic != null) pathMusic.SetActive(false);
            if(openLevel != null) openLevel.SetActive(true);

            growthPalace.ReturnAllIdeyas();
            growthPalace.UpdateStuff();
            currentPath.gameObject.SetActive(false);
        }
        public bool isBoosting, BoostAttempt;
        public float BoostGauge, BoostTime;
        public void Boosting(InputAction.CallbackContext context)
        {
            if(0 <= BoostGauge && context.started && isMoving(new Vector2(movingHorizontal, movingVertical)))
            {
                isBoosting = true;
                speed = boostingSpeed;
                BoostGauge -= 1;
                //Gamepad.current.SetMotorSpeeds(.5f, .6f);
                Sounds.PlayOneShot(BoostStart, 1.0f);
                //Sounds.PlayScheduled(AudioSettings.dspTime + BoostStart.length);
            }
            if(context.canceled)
            {
                isBoosting = false;
                speed = nonBoostingSpeed;
                //Gamepad.current.SetMotorSpeeds(.25f, .25f);
                Sounds.PlayOneShot(BoostEnd, 1.0f);
            }
            if(0 >= BoostGauge && context.started)
            {
                isBoosting = false;
                BoostAttempt = true;
                anims.mAnimator.SetTrigger("TrBoostAttempt");
                speed = nonBoostingSpeed;
                //Gamepad.current.SetMotorSpeeds(.25f, .25f);
            }
        }
        public float movingVertical, movingHorizontal, rotatingVertical, rotatingHorizontal;
        public void Rotating(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            movingHorizontal = direction.x;
            movingVertical = direction.y;
            //Debug.Log(movingVertical + "," + movingHorizontal);
        }
        
        void Update()
        {
            scoreText.text = score.ToString();
            timeText.text = levelTimeInt.ToString();
            chipText.text = chipCounter.ToString() + " / " + chipReq.ToString();
            boostBar.SetBoost((int)BoostGauge);

            
            if(!BossFight) levelTimeLeft -= Time.deltaTime;
            else levelTimeLeft += Time.deltaTime;
            levelTimeInt = (int)levelTimeLeft;
            
            if(levelTimeLeft < 0 && !BossFight)
            {
                sc.Activate1();
                currentPath.gameObject.SetActive(false);
                if(pathMusic != null) pathMusic.SetActive(false);
                if(openLevel != null) openLevel.SetActive(true);
                
                growthPalace.freedIdeas = 0;
                growthPalace.UpdateStuff();
                growthPalace.ReturnAllIdeyas();
            }
            

            //Paraloop
            if(speed >= nonBoostingSpeed)
                paraloopInstantiator.SetActive(true);
            else paraloopInstantiator.SetActive(false);
            

            //BoostStuffs
            if(isMoving(new Vector2(movingHorizontal, movingVertical)) && isBoosting)
            {
                BoostGauge -= Time.deltaTime * 10;
            }
            if(BoostGauge <= 0)
            {
                speed = nonBoostingSpeed;
                isBoosting = false;
            }
            
            //BoostAttempts
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
            
            //Links
            if(linkTimeLeft <= 0) LinkEmpty();
            if(linkActive) linkTimeLeft -= Time.deltaTime;
        }
        public float addForceMult;
        void FixedUpdate()
        {            
            distanceTravelled += movingHorizontal * speed * Time.deltaTime;

            pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            
            playerRb.MovePosition(new Vector3(pathPosition.x, transform.position.y, pathPosition.z));
            playerRb.AddForce(Vector3.up * movingVertical * addForceMult * speed, ForceMode.VelocityChange);

            //// The rest of this stuff deals with player rotation and needs to be revamped
            pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;


            playerRotation.x = transform.rotation.x; 
            playerRotation.y = pathRotation.y;          // Gets the Left/Right rotation of the track
            playerRotation.z = transform.rotation.z;

            setRotation.eulerAngles = playerRotation;   // Puts the Vector3 playerRotation into the Quaternion setRotation
            transform.rotation = setRotation;           // So that the transform.rotation can then be set to the Quaternion
            
            if(movingHorizontal < 0) // Checks if  you are moving backwards
            {
                //transform.rotation = new Quaternion(transform.rotation.x, -transform.rotation.y, transform.rotation.z, transform.rotation.w);
                Vector3 flippedRotation = new Vector3(transform.rotation.x, pathRotation.y + 180, transform.rotation.z);
                transform.rotation = Quaternion.Euler(flippedRotation); 
            }

            rotatingVertical = movingVertical;  // I dont know why i did this
                                                // I dont know how this line below works
            Vector3 verticalInput = new Vector3(transform.rotation.x, rotatingVertical * 2, transform.rotation.z);
            transform.forward += verticalInput; // I have no idea why this line is needed
                                                // But it all works so its all good
        }

        public TextMeshProUGUI scoreText, timeText, chipText;
        public int score, link, chipCounter, levelTimeInt;//public
        public int chipReq;
        public float linkTimeLeft;//public
        public float linkPitch, linkPitchIncrease;
        public bool power, linkActive;
        public bool isInGround;
        public AudioSource Sounds;
        [SerializeField] private AudioClip BoostStart, BoostIng, BoostEnd;
        [SerializeField] public AudioClip BlueChipSFX, YellowRingSFX, GreenRingSFX, HalfRingSFX, PowerRingSFX, SpikeRingSFX, DamageSFX, StunSFX;
        void OnTriggerEnter(Collider other)
        { 
            if(other.CompareTag("BlueChip"))
            {
                CollectBlueChip(other);
            }
            if(other.CompareTag("Star"))
            {
                CollectStarChip(other);
            }
            if(other.CompareTag("YellowRing"))
            {
                //pointItemScript.InstantiatePointAndChip(false);

                Sounds.pitch = linkPitch;
                Sounds.PlayOneShot(YellowRingSFX, 1.0f);

                if(BoostGauge <= 90) BoostGauge += 10;
                else BoostGauge = 100;

                score += 10 * link;
                LinkIncrease();
                other.GetComponent<RingYellow>().isCollected = true;
            }
            if(other.CompareTag("GreenRing"))
            {
                Sounds.pitch = 1;
                Sounds.PlayOneShot(GreenRingSFX, 1.0f);

                if(BoostGauge <= 90) BoostGauge += 10;
                else BoostGauge = 100;
            }
            if(other.CompareTag("HalfRing"))
            {
                //pointItemScript.InstantiatePointAndChip(false);
                
                Sounds.pitch = RandomPitch();
                Sounds.PlayOneShot(HalfRingSFX, 1.0f);

                if(BoostGauge <= 90) BoostGauge += 10;
                else BoostGauge = 100;

                score += 5 * link;
                LinkIncrease();
            }
            if(other.CompareTag("PowerRing"))
            {
                Sounds.pitch = 1;
                Sounds.PlayOneShot(PowerRingSFX, 1.0f);
                BoostGauge = 100;
                score += 50 * link;
                power = true;
                LinkIncrease();
            }
            if(other.CompareTag("SpikeRing"))
            {
                Sounds.pitch = 1;
                Sounds.PlayOneShot(SpikeRingSFX, 1.0f);
                BoostGauge -= 5;
                score -= 50;
                takeDamage();
            }

            if(other.CompareTag("GrowthPalace") && continueLevel)
            {
                Debug.Log("Continue Level");
                ContinueLevel();
            }
            if(BossFight)
            {
                if(other.CompareTag("JHands Stn"))
                {
                    StartCoroutine(Stun());
                }
                if(other.CompareTag("JHands Dmg"))
                {
                    takeDamage();
                }
            }
        }
        public void LinkIncrease()
        {
            linkTimeLeft = 1;
            link += 1;
            linkActive = true;
            linkPitch += linkPitchIncrease;

            if(linkControl != null)
            {
                linkControl.link = link;
                linkControl.RunLinkIncrease();
            }
        }
        void LinkEmpty()
        {
            linkTimeLeft = 0;
            link = 0;
            linkPitch = 1 - linkPitchIncrease;
            linkActive = false;
            if(linkControl != null)
            {
                linkControl.link = link;
                linkControl.RunLinkIncrease();
            }
        }

        public int levelSegmentNum;
        void ContinueLevel()
        {
            grade = evaluateScore.CalculateGrade(levelSegmentNum, score);
            levelSegmentNum++;
            distanceTravelled = 0;
            continueLevel = false;
            chipCounter = 0;
            score = 0;
            if(levelSegmentNum == 2)
            {
                pathS11.gameObject.SetActive(false);
                pathS12.gameObject.SetActive(true);
                currentPath = pathS12;
                levelTimeLeft = pathS12time + bonusTime;;
                camera.pathCreator = pathS12;
            }
            if(levelSegmentNum == 3)
            {
                pathS12.gameObject.SetActive(false);
                pathS13.gameObject.SetActive(true);
                currentPath = pathS13;
                levelTimeLeft = pathS13time + bonusTime;;
                camera.pathCreator = pathS13;
            }
            if(levelSegmentNum == 4)
            {
                pathS13.gameObject.SetActive(false);
                pathS14.gameObject.SetActive(true);
                currentPath = pathS14;
                levelTimeLeft = pathS14time + bonusTime;;
                camera.pathCreator = pathS14;
            }
            if(levelSegmentNum >= 5)
            {
                fullGrade = evaluateScore.CalculateFullGrade();
                pathS14.gameObject.SetActive(false);
                growthPalace.accessBossFight = true;
                sc.Activate1();
                growthPalace.UpdateStuff();
            }
        }

        float RandomPitch()
        {
            float num = Random.Range(100, 100);
            num *= 0.01f;
            return num;
        }

        public void CollectBlueChip(Collider other)
        {
            pointItemScript.InstantiatePointAndChip(true);
            Sounds.pitch = 1;
            Sounds.PlayOneShot(BlueChipSFX, 1.0f);
            other.gameObject.SetActive(false);
            score += 10 * link;
            chipCounter += 1;

            if(continueLevel) score += 10;
        }
        public void CollectStarChip(Collider other)
        {
            Sounds.pitch = 1;
            Sounds.PlayOneShot(BlueChipSFX, 1.0f);
            other.gameObject.SetActive(false);
            score += 10 * link;
            levelTimeLeft += 0.25f;
        }

        public void takeDamage()
        {
            StartCoroutine(TakeDamage(Vector3.up, 10));
        }
        public float stunDir;
        IEnumerator TakeDamage(Vector3 knockbackDir, float knockbackPower)
        {
            speed = 0;
            if(!BossFight) levelTimeLeft -= 5f;
            else levelTimeLeft += 5f;
            playerRb.AddForce(knockbackDir * knockbackPower, ForceMode.Impulse);
            Sounds.PlayOneShot(DamageSFX, 1.0f);
            float t = 0;
            bool stunned = true;
            while (stunned)
            {
                //Play animation
                t += 0.01f;
                speed = 0;
                yield return new WaitForSeconds(0.01f);
                if(t > stunDir)
                {
                    stunned = false;
                    speed = nonBoostingSpeed;
                }
            }
        }
        IEnumerator Stun()
        {
            speed = 0;
            Sounds.PlayOneShot(StunSFX, 1.0f);
            float t = 0;
            bool stunned = true;
            while (stunned)
            {
                //Play animation
                t += 0.01f;
                speed = 0;
                yield return new WaitForSeconds(0.01f);
                if(t > stunDir)
                {
                    stunned = false;
                    speed = nonBoostingSpeed;
                }
            }
        }

        public bool isMoving(Vector2 direction)
        {
            if(direction != Vector2.zero)
                 return true;
            else
            {
                isBoosting = false;
                return false;
            }
        }

    }
}