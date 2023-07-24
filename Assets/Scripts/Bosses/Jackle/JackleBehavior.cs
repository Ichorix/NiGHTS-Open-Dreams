using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackleBehavior : MonoBehaviour
{
    public int hands;
    public GameObject leftHand, rightHand, mesh;
    public SkinnedMeshRenderer meshRenderer;
    public ParticleSystem doubleSawParticle;
    public bool sawing;
    void Start()
    {
        hands = 2;
        doubleSawParticle.enableEmission = false;
        mesh = this.gameObject.transform.GetChild(1).gameObject;
        meshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
    }
    void Update()
    {
        if(sawing)
        {
            leftHand.transform.rotation = new Quaternion(leftHand.transform.rotation.x, leftHand.transform.rotation.y, leftHand.transform.rotation.z + Time.deltaTime, 1);
        }
    }

    public void Saw()
    {
        Debug.Log("Saw");
        if(hands >= 2)
        {
            hands = 2;
            DoubleSaw();
        }
        if(hands == 1)
        {
            //SingleSaw();
        }
        if(hands <= 0)
        {
            Debug.Log("No Hands");
        }
    }

    public void DoubleSaw()
    {
        doubleSawParticle.enableEmission = true;
        sawing = true;
    }


    

}
