using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public Vector3 startingPos, currentPos, gotoPos;
    public GameObject targetObject, jackleLocal, self, mesh, portalParticle;
    public SkinnedMeshRenderer meshRenderer;
    public float _time, _speed, teleportTime;
    public AnimationCurve curve;
    public bool isChasing, grabbing, stopGrab, moveBack, moveForward, atDestination;
    public int mostRecentFunction; //0 None, 1 Grab, 2 Return,
    void Start()
    {
        currentPos = transform.position;
        gotoPos = transform.position;
        _speed = 0.7f;
        mostRecentFunction = 0;

        targetObject = GameObject.Find("/TargetObject");
        jackleLocal = GameObject.Find("/Jackle/JackleLocal");
        self = this.gameObject;
        mesh = self.transform.GetChild(1).gameObject;
        meshRenderer = mesh.GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Grab();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Return();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Teleport();
        }

        if(grabbing)
        {
            if(_time <= 1) //Time and speed of lerp
            {
                _time += Time.deltaTime * _speed;
            }
            if(_time <= 0.5f && isChasing) //Chasing
            {
                gotoPos = targetObject.transform.position;
            }
            if(_time >= 0.5f)
            {
                isChasing = false;
            }
            
            transform.position = Vector3.Lerp(currentPos, gotoPos, curve.Evaluate(_time));
        }
        if(stopGrab)
        {
            gotoPos = jackleLocal.transform.position;
            if(_time >= 1)
            {
                grabbing = false;
            }
        }
        
        if(moveBack)
        {
            transform.Translate(Vector3.back * Time.deltaTime);
            teleportTime += Time.deltaTime;
            if(teleportTime >= 1)
            {
                meshRenderer.enabled = false;
                transform.position = gotoPos;
                Instantiate(portalParticle, mesh.transform);
                teleportTime = 0;
                moveBack = false;
                moveForward = true;
            }
        }
        if(moveForward)
        {
            meshRenderer.enabled = true;
            transform.Translate(Vector3.forward * Time.deltaTime);
            teleportTime += Time.deltaTime;
            if(teleportTime >= 1)
            {
                moveForward = false;
                if(mostRecentFunction == 2 || mostRecentFunction == 0)
                {
                    atDestination = true;
                }
                else
                {
                    atDestination = false;
                    stopGrab = true;
                }
            }
        }

        if(atDestination)
        {
            gotoPos = transform.position;
            currentPos = transform.position;
            transform.position = Vector3.Lerp(currentPos, gotoPos, curve.Evaluate(_time));
        }
    }

    public void Grab()
    {
        Debug.Log("Grab()");
        _time = 0;
        currentPos = transform.position;
        gotoPos = targetObject.transform.position;
        isChasing = true;
        grabbing = true;
        stopGrab = false;
        mostRecentFunction = 1;
    }
    public void Return()
    {
        Debug.Log("Return()");
        _time = 0;
        currentPos = transform.position;
        gotoPos = jackleLocal.transform.position;
        stopGrab = true;
        mostRecentFunction = 2;
    }
    
    public void Teleport()
    {
        Debug.Log("Hands Teleport()");
        Instantiate(portalParticle, mesh.transform);
        teleportTime = 0;
        moveBack = true;
        if(mostRecentFunction == 1) //Return
        {
            gotoPos = jackleLocal.transform.position;
            Debug.Log("TP Return");
        }
        if(mostRecentFunction == 2 || mostRecentFunction == 0) //Attack
        {
            gotoPos = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y +3, targetObject.transform.position.z);
            Debug.Log("TP Attack");
        }
    }

    public void DoubleSaw()
    {
        Debug.Log("Double Saw");
    }
    public void SingleSaw()
    {
        Debug.Log("Single Saw");
    }
}
