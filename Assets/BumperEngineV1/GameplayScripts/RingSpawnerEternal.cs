using UnityEngine;
using System.Collections;

public class RingSpawnerEternal : MonoBehaviour {

    public float Distance;
    Transform Player;

    public float RespawnTime;
    float counter;

    public GameObject Ring;
	private GameObject RingClone = null;
    public bool HasSpawned { get; set; }
    public GameObject TeleportSparkle;

    void Start()
    {
        HasSpawned = false;
        Player = GameObject.FindWithTag("Player").transform;
        GetComponent<MeshRenderer>().enabled = false;
        counter = RespawnTime;
    }

    void LateUpdate()
    {
        if (Vector3.Distance(Player.position, transform.position) < Distance)
        {
			if (!HasSpawned && RingClone == null) 
			{
				HasSpawned = true;
				//Debug.Log ("ShouldSpawn");
				StartCoroutine(SpawnInNormal());
			}
        }
    }

	private IEnumerator SpawnInNormal()
    {
		//Debug.Log ("SpawnRing");
		yield return new WaitForSeconds (RespawnTime);
		HasSpawned = false;
        Instantiate(TeleportSparkle, transform.position, transform.rotation);
		RingClone = (GameObject)Instantiate(Ring, transform.position, transform.rotation);
		GameObject.FindObjectOfType<LightDashControl>().UpdateHomingTargets();
    }
		
}
