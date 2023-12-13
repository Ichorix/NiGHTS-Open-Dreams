using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTiler : MonoBehaviour
{
    public Renderer _renderSelph;
    public GameObject _objSelph;
    void Start()
    {
        _renderSelph = _objSelph.GetComponent<Renderer>();

        _renderSelph.material.SetTextureScale("_MainTex", new Vector2(2,2));
    }
    void Update()
    {
        _renderSelph.material.SetTextureOffset("_MainTex", new Vector2(1.5f,1.5f));
    }


}
