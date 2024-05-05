using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private CharacterAnimationController animationController;
	private Rigidbody rigidBody;
	private Vector3 inputVector;

	[SerializeField] private float speed = 10;
	[Range(0.01f, 1f), SerializeField] private float rotationDamp = .1f;

	private void Awake() {
		rigidBody = GetComponent<Rigidbody>();
		animationController = GetComponent<CharacterAnimationController>();
	}

	private void Update() {
		ReadInput();
		RotatePlayer();
		UpdateAnimation();
	}

	private void FixedUpdate() {
		Move();
	}

	private void ReadInput() {
		inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
	}

	private void Move() {
		rigidBody.AddForce(inputVector * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}

	private void RotatePlayer() {
		const float ROTATE_THRESHOLD = 0.2f;
		Quaternion targetRotation = transform.rotation;
		if(inputVector.sqrMagnitude > ROTATE_THRESHOLD) {
			targetRotation = Quaternion.LookRotation(inputVector, Vector3.up);
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationDamp);
	}

	private void UpdateAnimation() {
		if(animationController == null)
			return;

		animationController.SetSpeed(inputVector.sqrMagnitude);
	}
}