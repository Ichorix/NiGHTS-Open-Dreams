using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jackle : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rigidbody;
    public SkinnedMeshRenderer meshRenderer;
    public GameObject sawParticles;
    public JackleHands leftHandScript;
    public JackleHands rightHandScript;
    public bool leftHandContributesHealth;
    public bool rightHandContributesHealth;
    public JackleHands chosenHand;
    public JackleHands otherHand;
    public GameObject target;

    public float speed;
    public float rotationPower;
    public float _time;
    public float timeBetweenMove;

    public Vector3 currentPos;
    public Vector3 goToPos;
    public AnimationCurve curve, damagedCurve;

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

    private IEnumerator moveEnumerator;

    //public ColliderDetection leftZone;
    public bool playerIsLeft;
    //public ColliderDetection rightZone;
    public bool playerIsRight;


    void Start()
    {
        rotBeforeTele = transform.rotation;
        currentPos = transform.position;
        goToPos = transform.position;
        moveEnumerator = MoveNormal();
        _health = 15;

        StartCoroutine(moveEnumerator);
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
        
        if(isSawing && leftHandContributesHealth && rightHandContributesHealth) sawParticles.SetActive(true);
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
        if(sawWhenNear && playerIsNear)
        {
            //Debug.Log("Sawwww"); Moved to ColliderDetection.cs
        }



        //////////Debug Inputs//////////
        if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Damage");
            Damage();
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Teleport");
            Teleport(false, Vector3.zero);
        }
    }

    /*
        public Vector2 randomRangePos;                  // (chosenX, chosenY)            /// ( -14, 6)                  //// if(chosenX < 0) minX + -chosenX; if(chosenX > 0) maxX + -chosenX;
        public Vector2 maximumXRange, maximumYRange;    // (min X, max X) (min Y, max Y) /// (-100, 100) (-100, 100)    //// if(randomRangePos.x < 0) maximumXRange.x + -randomRangePos.x; if(randomRangePos.x > 0) maximumXRange.y + -randomRangePos.x;
        public float minXBounding, maxXBounding, minYBounding, maxYBounding;                                            //// if(randomRangePos.y < 0) maximumYRange.x + -randomRangePos.y; if(randomRangePos.Y > 0) maximumYRange.y + -randomRangePos.y; 
    */
    public Transform bounding1, bounding2;

    IEnumerator MoveNormal()
    {
        while(true)
        {
            //Debug.Log("MoveNormal");
            isVulnerable = true;
            _time = 0;

            /*
                float minX = -20; 
                float maxX = 20;
                if(minX > maximumXRange.x) minX = maximumXRange.x; Debug.Log(minX);
                if(maxX < maximumXRange.y) maxX = maximumXRange.y; Debug.Log(maxX);
                float randomX = Random.Range(minX, maxX);
                float minY = -20;
                float maxY = 20;
                if(minY > maximumYRange.x) minY = maximumYRange.x; Debug.Log(minY);
                if(maxY < maximumYRange.y) maxY = maximumYRange.y; Debug.Log(maxY);
                float randomY = Random.Range(minY, maxY);

                randomRangePos = new Vector2(randomX, randomY);/// Use this to make the "bounding area" for jackle

                if(randomRangePos.x < 0) maximumXRange.x += -randomRangePos.x;
                if(randomRangePos.x > 0) maximumXRange.y += -randomRangePos.x;

                if(randomRangePos.y < 0) maximumYRange.x += -randomRangePos.y;
                if(randomRangePos.y > 0) maximumYRange.y += -randomRangePos.y;

                randomX *= 0.1f;
                randomY *= 0.1f;
            */

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
            ChooseHand();
            chosenHand.Grab();
            yield return new WaitForSeconds(timeBetweenSend);
            chosenHand.Return();
            yield return new WaitForSeconds(1.33f);
        }
    }
    IEnumerator SendPunchLoop(float timeBetweenSend, bool switchBetweenThrows, bool isBatchShot, int batchNum)
    {
        while(switchBetweenThrows)
        {
            ChooseHand();
            chosenHand.Punch(false);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            ChooseHand();
            otherHand.Return();
            chosenHand.Punch(false);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            ChooseHand();
            otherHand.Return();
        }
        while(!switchBetweenThrows)
        {
            ChooseHand();
            chosenHand.Punch(false);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
        }
    }
    IEnumerator DoublePunchLoop(float timeBetweenSend, bool isBatchShot)
    {
        while(!isBatchShot)
        {
            ChooseHand();
            chosenHand.Punch(false);
            yield return new WaitForSeconds(timeBetweenSend);
            DoublePunch();
            yield return new WaitForSeconds(timeBetweenSend);
        }
        while(isBatchShot)
        {
            ChooseHand();
            chosenHand.Punch(false);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            DoublePunch();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            otherHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Punch(false);
            yield return new WaitForSeconds(timeBetweenSend * 0.5f);
            chosenHand.Return();
            yield return new WaitForSeconds(timeBetweenSend * 2);
        }
    }
    IEnumerator TarotCards(float timeBetweenSend)/////WIP
    {
        while(true)
        {
            TarotCardsLaunch();
            yield return new WaitForSeconds(timeBetweenSend);
        }
    }
    

    //////////Functions//////////
    void DoublePunch()
    {
        leftHandScript.Punch(true);
        rightHandScript.Punch(true);
    }
    public Quaternion rotBeforeTele;
    void Teleport(bool setLocation, Vector3 theLocation)/////Mostly finished, currently just teleports to the right tho
    {
        rotBeforeTele = transform.rotation;
        animator.SetTrigger("TrCloak");
        rigidbody.AddTorque(Vector3.up * rotationPower, ForceMode.Impulse);
        teleportIng = true;
        outTeleportTime = 0;
        inTeleportTime = 0;
    }
    public void SawIt(bool keepMoving)
    {
        isSawing = true;
        StopAllCoroutines();
        if(keepMoving) StartCoroutine(moveEnumerator);
        
        leftHandScript.Return();
        rightHandScript.Return();

        leftHandScript.HandSaw();
        rightHandScript.HandSaw();
    }
    public void Damage()
    {
        if(isVulnerable)
        {
            if(leftHandContributesHealth)
            {
                leftHandScript.Stunned();
            }
            else if(rightHandContributesHealth)
            {
                rightHandScript.Stunned();
            }
            else
            {
                RealDamage();
            }
        }
    }
    void RealDamage()
    {
        StopAllCoroutines();
        _health--;
        StartCoroutine(Stretch());
    }
    void TarotCardsLaunch()////WIP
    {
        Debug.Log("IDK how to do this yet");
    }

    void ChooseHand() ////Working? idk it hasnt made any issues yet ///It was working fine but now it doesnt detect the player?
    {                   ///Wait the player didnt have a tag one moment
        Debug.Log("ChooseHand");

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
                otherHand = rightHandScript;
            }
        }
        if(canBeRight)
        {
            chosenHand = rightHandScript;
            if(!canBeLeft)
            {
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
            Debug.Log(num);

            t += 0.01f;
            if(t > 1) TEnum();

            yield return new WaitForSeconds(0.01f);
        }
    }
    void TEnum()
    {
        StopAllCoroutines();
        StartCoroutine(TeleportEnum());
    }
    IEnumerator TeleportEnum()
    {
        Teleport(false, Vector3.zero);
        isVulnerable = false;
        yield return new WaitForSeconds(3);

        if(_health == 14)//Working Here
        {
            //StartCoroutine(OverallPhase1Start());
            Phase14();
        }
        if(_health == 13)
        {
            Phase13();
        }
        if(_health == 12)
        {
            Phase12();
        }
        if(_health == 11)
        {
            Phase11();
        }
        if(_health == 10)
        {
            Phase10();
        }
        if(_health == 9)
        {
            Phase9();
        }
        if(_health == 8)
        {
            Phase8();
        }
        if(_health == 7)
        {
            Phase7();
        }
        if(_health == 6)
        {
            Phase6();
        }
        if(_health == 5)
        {
            Phase5();
        }
        if(_health == 4)
        {
            Phase4();
        }
        if(_health == 3)
        {
            FinalPhase();
        }
        yield return new WaitForSeconds(2);
        isVulnerable = true;
    }

    ////////Overall Phase 1////////    
    void Phase14()
    {
        _Phase = 14;
        Debug.LogWarning("Phase 14");
        StartCoroutine(moveEnumerator);

        leftHandScript.Return();
        rightHandScript.Return();

        ChooseHand();
        StartCoroutine(SendGrabLoop(5));
    }
    public void Phase13()
    {
        _Phase = 13;
        Debug.LogWarning("Phase 13");
        sawWhenNear = true;
        StartCoroutine(moveEnumerator);

        leftHandScript.Return();
        rightHandScript.Return();

        ChooseHand();
        StartCoroutine(SendPunchLoop(15, true, false, 0));
    }
    public void Phase12()///Fixed       ///Figure out a way to make it so that it waits a second before the grab so that the hand can return
    {                                   ///The saw should fix this ^^^
        _Phase = 12;                    ///What saw? dont you mean the teleport
        Debug.LogWarning("Phase 12");   ///Plus the teleport sure did work we dotnt have any more issues
        StartCoroutine(moveEnumerator);

        leftHandScript.Return();
        rightHandScript.Return();

        ChooseHand();
        chosenHand.Grab();
    }
    public void Phase11()
    {
        _Phase = 11;
        Debug.LogWarning("Phase 11");
        StartCoroutine(moveEnumerator);

        leftHandScript.Return();
        rightHandScript.Return();

        ChooseHand();
        chosenHand.Grab();
        StartCoroutine(SendPunchLoop(10, false, true, 4));
    }
    ////////Overall Phase 2////////
    void Phase10()
    {
        //a
    }
    ////////Overall Phase 3////////
    void Phase9()
    {
        Debug.LogWarning("Phase 9");
        StartCoroutine(moveEnumerator);

        speed = 1.2f;
        timeBetweenMove = 0.8f;
        //DoubleSaw
    }
    void Phase8()
    {
        Debug.LogWarning("Phase 8");
        StartCoroutine(moveEnumerator);

        StartCoroutine(DoublePunchLoop(5, true));
    }
    void Phase7()
    {
        Debug.LogWarning("Phase 7");
        StartCoroutine(moveEnumerator);

        chosenHand.BounceSaw(true);
        otherHand.Grab();
    }
    void Phase6()
    {
        Debug.LogWarning("Phase 6");
        //StartCoroutine(moveEnumerator);

        chosenHand.BounceSaw(true);
        otherHand.BounceSaw(false);
        crossSawAttack = true;
        crossSawAttackTime = 0;
    }
    void Phase5()
    {
        Debug.LogWarning("Phase 5");
        StartCoroutine(moveEnumerator);

        chosenHand.BounceSaw(true);
        ChooseHand();
        StartCoroutine(SendPunchLoop(7, false, false, 0));
    }
    ////////Overall Phase 4////////
    void Phase4()
    {
        Debug.LogWarning("Phase 4");

    }
    ////////Overall Phase 5////////
    void FinalPhase()
    {
        Debug.LogWarning("This is the End");
        StartCoroutine(moveEnumerator);

        speed = 2;
        timeBetweenMove = 0.5f;
        chosenHand.BounceSaw(true);
        otherHand.BounceSaw(false);
        StartCoroutine(TarotCards(2));
    }
}
