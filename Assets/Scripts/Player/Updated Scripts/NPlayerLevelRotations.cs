using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerLevelRotations : MonoBehaviour
{
    public NPlayerScriptableObject _stats;

    private Vector3 playerRotation;
    private bool isBackwards;
    public float upsideDownTime;
    public float flipTimeThreshold;
    public float flip = 180;
    public float oppositeFlip = 180;
    public bool flipped;

    void Update()
    {
        RotatePlayer();
    }
    void RotatePlayer()
    {
        /*
        playerRotation.x = transform.rotation.x; 
        playerRotation.y = pathRotation.y;          // Gets the Left/Right rotation of the track
        playerRotation.z = transform.rotation.z;

        transform.rotation = Quaternion.Euler(pathRotation);
        */

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
        {
            if(_stats.isMoving)
                upsideDownTime += Time.deltaTime;
            else upsideDownTime = flipTimeThreshold;
        }
        else upsideDownTime = 0;
        if(upsideDownTime >= flipTimeThreshold)
        {
            if(!flipped)
                StartCoroutine(FlipPlayer());
            else
                StartCoroutine(OppositeFlipPlayer());
        }
        
        transform.localEulerAngles = new Vector3(
            _stats.isMoving ? transform.localEulerAngles.x : 0,
            isBackwards ? 180 : 0,
            transform.localEulerAngles.z);
    }

    IEnumerator FlipPlayer()
    {
        float t = 0;
        flipped = true;
        upsideDownTime = 0;
        while(t < 1)
        {
            t += Time.deltaTime * _stats.recenterSpeed;
            flip = Mathf.SmoothStep(180, 0, t);
            oppositeFlip = Mathf.SmoothStep(360, 180, t);
            Debug.Log("Flipping");
            yield return null;
        }
    }
    IEnumerator OppositeFlipPlayer()
    {
        float t = 0;
        flipped = false;
        upsideDownTime = 0;
        while(t < 1)
        {
            t += Time.deltaTime * _stats.recenterSpeed;
            oppositeFlip = Mathf.SmoothStep(180, 360, t);
            flip = Mathf.SmoothStep(0, 180, t);
            Debug.Log("ReFlipping");
            yield return null;
        }
    }
}
