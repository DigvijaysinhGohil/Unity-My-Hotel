using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
	[SerializeField] private AudioSource footsteps;

	public void FootstepSfx(bool play) {
		if(!play) {
			footsteps.Stop();
			return;
		}

		if(!footsteps.isPlaying ) {
			footsteps.Play();
		}
	}
}