using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Objects_Interaction : MonoBehaviour {

    [Header("For Rings, Springs and so on")]

    public PlayerBhysics Player;
    public HedgeCamera Cam;
    public SonicSoundsControl Sounds;
    public ActionManager Actions;
    public PlayerBinput Inp;
    public SonicSoundsControl sounds;
    Spring_Proprieties spring;
    int springAmm;


    public GameObject RingCollectParticle;
    public Material SpeedPadTrack;
    public Material DashRingMaterial;

    [Header("Enemies")]

    public float BouncingPower;
	public float HomingBouncingPower;
	public float EnemyHomingStoppingPowerWhenAdditive;
    public bool StopOnHomingAttackHit;
    public bool StopOnHit;
    public bool updateTargets { get; set; }

    public float EnemyDamageShakeAmmount;
    public float EnemyHitShakeAmmount;

    [Header("UI objects")]

    public Text RingsCounter;
    public Text SpeedCounter;

    public static int RingAmount { get; set; }

    MovingPlatformControl Platform;
    Vector3 TranslateOnPlatform;
    public Color DashRingLightsColor;
	private bool CanHitAgain = true;

    void Update()
    {
        if(SpeedCounter != null) SpeedCounter.text = Player.SpeedMagnitude.ToString("F0");
        //RingsCounter.text = ": " + RingAmount;
        if (updateTargets)
        {
            HomingAttackControl.UpdateHomingTargets();
			if (Actions.Action02 != null) {
				if (Actions.Action02 != null) {
			Actions.Action02.HomingAvailable = true;
		}
			}
            updateTargets = false;
        }

        //Set speed pad trackpad's offset
        SpeedPadTrack.SetTextureOffset("_MainTex", new Vector2(0, -Time.time) * 3);
        DashRingMaterial.SetColor("_EmissionColor", (Mathf.Sin(Time.time * 15) * 1.3f) * DashRingLightsColor);
    }

	private IEnumerator ResetTriggerBool()
	{
		CanHitAgain = false;
		yield return new WaitForSeconds (0.05f);
		CanHitAgain = true;
	}

    void FixedUpdate()
    {
        if(Platform != null)
        {
            transform.position += (-Platform.TranslateVector);
        }
        if (!Player.Grounded)
        {
            Platform = null;
        }
    }

	public void OnTriggerEnter(Collider col)
    {
        //Speed Pads Collision
        if(col.tag == "SpeedPad")
        {
			Actions.Action01.JumpBall.SetActive(false);
			if (Actions.Action08 != null) {
				if (Actions.Action08.DropEffect.isPlaying == true) {
					Actions.Action08.DropEffect.Stop ();
				}
			}
            if(col.GetComponent<SpeedPadData>() != null)
            {
                transform.rotation = Quaternion.identity;
                //ResetPlayerRotation

                if (col.GetComponent<SpeedPadData>().LockToDirection)
                {
                    Player.p_rigidbody.velocity = Vector3.zero;
                    Player.AddVelocity(col.transform.forward * col.GetComponent<SpeedPadData>().Speed);
                }
                else
                {
                    Player.AddVelocity(col.transform.forward * col.GetComponent<SpeedPadData>().Speed);
                }
                if (col.GetComponent<SpeedPadData>().Snap)
                {
                    transform.position = col.transform.position;
                }
                if (col.GetComponent<SpeedPadData>().isDashRing)
                {
                    Actions.ChangeAction(0);
                    Actions.Action00.CharacterAnimator.SetBool("Grounded", false);
                    Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
                }

                if (col.GetComponent<SpeedPadData>().LockControl)
                {
                    Inp.LockInputForAWhile(col.GetComponent<SpeedPadData>().LockControlTime, true);
                }
                if (col.GetComponent<SpeedPadData>().AffectCamera)
                {
                    Vector3 dir = col.transform.forward;
                    Cam.SetCamera(dir, 2.5f, 20, 5f, 1);
                    col.GetComponent<AudioSource>().Play();
                }

            }
        }

        //Rings Collision
        if (col.tag == "Ring")
        {
			Instantiate(RingCollectParticle, col.transform.position, Quaternion.identity);
			Destroy(col.gameObject);
			StartCoroutine(IncreaseRing ());
            
            
        }
        if (col.tag == "MovingRing")
        {
            if (col.GetComponent<MovingRing>() != null)
            {
                if (col.GetComponent<MovingRing>().colectable)
                {
					StartCoroutine(IncreaseRing ());
                    Instantiate(RingCollectParticle, col.transform.position, Quaternion.identity);
                    Destroy(col.gameObject);
                }
            }
        }

		//Switch
		if(col.tag == "Switch")
		{	
			if (col.GetComponent<Switch_Properties> () != null) {
				col.GetComponent<Switch_Properties> ().Activate ();
			}
		}

        //Hazard
        if(col.tag == "Hazard")
        {
			Actions.Action01.JumpBall.SetActive(false);
			if (Actions.Action08 != null) {
				if (Actions.Action08.DropEffect.isPlaying == true) {
					Actions.Action08.DropEffect.Stop ();
				}
			}
            DamagePlayer();
            HedgeCamera.Shakeforce = EnemyDamageShakeAmmount;
        }

        //Enemies
        if (col.tag == "Enemy")
        {
            HedgeCamera.Shakeforce = EnemyHitShakeAmmount;
            //If 1, destroy, if not, take damage.
            if (Actions.Action == 3)
            {
                col.transform.parent.GetComponent<EnemyHealth>().DealDamage(1);
                updateTargets = true;
            }
            if (Actions.Action00.CharacterAnimator.GetInteger("Action") == 1)
            {
				//Actions.Action01.JumpBall.enabled = false;
                if (col.transform.parent.GetComponent<EnemyHealth>() != null)
                {
                    if (!Player.isRolling)
                    {

                        Vector3 newSpeed = new Vector3(1, 0, 1);

						if ((Actions.Action == 1 || Actions.Action == 0) && CanHitAgain)
						{
							StartCoroutine (ResetTriggerBool ());

							////Debug.Log ("AfterJumping");
							newSpeed = new Vector3(Player.p_rigidbody.velocity.x, 0, Player.p_rigidbody.velocity.z);
							newSpeed.y = BouncingPower + Mathf.Abs (Player.p_rigidbody.velocity.y);
							////Debug.Log (newSpeed);
							Player.p_rigidbody.velocity = newSpeed;
						}

						else if ((Actions.Action == 2 || Actions.PreviousAction == 2) && !StopOnHit && CanHitAgain)
                        {
							StartCoroutine (ResetTriggerBool ());

							//Debug.Log ("AfterHoming");
							//Vector3 Direction = col.transform.position - Player.transform.position;
							newSpeed = new Vector3(Player.p_rigidbody.velocity.x*(1/EnemyHomingStoppingPowerWhenAdditive), HomingBouncingPower, Player.p_rigidbody.velocity.z*(1/EnemyHomingStoppingPowerWhenAdditive));
							////Debug.Log (newSpeed);
							Player.p_rigidbody.velocity = newSpeed;
                        }
						else if(StopOnHit)
						{
							//Debug.Log ("AfterHomingStop");
							newSpeed = new Vector3(0, 0, 0);
							newSpeed = Vector3.Scale(Player.p_rigidbody.velocity, newSpeed);
							newSpeed.y = HomingBouncingPower;
							Player.p_rigidbody.velocity = newSpeed;
						}

						if (Actions.Action == 6 && CanHitAgain)
						{
							StartCoroutine (ResetTriggerBool ());
						//	//Debug.Log ("AfterBouncing");
							newSpeed = new Vector3(1, 0, 1);
							////Debug.Log ("AfterHoming");
							newSpeed = Vector3.Scale(Player.p_rigidbody.velocity, newSpeed);
							newSpeed.y = HomingBouncingPower*2;
							Player.p_rigidbody.velocity = newSpeed;

							Actions.Action01.JumpBall.SetActive(false);
							if (Actions.Action08 != null) {
								if (Actions.Action08.DropEffect.isPlaying == true) {
									Actions.Action08.DropEffect.Stop ();
								}
							}
						}
                        

							
                        
                    }
                    col.transform.parent.GetComponent<EnemyHealth>().DealDamage(1);
                    updateTargets = true;
					Actions.Action01.JumpBall.SetActive(false);
					if (Actions.Action08 != null) {
						if (Actions.Action08.DropEffect.isPlaying == true) {
							Actions.Action08.DropEffect.Stop ();
						}
					}
                    Actions.ChangeAction(0);
                    
                }
            }


            else if(Actions.Action != 3)
            {
                DamagePlayer();
            }
        }

		//Monitors
		if (col.tag == "Monitor")
		{
			if (Actions.Action00.CharacterAnimator.GetInteger("Action") == 1)
			{
					if (!Player.isRolling)
					{

						Vector3 newSpeed = new Vector3(1, 0, 1);

						if ((Actions.Action == 1 || Actions.Action == 0) && CanHitAgain)
						{
							StartCoroutine (ResetTriggerBool ());

							////Debug.Log ("AfterJumping");
							newSpeed = new Vector3(Player.p_rigidbody.velocity.x, 0, Player.p_rigidbody.velocity.z);
							newSpeed.y = BouncingPower + Mathf.Abs (Player.p_rigidbody.velocity.y);
							////Debug.Log (newSpeed);
							Player.p_rigidbody.velocity = newSpeed;
						}

						else if ((Actions.Action == 2 || Actions.PreviousAction == 2) && !StopOnHit && CanHitAgain)
						{
							StartCoroutine (ResetTriggerBool ());

							//Debug.Log ("AfterHoming");
							//Vector3 Direction = col.transform.position - Player.transform.position;
							newSpeed = new Vector3(Player.p_rigidbody.velocity.x*(1/EnemyHomingStoppingPowerWhenAdditive), HomingBouncingPower, Player.p_rigidbody.velocity.z*(1/EnemyHomingStoppingPowerWhenAdditive));
							////Debug.Log (newSpeed);
							Player.p_rigidbody.velocity = newSpeed;
						}
						else if(StopOnHit)
						{
							//Debug.Log ("AfterHomingStop");
							newSpeed = new Vector3(0, 0, 0);
							newSpeed = Vector3.Scale(Player.p_rigidbody.velocity, newSpeed);
							newSpeed.y = HomingBouncingPower;
							Player.p_rigidbody.velocity = newSpeed;
						}

						if (Actions.Action == 6 && CanHitAgain)
						{
							StartCoroutine (ResetTriggerBool ());
							//	//Debug.Log ("AfterBouncing");
							newSpeed = new Vector3(1, 0, 1);
							////Debug.Log ("AfterHoming");
							newSpeed = Vector3.Scale(Player.p_rigidbody.velocity, newSpeed);
							newSpeed.y = HomingBouncingPower*2;
							Player.p_rigidbody.velocity = newSpeed;

							Actions.Action01.JumpBall.SetActive(false);
							if (Actions.Action08 != null) {
								if (Actions.Action08.DropEffect.isPlaying == true) {
									Actions.Action08.DropEffect.Stop ();
								}
							}
						}




					}
					updateTargets = true;
					Actions.Action01.JumpBall.SetActive(false);
					if (Actions.Action08 != null) {
						if (Actions.Action08.DropEffect.isPlaying == true) {
							Actions.Action08.DropEffect.Stop ();
						}
					}
					Actions.ChangeAction(0);
			}
			
		}


        //Spring Collision

        if (col.tag == "Spring")
        {
			Actions.Action01.JumpBall.SetActive(false);
			if (Actions.Action08 != null) {
				if (Actions.Action08.DropEffect.isPlaying == true) {
					Actions.Action08.DropEffect.Stop ();
				}
			}


            if (col.GetComponent<Spring_Proprieties>() != null)
            {
                spring = col.GetComponent<Spring_Proprieties>();
                if (spring.IsAdditive)
                {
                    transform.position = col.transform.GetChild(0).position;
                    if (col.GetComponent<AudioSource>()) { col.GetComponent<AudioSource>().Play(); }
                    Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
                    if (Actions.Action02 != null) {
			Actions.Action02.HomingAvailable = true;
		}
                    Player.p_rigidbody.velocity += (spring.transform.up * spring.SpringForce);
                    Actions.ChangeAction(0);
                    spring.anim.SetTrigger("Hit");
                }
                else
                {
                    transform.position = col.transform.GetChild(0).position;
                    if (col.GetComponent<AudioSource>()) { col.GetComponent<AudioSource>().Play(); }
                    Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
                    if (Actions.Action02 != null) {
						Actions.Action02.HomingAvailable = true;
					}
                    Player.p_rigidbody.velocity = spring.transform.up * spring.SpringForce;
                    Actions.ChangeAction(0);
                    spring.anim.SetTrigger("Hit");
                }

                if (col.GetComponent<Spring_Proprieties>().LockControl)
                {
                    Inp.LockInputForAWhile(col.GetComponent<Spring_Proprieties>().LockTime, false);
                }
            }
        }

		if (col.tag == "Bumper")
		{
			Actions.Action01.JumpBall.SetActive(false);
			if (Actions.Action08 != null) {
				if (Actions.Action08.DropEffect.isPlaying == true) {
					Actions.Action08.DropEffect.Stop ();
				}
			}


			if (col.GetComponent<Spring_Proprieties>() != null)
			{
				spring = col.GetComponent<Spring_Proprieties>();
				if (spring.IsAdditive)
				{
				//	transform.position = col.transform.GetChild(0).position;
					if (col.GetComponent<AudioSource>()) { col.GetComponent<AudioSource>().Play(); }
					Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
					if (Actions.Action02 != null) {
						Actions.Action02.HomingAvailable = true;
					}
					Player.p_rigidbody.velocity += (Player.transform.position-spring.transform.position) * spring.SpringForce;

				}
				else
				{
					//transform.position = col.transform.GetChild(0).position;
					if (col.GetComponent<AudioSource>()) { col.GetComponent<AudioSource>().Play(); }
					Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
					if (Actions.Action02 != null) {
						Actions.Action02.HomingAvailable = true;
					}
					Player.p_rigidbody.velocity = (Player.transform.position-spring.transform.position) * spring.SpringForce;
				
				}

				if (col.GetComponent<Spring_Proprieties>().LockControl)
				{
					Inp.LockInputForAWhile(col.GetComponent<Spring_Proprieties>().LockTime, false);
				}
			}
		}

		//Monitors
		if (col.tag == "CancelHoming") 
		{
			if (Actions.Action == 2) {

				Vector3 newSpeed = new Vector3(1, 0, 1);

				Actions.ChangeAction (0);
				newSpeed = new Vector3(0, HomingBouncingPower, 0);
				////Debug.Log (newSpeed);
				Player.p_rigidbody.velocity = newSpeed;
				//Player.transform.position = col.ClosestPoint (Player.transform.position);
				if (Actions.Action02 != null) {
					Actions.Action02.HomingAvailable = true;
				}
			}
		}


    }

    public void OnTriggerStay(Collider col)
    {
        //Hazard
        if (col.tag == "Hazard")
        {
            DamagePlayer();
        }

        if (col.gameObject.tag == "MovingPlatform")
        {
            Platform = col.gameObject.GetComponent<MovingPlatformControl>();
        }
        else
        {
            Platform = null;
        }
    }

	private IEnumerator IncreaseRing()
	{
		int ThisFramesRingCount = RingAmount;
		RingAmount++;
		yield return new WaitForEndOfFrame ();
		if (RingAmount > ThisFramesRingCount + 1) 
		{
			RingAmount--;
		}
			
	}

    public void DamagePlayer()
    {
        if (!Actions.Action04Control.IsHurt && Actions.Action != 4)
        {

            if (!Monitors_Interactions.HasShield)
            {
                if (RingAmount > 0)
                {
                    //LoseRings
                    Sounds.RingLossSound();
                    Actions.Action04Control.GetHurt();
                    Actions.ChangeAction(4);
                    Actions.Action04.InitialEvents();
                }
                if (RingAmount <= 0)
                {
                    //Die
                    if (!Actions.Action04Control.isDead)
                    {
                        Sounds.DieSound();
                        Actions.Action04Control.isDead = true;
                        Actions.ChangeAction(4);
                        Actions.Action04.InitialEvents();
                    }
                }
            }
            if (Monitors_Interactions.HasShield)
            {
                //Lose Shield
                Actions.Action04.sounds.SpikedSound();
                Monitors_Interactions.HasShield = false;
                Actions.ChangeAction(4);
                Actions.Action04.InitialEvents();
            }
        }
    }
}
