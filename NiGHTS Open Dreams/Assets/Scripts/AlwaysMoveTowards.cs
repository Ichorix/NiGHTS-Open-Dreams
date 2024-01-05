using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysMoveTowards : MonoBehaviour
{
    [SerializeField] private Transform follow;
    [SerializeField] private float speed;
    enum UpdateType
    { Update, LateUpdate, FixedUpdate }
    [SerializeField] private UpdateType updateType;

    void OnEnable()
    {
        transform.position = follow.position;
    }
    void Update()
    {
        if (updateType == UpdateType.Update)
        {
            float step = speed * Time.deltaTime * Vector3.Distance(transform.position, follow.position);
            transform.position = Vector3.MoveTowards(transform.position, follow.position, step);
        }
    }
    void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
        {
            float step = speed * Vector3.Distance(transform.position, follow.position);
            transform.position = Vector3.MoveTowards(transform.position, follow.position, step);
        }
    }
    void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
        {
            float step = speed * Time.deltaTime * Vector3.Distance(transform.position, follow.position);
            transform.position = Vector3.MoveTowards(transform.position, follow.position, step);
        }
    }
}
