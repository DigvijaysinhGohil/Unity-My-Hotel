using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
	private enum States { FREE_ROAMING, BEING_CUSTOMER}

	private Vector3 targetPosition;
	private NavMeshAgent agent;
	private CharacterAnimationController animationController;
	private CharacterAudioController audioController;

	private bool idle = true;
	private States currentState = States.FREE_ROAMING;

	[SerializeField] private RandomPositionGenerator positionGenerator;

	public UnityAction OnTargetReached;

	private void Awake() {
		agent = GetComponent<NavMeshAgent>();
		animationController = GetComponent<CharacterAnimationController>();
		audioController = GetComponent<CharacterAudioController>();
		OnTargetReached += OnTargetReachedHandler;
	}

	private void OnDestroy() {
		OnTargetReached -= OnTargetReachedHandler;
	}

	private void Update() {
		CheckTargetReached();
		UpdateWalkAnimation();
		if(currentState == States.FREE_ROAMING) {
			if(idle) {
				WalkAround();
			}
		}
	}

	private void CheckTargetReached() {
		const float DISTANCE_THRESHOLD = 0.21f;
		if(Vector3.Distance(agent.destination, transform.position) < DISTANCE_THRESHOLD) {
			OnTargetReached?.Invoke();
		}
	}

	private void OnTargetReachedHandler() {
		if(currentState == States.FREE_ROAMING) {
			idle = true;
		}
	}
	private void WalkAround() {
		agent.SetDestination(positionGenerator.GetPosition());
		idle = false;
	}

	private void UpdateWalkAnimation() {
		const float SOUND_THRESHOLD = 0.2f;
		float speed = agent.velocity.sqrMagnitude;
		animationController.SetSpeed(speed);
		audioController.FootstepSfx(speed > SOUND_THRESHOLD);
	}

	public void MoveTo(Vector3 destination) {
		currentState = States.BEING_CUSTOMER;
		agent.SetDestination(destination);
	}

	public void SetStateFreeRoam() {
		currentState = States.FREE_ROAMING;
		OnTargetReached += OnTargetReachedHandler;
	}
}