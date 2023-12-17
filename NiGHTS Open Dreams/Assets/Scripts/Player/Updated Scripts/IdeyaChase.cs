using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeyaChase : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Transform goToPosition;
    public bool inPlace;
    public float chaseSpeed;
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private float maxDrag;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(inPlace)
        {
            float step = chaseSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, goToPosition.position, step);
        }
    }
    void FixedUpdate()
    {
        if(!inPlace)
        {
            transform.LookAt(goToPosition);
            rigidbody.AddForce(transform.forward * chaseSpeed, forceMode);
            rigidbody.drag = CalculateDrag();
        }
    }

    float CalculateDrag()
    {
        float distance = Vector3.Distance(transform.position, goToPosition.position);
        return Mathf.Clamp((distance * distance) - 5, 0, maxDrag);
    }
}
