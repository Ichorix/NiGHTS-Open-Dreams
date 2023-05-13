using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceAligning : MonoBehaviour
{


    void Update()
    {
        SurfaceAlignment();
        //BuylAlignment();
        //GetAlignment();
    }
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private AnimationCurve animCurve;
    public float gTime;
    public void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        //Quaternion RotationRef = Quaternion.Euler(0, 0, 0);
        //Debug.Log(info);
        vAlignToGround = info.normal;
        if (Physics.Raycast(ray, out info, WhatIsGround))
        {
            //transform.rotation =
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), animCurve.Evaluate(gTime));
            //surfaceAlignmentRot =
        }
    } 
    public void GetAlignment()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, -transform.up, out hit, 0.7f, WhatIsGround);

        Vector3 newUp = hit.normal;

        transform.up = newUp;
    }

    public bool AlignToGround;
    public Vector3 movement;
    public Vector3 vAlignToGround;
    public void BuylAlignment()
    {
        Quaternion targetRotation;
        if(AlignToGround)
        {
            Quaternion qAlign = Quaternion.FromToRotation(Vector3.up, vAlignToGround);
            targetRotation = qAlign * Quaternion.LookRotation(movement);

        }
        else
            targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, gTime);
        transform.rotation = newRotation;

        
    }

}
