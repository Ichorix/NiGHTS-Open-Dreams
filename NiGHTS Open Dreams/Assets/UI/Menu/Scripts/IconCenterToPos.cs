using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconCenterToPos : MonoBehaviour
{
    public Vector3 goToPos = new Vector3(283.4466f, 403.2706f, 0);
    public Vector3 goToScale = Vector3.one;


    void Start()
    {
        goToPos = transform.position;
        goToScale = transform.localScale;
    }
}
