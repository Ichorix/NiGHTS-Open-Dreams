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
    private float flip = 180;
    private float oppositeFlip = 180;
    private bool flipped;
    private bool oppositeFlipped;

    void Update()
    {
        RotatePlayer();
    }
    void RotatePlayer()
    {
        // MoveDirection.x when positive gives the correct values. When negative it gives some funky results, so I flip it later depending on if you are moving backwards or not.
        // MoveDirection.y works fine so it doesnt need any special treatment
        Vector3 lookDirection = new Vector3(Mathf.Abs(_stats.MoveDirection.x), _stats.MoveDirection.y, 0).normalized;
        transform.forward += lookDirection * 5; // Whatever dark magic I used, this fixes it from being 45 to -45 to now be closer to 90 to -90
        
        
        if(_stats.MoveDirection.x > 0) // Checks if moving forwards
            isBackwards = false;
        else if(_stats.MoveDirection.x < 0) // Checks if moving backwards
            isBackwards = true;
        //Keeps value when not moving so that the player will look either left or right accordingly, rather than snapping to forward-facing like it used to


        if(isBackwards)
        // Rotates the player properly. Negative Y flips left vs right, and z+flip (lerp between 0 and 180) flips over his head. Lerped for more dynamic effect in the Enumerator below
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z + flip);
        else if(flipped)
        // Same deal as before except now without the -Y since you are looking forward, and a new flip variable to keep them separate
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + oppositeFlip);


        // Returns true when the player is upside down. Used to determine whether to lerp for flip
        if(transform.eulerAngles.z == 180)
        {
            if(_stats.isMoving)
                upsideDownTime += Time.deltaTime;
            else upsideDownTime = flipTimeThreshold;
        }
        else upsideDownTime = 0;

        //
        if(upsideDownTime >= flipTimeThreshold)
        {
            if(!flipped)
                StartCoroutine(FlipPlayer());
            else if(!oppositeFlipped)
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
        oppositeFlipped = false;
        upsideDownTime = 0;
        while(t < 1)
        {
            t += Time.deltaTime * _stats.flipSpeed;
            flip = Mathf.SmoothStep(180, 0, t);
            oppositeFlip = Mathf.SmoothStep(360, 180, t);
            yield return null;
        }
    }
    IEnumerator OppositeFlipPlayer()
    {
        float t = 0;
        oppositeFlipped = true;
        upsideDownTime = 0;
        while(t < 1)
        {
            t += Time.deltaTime * _stats.flipSpeed;
            flip = Mathf.SmoothStep(0, 180, t);
            oppositeFlip = Mathf.SmoothStep(180, 360, t);
            yield return null;
        }
        //Delayed to let the lerp happen. If not it would snap back
        flipped = false;
    }
}
