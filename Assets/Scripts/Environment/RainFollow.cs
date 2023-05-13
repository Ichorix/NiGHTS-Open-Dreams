using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollow : MonoBehaviour
{
    public StateController sc;
    [SerializeField]
    private Vector3 follow;
    [SerializeField]
    private float yVal;

    void Update()
    {
        follow = new Vector3(sc.currentPlayer.transform.position.x, yVal, sc.currentPlayer.transform.position.z);
        transform.position = follow;
    }
}
