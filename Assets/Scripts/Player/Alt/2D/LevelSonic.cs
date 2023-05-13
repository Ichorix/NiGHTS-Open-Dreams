using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSonic : MonoBehaviour
{
    public Rigidbody playerRb;
    public GameObject jumpBall, boostAura, chargeUpParticles;
    public SonicAnimationScript animScript;
    public SonicSoundsControl sfxScript;
    public float speed, boostPower, boostTime;
    private float gravityMult;
    public bool hasBoosted, canBoost, canJump;
    public Vector3 directionalInput, playerLR;

    void Start()
    {
        canBoost = true;
        canJump = true;
        speed = 10;
        gravityMult = 4;
        boostPower = 60;
        playerRb.mass = 3;
        playerRb.drag = 1;
        Physics.gravity *= gravityMult;
        boostAura.SetActive(false);
        chargeUpParticles.SetActive(false);
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        //playerRb.AddForce(Vector3.right * horizontalInput * speed, ForceMode.Impulse);
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
    }
    void Update()
    {
        Debug.Log("Speed = " + playerRb.velocity);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        directionalInput = new Vector3(horizontalInput, verticalInput, 0);
        Debug.Log(directionalInput);
        boostAura.transform.forward = directionalInput;

        //playerLR = new Vector3(this.transform.rotation.x, horizontalInput, this.transform.rotation.z);
        //this.transform.forward = playerLR;

        if(canJump && Input.GetKeyDown(KeyCode.Space))
        {
            sfxScript.JumpSound();
            playerRb.AddForce(Vector3.up * 50, ForceMode.Impulse);
            canJump = false;
            animScript.mAnimator.SetInteger("Action", 1);
            animScript.mAnimator.SetBool("Grounded", false);
            jumpBall.SetActive(true);
        }
        if(canBoost && Input.GetKeyDown(KeyCode.LeftShift))
        {
            boostPower = 60f;
            animScript.mAnimator.SetBool("isCharging", true);
            jumpBall.SetActive(false);
            boostAura.SetActive(false);
            chargeUpParticles.SetActive(true);
        }
        if(canBoost && Input.GetKey(KeyCode.LeftShift))
        {
            boostPower += Time.deltaTime * 25;
            playerRb.drag = 20;
            speed = 2;
        }
        if(canBoost && Input.GetKeyUp(KeyCode.LeftShift))
        {
            Boost();
            animScript.mAnimator.SetBool("isCharging", false);
            playerRb.drag = 1;
        }
        if(boostPower >= 130)
        {
            canBoost = false;
            playerRb.drag = 20;
            speed = 10;
            boostPower = 60f;
            boostAura.SetActive(false);
            chargeUpParticles.SetActive(false);
        }

        if(hasBoosted == true && boostTime <= 0.7f)
        {
            boostTime += Time.deltaTime;
        }
        if(boostTime >= 0.7f)
        {
            boostTime = 0f;
            hasBoosted = false;
            animScript.mAnimator.SetInteger("Action", 0);
            boostAura.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        canBoost = true;
        canJump = true;
        animScript.mAnimator.SetBool("Grounded", true);
        animScript.mAnimator.SetInteger("Action", 0);
        jumpBall.SetActive(false);
        boostAura.SetActive(false);
        sfxScript.LandingSound();
    }
    void OnTriggerEnter()
    {
        canBoost = true;
    }

    void Boost()
    {
        sfxScript.BoostSound();
        playerRb.AddForce(Vector3.up + directionalInput.normalized * boostPower, ForceMode.Impulse);
        canBoost = false;
        hasBoosted = true;
        boostPower = 60f;
        speed = 10;
        boostTime = 0;
        animScript.mAnimator.SetInteger("Action", 7);
        jumpBall.SetActive(false);
        boostAura.SetActive(true);
        chargeUpParticles.SetActive(false);
    }
}
