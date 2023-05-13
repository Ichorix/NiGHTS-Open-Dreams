using UnityEngine;
using System.Collections;

public class SonicEffectsControl : MonoBehaviour {

    public PlayerBhysics Player;
	public LinkBoost linker;
    public ParticleSystem RunningDust;
	public ParticleSystem SpeedLines;
    public ParticleSystem SpinDashDust;
    public float RunningDustThreshold;
	public float SpeedLinesThreshold;

	void FixedUpdate () {
	
		if(Player.p_rigidbody.velocity.sqrMagnitude > RunningDustThreshold && Player.Grounded && RunningDust != null)
        {
            RunningDust.Emit(Random.Range(0,20));
        }

		if (linker.linkActive == true && Player.Grounded && SpeedLines != null && SpeedLines.isPlaying == false) 
		{
			SpeedLines.Play ();
		} 
		else if (linker.linkActive == false && SpeedLines.isPlaying == true || (!Player.Grounded)) 
		{
			SpeedLines.Stop ();
		}

	}
    public void DoSpindashDust(int amm, float speed)
    {
        SpinDashDust.startSpeed = speed;
        SpinDashDust.Emit(amm);
    }
}
