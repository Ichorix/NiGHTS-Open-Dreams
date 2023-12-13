using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowProjectile : MonoBehaviour
{
    [SerializeField] private float force;
    private float time;
    [SerializeField] private float killTime;
    void Start()
    {
        Launch();
    }
    void OnEnable()
    {
        Launch();
    }

    void Update()
    {
        if (time < killTime) time += Time.deltaTime;
        else Destroy(gameObject);

    }

    private void Launch()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
    }
}
