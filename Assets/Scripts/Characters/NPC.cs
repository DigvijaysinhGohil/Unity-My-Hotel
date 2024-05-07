using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
	private Vector3 targetPosition;
	private NavMeshAgent agent;
	private CharacterAnimationController animationController;
	private CharacterAudioController audioController;

	[SerializeField] private RandomPositionGenerator positionGenerator;
	public bool idle = true;

	private void Awake() {
		agent = GetComponent<NavMeshAgent>();
		animationController = GetComponent<CharacterAnimationController>();
		audioController = GetComponent<CharacterAudioController>();
	}

	private void Update() {
		if (!idle) {
			WalkAround();
		}
		else {
			GetNewDestination();
		}
		UpdateWalkAnimation();
	}

	private void WalkAround() {
		agent.SetDestination(targetPosition);
		const float DISTANCE_THRESHOLD = 0.1f;
		if(Vector3.Distance(targetPosition, transform.position) < DISTANCE_THRESHOLD) {
			idle = true;
		}
	}

	private void GetNewDestination() {
		targetPosition = positionGenerator.GetPosition();
		idle = false;
	}

	private void UpdateWalkAnimation() {
		animationController.SetSpeed(agent.speed);
		const float SOUND_THRESHOLD = 0.2f;
		audioController.FootstepSfx(agent.speed > SOUND_THRESHOLD);
	}
}