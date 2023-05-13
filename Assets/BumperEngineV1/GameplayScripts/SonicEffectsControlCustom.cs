using UnityEngine;
using System.Collections;

public class SonicEffectsControlCustom : MonoBehaviour {

    public LevelSonic Player;
    public ParticleSystem RunningDust;
	public ParticleSystem SpeedLines;
    public ParticleSystem SpinDashDust;
    public float RunningDustThreshold;
	public float SpeedLinesThreshold;

	void FixedUpdate () {
	
		if(Player.playerRb.velocity.sqrMagnitude > RunningDustThreshold && Player.canJump && RunningDust != null)
        {
            RunningDust.Emit(Random.Range(0,20));
        }

		if (Player.playerRb.velocity.sqrMagnitude > SpeedLinesThreshold && Player.canJump && SpeedLines != null && SpeedLines.isPlaying == false) 
		{
			SpeedLines.Play ();
		} 
		else if (Player.playerRb.velocity.sqrMagnitude < SpeedLinesThreshold && SpeedLines.isPlaying == true || (!Player.canJump)) 
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
