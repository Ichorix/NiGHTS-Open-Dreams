using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeyaFollow : MonoBehaviour
{
    public Vector3 initialPos;
    public AnimationCurve curveX, curveY, curveZ;
    public GameObject player;
    public Vector3 pos;
    public float t, maxT;
    public float speed;
    public float offset;

    void Update()
    {
        pos = new Vector3(
            Mathf.Lerp(player.transform.position.x - offset, player.transform.position.x + offset, curveX.Evaluate(t)),
            Mathf.Lerp(player.transform.position.y - offset, player.transform.position.y + offset, curveY.Evaluate(t)),
            Mathf.Lerp(player.transform.position.z - offset, player.transform.position.z + offset, curveZ.Evaluate(t)));

        t += Time.deltaTime * speed;
        transform.position = pos;

        if(t >= maxT) t = 0;
    }

    void OnEnable()
    {
        initialPos = transform.position;
    }
    
    void OnDisable()
    {
        transform.position = initialPos;
        this.gameObject.GetComponent<IdeyaFollow>().enabled = false;
    }
}
