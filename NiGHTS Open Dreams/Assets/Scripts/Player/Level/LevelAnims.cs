using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace PathCreation.Examples
{
    public class LevelAnims : MonoBehaviour
    {
        public LevelFollow levelFollow;
        public Animator mAnimator;
        public ParticleSystem LeftHandSparkles, RightHandSparkles, BoostTrail;
        public TrailRenderer trail;
        public bool rotateIn, rotateOut, isBoosting;
        public float BoostGauge, BoostNum;
        
        void Start()
        {
            mAnimator = GetComponent<Animator>();

            
            //Particles
            LeftHandSparkles.enableEmission = false;
            RightHandSparkles.enableEmission = false;
            BoostTrail.enableEmission = false;
            trail.emitting = false;
        }

        public float vDir, hDir, vRot, hRot;
        void Update()
        {
            vDir = levelFollow.movingVertical;
            hDir = levelFollow.movingHorizontal;
            vRot = levelFollow.rotatingVertical;
            hRot = levelFollow.rotatingHorizontal;
            if(vDir != 0 || hDir != 0)
            {
                LeftHandSparkles.enableEmission = true;
                RightHandSparkles.enableEmission = true;
            }
            else{
                LeftHandSparkles.enableEmission = false;
                RightHandSparkles.enableEmission = false;
            }
            if(levelFollow.isBoosting)
            {
                BoostTrail.enableEmission = true;
                trail.emitting = true;
                mAnimator.SetBool("isBoosting", true);
            }
            else{
                BoostTrail.enableEmission = false;
                trail.emitting = false;
                mAnimator.SetBool("isBoosting", false);
            }
            
            if(vRot <= -0.60)
            {
                mAnimator.SetBool("isUpsideDown", false);
                mAnimator.SetBool("isUp", true);
            }
            if(vRot >= 0.60)
            {
                mAnimator.SetBool("isUpsideDown", false);
                mAnimator.SetBool("isDown", true);
            }
            if(vRot <= 0.60 && vRot >= -0.60)
            {
                mAnimator.SetBool("isUpsideDown", false);
                mAnimator.SetBool("isUp", false);
                mAnimator.SetBool("isDown", false);
            }
            if(hDir >= 0.1f)
            {
                mAnimator.SetBool("isUpsideDown", false);
                mAnimator.SetBool("isMoving", true);
            }
            if(hDir <= -0.1f)
            {
                mAnimator.SetBool("isUpsideDown", true);
            }
            if(hDir == 0)
            {
                mAnimator.SetBool("isMoving", false);
                mAnimator.SetBool("isUpsideDown", false);
            }

            if(hDir == 0 && vRot <= 0.60 && vRot >= -0.60)
            {
                //mAnimator.SetTrigger("TrHover");
                //Debug.Log("i didnt do anything here cuz i dont think i need to");
            }
        }
    }
}
