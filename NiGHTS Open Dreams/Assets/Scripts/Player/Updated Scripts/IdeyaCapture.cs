using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdeyaCapture : MonoBehaviour
{
    [SerializeField] private NPlayerScriptableObject _stats;
    public bool ideyaReleased;
    public Transform ideyaHomePos;
    public IdeyaChase ideya;

    void Start()
    {
        ideyaReleased = false;
        ideya.inPlace = true;
        ideya.goToPosition = ideyaHomePos;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _stats.isLevelPlayer && !ideyaReleased)
        {
            NPlayerLevelFollow levelFollow = other.gameObject.GetComponent<NPlayerLevelFollow>();
            if(levelFollow.currentChips >= levelFollow.chipRequirement)
            {
                levelFollow.ContinueLevel = true;
                levelFollow.recoveredIdeya = ideya;
                ReleaseIdeya(other);
                levelFollow.blueChipMaterial.SetFloat("_EmissionOn", 1f);
                levelFollow.blueChipData.Score = 10;
            }
        }
    }

    public void ReleaseIdeya(Collider other)
    {
        ideyaReleased = true;
        ideya.inPlace = false;
        ideya.goToPosition = other.gameObject.transform;
    }
}
