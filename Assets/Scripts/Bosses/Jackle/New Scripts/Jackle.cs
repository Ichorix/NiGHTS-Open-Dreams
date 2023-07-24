using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackle : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Rigidbody rigidbody;
    public SkinnedMeshRenderer meshRenderer;
    public SceneFade sceneFade;
    public MainMenuSimplified mmS;
    public GameObject sawParticles;
    public JackleHands leftHandScript;
    public JackleHands rightHandScript;
    public GameObject target;
    public AnimationCurve curve, damagedCurve;

    [Header("Information")]
    public float speed;
    public bool leftHandContributesHealth;
    public bool rightHandContributesHealth;
    public JackleHands chosenHand;
    public JackleHands otherHand;
    public int _health = 15; //phase 1 : 5; phase 3 : 5; phase 5 : 5;
    public int _Phase;
    public bool isVulnerable;
    public bool sawWhenNear;
    public bool isSawing;
    public bool playerIsNear;
    public bool crossSawAttack;
    public float crossSawAttackTime;

    public bool teleportIng;
    public float outTeleportTime;
    public float inTeleportTime;

    public bool playerIsLeft;
    public bool playerIsRight;
    public Transform bounding1, bounding2;

    private float rotationPower;
    private float _time;
    private float timeBetweenMove = 1;

    private Vector3 currentPos;
    private Vector3 goToPos;
    private Quaternion rotBeforeTele;

    void Start()
    {
        rotBeforeTele = transform.rotation;
        currentPos = transform.position;
        goToPos = transform.position;
        _health = 15;

    }
    void Update()
    {
        _time += Time.deltaTime * speed;
        //transform.position = Vector3.Lerp(currentPos, goToPos, curve.Evaluate(_time));
        transform.position = Vector3.MoveTowards(currentPos, goToPos, curve.Evaluate(_time));

        if(!leftHandScript.isAvailable) leftHandContributesHealth = false;
        if(leftHandScript.stunned) leftHandContributesHealth = false;
        if(leftHandScript.isAvailable && !leftHandScript.stunned) leftHandContributesHealth = true;

        if(!rightHandScript.isAvailable) rightHandContributesHealth = false;
        if(rightHandScript.stunned) rightHandContributesHealth = false;
        if(rightHandScript.isAvailable && !rightHandScript.stunned) rightHandContributesHealth = true;
        
        if(isSawing && leftHandContributesHealth && rightHandContributesHealth)
        {
            sawParticles.SetActive(true);
        }
        else sawParticles.SetActive(false);

        if(teleportIng)
        {
            outTeleportTime += Time.deltaTime;
            if(outTeleportTime > 0.4f && outTeleportTime < 0.55f)
            {
                float shrinkFactor = 1f;
                shrinkFactor += Time.deltaTime * 5;

                transform.localScale /= shrinkFactor;
            }
            if(outTeleportTime > 0.55f && outTeleportTime < 0.66f)
            {
                meshRenderer.enabled = false;
                leftHandScript.active = false;
                rightHandScript.active = false;
            }
            if(outTeleportTime > 0.66f && outTeleportTime < 1)
            {
                goToPos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);///Testing Location; Replace Location Later
            }
            if(outTeleportTime > 1)
            {
                _time = 1;
                inTeleportTime += Time.deltaTime;
                meshRenderer.enabled = true;
                leftHandScript.active = true;
                rightHandScript.active = true;
            }
            if(inTeleportTime > 0.2f && inTeleportTime < 0.35f)
            {
                animator.SetTrigger("TrUnCloak");
                rigidbody.AddTorque(Vector3.up * rotationPower * 0.1f, ForceMode.Impulse);

                float shrinkFactor = 1f;
                shrinkFactor += Time.deltaTime * 5;

                transform.localScale *= shrinkFactor;
            }
            if(inTeleportTime > 0.35f)
            {
                outTeleportTime = 0;
                teleportIng = false;
            }

        }
        else
        {
            transform.localScale = Vector3.one;
            transform.rotation = rotBeforeTele;
        }
        

        //////////BOSS PHASES//////////
    }

    /*
        public Vector2 randomRangePos;                  // (chosenX, chosenY)            /// ( -14, 6)                  //// if(chosenX < 0) minX + -chosenX; if(chosenX > 0) maxX + -chosenX;
        public Vector2 maximumXRange, maximumYRange;    // (min X, max X) (min Y, max Y) /// (-100, 100) (-100, 100)    //// if(randomRangePos.x < 0) maximumXRange.x + -randomRangePos.x; if(randomRangePos.x > 0) maximumXRange.y + -randomRangePos.x;
        public float minXBounding, maxXBounding, minYBounding, maxYBounding;                                            //// if(randomRangePos.y < 0) maximumYRange.x + -randomRangePos.y; if(randomRangePos.Y > 0) maximumYRange.y + -randomRangePos.y; 
    */

    //////////Send Attacks//////////
    /*IEnumerator MoveNormal()
    {
        while(true)
        {
            _time = 0;

            float randomX = Random.Range(bounding1.position.x, bounding2.position.x);
            float randomY = Random.Range(bounding1.position.y, bounding2.position.y);
            float randomZ = Random.Range(bounding1.position.z, bounding2.position.z);

            currentPos = transform.position;
            goToPos = new Vector3(randomX, randomY, randomZ);

            yield return new WaitForSeconds(timeBetweenMove);
        }
    }
    IEnumerator SendGrabLoop(float timeBetweenSend)
    {
        while(true)
        {
            Debug.Log("SendGrabLoop");
            ChooseHand();
            if(chosenHand == null) Debug.Log("Chosen Hand is Null!"); 
            if(chosenHand != null) chosenHand.Grab();
            yield return new WaitForSeconds(timeBetweenSend);
            if(chosenHand != null) chosenHand.Return();
            yield return new WaitForSeconds(1.33f);
        }
    }
    IEnumerator SendPunchLoop(float timeBetweenSend, bool switchBetweenThrows, bool isBatchShot, int batchNum)
    {
        while(switchBetweenThrows)
        {
            Debug.Log("SendPunchLoop isSwitch");
            ChooseHand();
            chosenHand.Punch(false, 3);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            ChooseHand();
            otherHand.Return();
            chosenHand.Punch(false, 3);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            ChooseHand();
            otherHand.Return();
        }
        while(!switchBetweenThrows)
        {
            Debug.Log("SendPunchLoop !isSwitch");
            ChooseHand();
            chosenHand.Punch(false, 3);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
        }
    }
    IEnumerator DoublePunchLoop(float timeBetweenSend, bool isBatchShot)
    {
        while(!isBatchShot)
        {
            Debug.Log("SendD-PunchLoop !isBatch");
            ChooseHand();
            chosenHand.Punch(false, 3);
            yield return new WaitForSeconds(timeBetweenSend);
            DoublePunch();
            yield return new WaitForSeconds(timeBetweenSend);
        }
        while(isBatchShot)
        {
            Debug.Log("SendD-PunchLoop isBatch");
            ChooseHand();
            chosenHand.Punch(false, 3);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            DoublePunch();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            otherHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Punch(false, 3);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 2);
        }
    }*/
    IEnumerator TarotCards(float timeBetweenSend)/////WIP
    {
        while(true)
        {
            TarotCardsLaunch();
            yield return new WaitForSeconds(timeBetweenSend);
        }
    }
    
    void Move()
    {
        _time = 0;

        float randomX = Random.Range(bounding1.position.x, bounding2.position.x);
        float randomY = Random.Range(bounding1.position.y, bounding2.position.y);
        float randomZ = Random.Range(bounding1.position.z, bounding2.position.z);

        currentPos = transform.position;
        goToPos = new Vector3(randomX, randomY, randomZ);
    }
    void SendGrab()
    {
        Debug.Log("Send Grab");
        //EndSaw();
        ChooseHand();
        if(chosenHand == null) Debug.Log("Chosen Hand is Null!"); 
        else chosenHand.Grab();
    }
    void SendPunch(float aimingTime)
    {
        Debug.Log("SendPunch");
        //EndSaw();
        ChooseHand();
        chosenHand.Punch(false, aimingTime);
    }
    void SendDoublePunch(float aimingTime)
    {
        Debug.Log("Send DoublePunch");
        EndSaw();
        ChooseHand();
        DoublePunch(aimingTime);
    }
    void SendSawHand()
    {
        Debug.Log("SendSaw");
        ChooseHand();
        chosenHand.BounceSaw(chosenHand.isTop);
    }
    void SendDoubleSaw()
    {
        Debug.Log("DoubleBounce");
        ChooseHand();
        chosenHand.BounceSaw(chosenHand.isTop);
        otherHand.BounceSaw(otherHand.isTop);
    }

    //////////Functions//////////
    void DoublePunch(float aimingTime)
    {
        leftHandScript.Punch(true, aimingTime);
        rightHandScript.Punch(true, aimingTime);
    }
    void DoubleReturn()
    {
        if(isSawing) EndSaw();

        leftHandScript.UnStun();
        rightHandScript.UnStun();
        leftHandScript.Return();
        rightHandScript.Return();
    }
    void SawIt()
    {
        isSawing = true;
        
        leftHandScript.Return();
        rightHandScript.Return();

        leftHandScript.HandSaw();
        rightHandScript.HandSaw();
    }
    void EndSaw()
    {
        isSawing = false;

        leftHandScript.Return();
        rightHandScript.Return();
    }
    void Teleport(bool setLocation, Vector3 theLocation)/////Mostly finished, currently just teleports to the right tho
    {
        rotBeforeTele = transform.rotation;
        animator.SetTrigger("TrCloak");
        rigidbody.AddTorque(Vector3.up * rotationPower, ForceMode.Impulse);
        teleportIng = true;
        outTeleportTime = 0;
        inTeleportTime = 0;
    }
    void Disappear()
    {
        StopAllCoroutines();
        rotBeforeTele = transform.rotation;
        animator.SetTrigger("TrCloak");
        rigidbody.AddTorque(Vector3.up * rotationPower, ForceMode.Impulse);
        outTeleportTime = 0;
        inTeleportTime = 0;
        StartCoroutine(Defeated());
    }
    IEnumerator Defeated()
    {
        sceneFade.BeginFade(1);
        yield return new WaitForSeconds(1f);
        //mmS.PlayClicked();
        Debug.Log("Load Scene");
    }
    public void Damage()
    {
        if(isVulnerable)
        {
            if(oneHit)
            {
                RealDamage();
                oneHit = false;
            }
            else if(leftHandContributesHealth)
                leftHandScript.Stunned();

            else if(rightHandContributesHealth)
                rightHandScript.Stunned();

            else
                RealDamage();
        }
    }
    void RealDamage()
    {
        StopAllCoroutines();
        DoubleReturn();
        isVulnerable = false;
        _health--;
        StartCoroutine(Stretch());
    }
    void TarotCardsLaunch()////WIP
    {
        Debug.Log("IDK how to do this yet");
    }

    void ChooseHand() ////Working? idk it hasnt made any issues yet ///It was working fine but now it doesnt detect the player?
    {                   ///Wait the player didnt have a tag one moment
        bool canBeLeft = false;
        bool canBeRight = false;

        if(playerIsLeft)
        {
            canBeLeft = true;
        }
        if(playerIsRight)
        {
            canBeRight = true;
        }

        if(!leftHandScript.isAvailable)
        {
            canBeLeft = false;
            if(rightHandScript.isAvailable)
            {
                canBeRight = true;
            }
        }
        if(!rightHandScript.isAvailable)
        {
            canBeRight = false;
            if(leftHandScript.isAvailable)
            {
                canBeLeft = true;
            }
        }

        if(canBeLeft) ////"Use of unassigned local variable" its mad that im using something that is currently null? ///I think i fixed it but i dont remember
        {
            chosenHand = leftHandScript;////^^^About that, idk but rn i need to work on the collision and the rest ***shouuuld*** be working because the only issue is it not knowing which side the player is
            if(!canBeRight)
            {
                otherHand = rightHandScript;//// Im back to this because all of a sudden it just doesnt pick a hand; Maybe something got messed up between the communication of the Colliders
            }                               //// Found it; Collider detection was still checking for "Player" not "levelPlayer"
        }
        if(canBeRight)
        {
            chosenHand = rightHandScript;
            if(!canBeLeft)
            {
                otherHand = leftHandScript;
            }
        }

        if(chosenHand == null)
        {
            int randomhand = Random.Range(0,2);
            if (randomhand == 1)
            {
                chosenHand = leftHandScript;
                otherHand = rightHandScript;
            }
            if(randomhand == 2)
            {
                chosenHand = rightHandScript;
                otherHand = leftHandScript;
            }
        }
        Debug.Log("Hand Chosen As" + chosenHand + "With other Hand as" + otherHand);
    }

    //////////BOSS PHASES//////////
    IEnumerator Stretch()
    {
        float t = 0;
        while (true)
        {
            float num = curve.Evaluate(t);
            Vector3 vec = new Vector3(num, 1.25f * num, transform.localScale.z);
            transform.localScale = vec;

            t += 0.01f;
            
                
            if(t > 1)
                if (_health <= 1)
                    StartCoroutine(Dieded());
                else
                    TEnum();

            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator Dieded()
    {
        float t = 0;
        bool goinUp = true;
        leftHandScript.Diedededed();
        rightHandScript.Diedededed();
        //Turn on the Particle system
        Debug.Log("Particle System");
        yield return new WaitForSeconds(2);
        //Activate the Explosion VFX Graph or smth
        Debug.Log("Explosion");
        yield return new WaitForSeconds(2);
        Disappear();
    }
    void TEnum()
    {
        StopAllCoroutines();
        StartCoroutine(TeleportEnum());
    }
    IEnumerator TeleportEnum()
    {
        Teleport(false, Vector3.zero);
        yield return new WaitForSeconds(3);

        if(_health == 14) StartCoroutine(Phase14());
        else if(_health == 13) StartCoroutine(Phase13());
        else if(_health == 12) StartCoroutine(Phase12());
        else if(_health == 11) StartCoroutine(Phase11());
        else if(_health == 10) StartCoroutine(Phase9());
        else if(_health == 9) StartCoroutine(Phase8());
        else if(_health == 8) StartCoroutine(Phase7());
        else if(_health <= 7) StartCoroutine(Phase5());

        yield return new WaitForSeconds(2);
        isVulnerable = true;
    }

    ////////Overall Phase 1////////    
    IEnumerator Phase15() //Idle in place until first attacked, then teleport away.
    {
        _Phase = 15;
        while (true)
        {
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
        }
    }
    IEnumerator Phase14() //send a hand to grab; if hit then teleport; if too long send hand back
    {
        _Phase = 14;
        while(true)
        {
            Debug.Log("Phase 14");
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            SendGrab();
            yield return new WaitForSeconds(timeBetweenMove/2);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            chosenHand.Return();
            yield return new WaitForSeconds(timeBetweenMove);
        }
    }
    IEnumerator Phase13() //if far then send punch. if near, then activate sawblade
    {
        _Phase = 13;
        while (true)
        {
            Debug.Log("Phase 13");
            if(!playerIsNear)
            {
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                SendPunch(2);
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);          
            }
            else
            {
                SawIt();
                yield return new WaitForSeconds(5);
                EndSaw();
            }
        }
    }
    IEnumerator Phase12() //if far send grab
    {
        _Phase = 12;
        while (true)
        {
            Debug.Log("Phase 12");
            if(!playerIsNear)
            {
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                SendGrab();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                Move();
                yield return new WaitForSeconds(timeBetweenMove);
                chosenHand.Return();
                
            }
            else
            {
                SawIt();
                yield return new WaitForSeconds(5);
                EndSaw();
            }
        }
    }
    ///////NEW TIMINGS; TEST THIS
    IEnumerator Phase11() //send grab followed by 4 fast punches, then wait
    {
        _Phase = 11;
        while(true)
        {
            Debug.Log("Phase 11");
            Move();
            yield return new WaitForSeconds(timeBetweenMove/2);
            SendGrab();
            yield return new WaitForSeconds(timeBetweenMove/2);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            DoubleReturn();
            Move();
            SendPunch(1);
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            SendPunch(1);
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            SendPunch(1);
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            SendPunch(1);
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(5);
        }
    }
    ////////Overall Phase 2////////  Move to the next section
    ////////Overall Phase 3////////  ~ Idle moves further and faster
    public bool oneHit;
    IEnumerator Phase9() //Fly with double sawblade until attacked
    {
        _Phase = 9;
        oneHit = true;
        SawIt();
        while (true)
        {
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
        }
    }
    /////NEW TIMINGS; TEST THIS
    IEnumerator Phase8() //Send single punch, double punch, single punch, wait.
    {
        _Phase = 8;
        oneHit = false;
        while (true)
        {   
            Move();
            SendPunch(1.5f);
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            SendDoublePunch(1.5f);
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            SendPunch(1.5f);
            yield return new WaitForSeconds(5);
        }
    }
    ////// MAKE THE HANDS RETURN EVERY ONCE IN A WHILE, ELSE BOUNCE HAND JUST LEAVES
    IEnumerator Phase7() //Send 1 bouncing saw hand and 1 following grab hand.
    {
        _Phase = 7;
        while (true)
        {
            SendSawHand();
            SendGrab();

            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            DoubleReturn();
            Move();
            yield return new WaitForSeconds(5);
        }
    }
    //////NEED TO BE DONE
    IEnumerator Phase6() //move back. Send both hands as bouncing saws that make an XXXXXXX pattern on the screen.
    {
        _Phase = 6;
        //Double Bouncing Hand phase
        yield return new WaitForSeconds(5);
    }

    //////NEW TIMINGS ; TEST THIS //////////ADD BOUNCING HAND
    IEnumerator Phase5() //go forward and send a bouncing saw hand along with a consistent slow punch.
    {
        _Phase = 5;
        while (true)
        {
            SendSawHand();
            SendPunch(2);

            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            //Fires Here
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            SendPunch(2);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            //FiresHere
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
            DoubleReturn();
            Move();
            yield return new WaitForSeconds(timeBetweenMove);
        }
    }
    ////////Overall Phase 4//////// Move to the next section, Tarot cards do EOL/DOG style grid attacks
    ////////Overall Phase 5//////// Rush to get 5 hits % Both hands bouncing saw and much faster now, Tarot Cards attacking every other second
    
}
