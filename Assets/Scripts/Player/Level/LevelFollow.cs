using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using PathCreation;

namespace PathCreation.Examples
{
    public class LevelFollow : MonoBehaviour
    {
        public bool BossFight;
        [SerializeField] private Rigidbody playerRb;
        public LevelAnims anims;
        public float levelTimeLeft;
        public float pathS11time, pathS12time, pathS13time, pathS14time;
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

        

        void Start()
        {
            if (currentPath != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                currentPath.pathUpdated += OnPathChanged;
            }
        }
        void OnEnable()
        {
            levelSegmentNum = 1;
            chipReq = 50;
            continueLevel = false;
            speed = nonBoostingSpeed;
            playerRb = GetComponent<Rigidbody>();
            boostBar.SetMaxBoost(maxBoost);
            Physics.gravity *= gravityModifier;

            BoostGauge = 100;
            maxBoost = 100;

            playerRb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            score = 0;

            if(!BossFight)
            {
                pathS11.gameObject.SetActive(true);
                currentPath = pathS11;
                levelTimeLeft = pathS11time;
            }
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
            }
            

            //Paraloop
            if(speed >= nonBoostingSpeed && movingHorizontal != 0)
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
            if(linkTimeLeft <= 0)
            {
                LinkEmpty();
            }
        }
        public float addForceMult;
        void FixedUpdate()
        {            
            distanceTravelled += movingHorizontal * speed * Time.deltaTime;

            pathPosition = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            
            playerRb.MovePosition(new Vector3(pathPosition.x, transform.position.y, pathPosition.z));
            playerRb.AddForce(Vector3.up * movingVertical * addForceMult * speed, ForceMode.VelocityChange);

            pathRotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction).eulerAngles;


            playerRotation.x = transform.rotation.x;
            playerRotation.y = pathRotation.y;
            playerRotation.z = transform.rotation.z;

            setRotation.eulerAngles = playerRotation;
            transform.rotation = setRotation;
            
            if(movingHorizontal < 0)
            {
                //transform.rotation = new Quaternion(transform.rotation.x, -transform.rotation.y, transform.rotation.z, transform.rotation.w);
                Vector3 flippedRotation = new Vector3(transform.rotation.x, pathRotation.y + 180, transform.rotation.z);
                transform.rotation = Quaternion.Euler(flippedRotation); 
            }

            rotatingVertical = movingVertical;
            
            Vector3 verticalInput = new Vector3(transform.rotation.x, rotatingVertical * 2, transform.rotation.z);
            transform.forward += verticalInput;


            if(isClosedPath && distanceTravelled >= maxDistance)
            {
                distanceTravelled -= maxDistance;
            }
            
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = currentPath.path.GetClosestDistanceAlongPath(transform.position);
        }

        public TextMeshProUGUI scoreText, timeText, chipText;
        public int score, link, chipCounter, levelTimeInt;//public
        public int chipReq;
        public float linkTimeLeft;//public
        public bool power, linkActive;
        public bool isInGround;
        public AudioSource Sounds;
        [SerializeField] private AudioClip BoostStart, BoostIng, BoostEnd;
        [SerializeField] public AudioClip BlueChipSFX, YellowRingSFX, GreenRingSFX, HalfRingSFX, PowerRingSFX, SpikeRingSFX, DamageSFX, StunSFX;
        void OnTriggerEnter(Collider other)
        { 
            if(other.gameObject.CompareTag("BlueChip"))
            {
                CollectBlueChip(other);
            }
            if(other.gameObject.CompareTag("Star"))
            {
                CollectStarChip(other);
            }
            if(other.gameObject.CompareTag("YellowRing"))
            {
                Sounds.pitch = RandomPitch();
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

                Sounds.pitch = 1;
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
                Sounds.pitch = RandomPitch();
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
                Sounds.pitch = 1;
                Sounds.PlayOneShot(PowerRingSFX, 1.0f);
                BoostGauge = 100;
                score += 250;
                power = true;
                LinkIncrease();
            }
            if(other.gameObject.CompareTag("SpikeRing"))
            {
                Sounds.pitch = 1;
                Sounds.PlayOneShot(SpikeRingSFX, 1.0f);
                BoostGauge -= 5;
                score -= 100;
            }

            if(other.gameObject.CompareTag("GrowthPalace") && continueLevel)
            {
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

        public int levelSegmentNum;
        void ContinueLevel()
        {
            levelSegmentNum++;
            distanceTravelled = 0;
            continueLevel = false;
            chipCounter = 0;
            if(levelSegmentNum == 2)
            {
                pathS11.gameObject.SetActive(false);
                pathS12.gameObject.SetActive(true);
                currentPath = pathS12;
                levelTimeLeft = pathS12time;
                camera.pathCreator = pathS12;
            }
            if(levelSegmentNum == 3)
            {
                pathS12.gameObject.SetActive(false);
                pathS13.gameObject.SetActive(true);
                currentPath = pathS13;
                levelTimeLeft = pathS13time;
                camera.pathCreator = pathS13;
            }
            if(levelSegmentNum == 4)
            {
                pathS13.gameObject.SetActive(false);
                pathS14.gameObject.SetActive(true);
                currentPath = pathS14;
                levelTimeLeft = pathS14time;
                camera.pathCreator = pathS14;
            }
            if(levelSegmentNum >= 5)
            {
                sc.Activate1();
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
            Sounds.pitch = 1;
            Sounds.PlayOneShot(BlueChipSFX, 1.0f);
            other.gameObject.SetActive(false);
            score += 10;
            chipCounter += 1;

            LinkIncrease();
            if(continueLevel) score += 10;
        }
        public void CollectStarChip(Collider other)
        {
            Sounds.pitch = 1;
            Sounds.PlayOneShot(BlueChipSFX, 1.0f);
            other.gameObject.SetActive(false);
            score += 10;
            levelTimeLeft += 0.25f;
            LinkIncrease();
        }

        public void takeDamage()
        {
            StartCoroutine(TakeDamage(Vector3.up, 10));
        }

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
                if(t > 0.75f)
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
                if(t > 1.25f)
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