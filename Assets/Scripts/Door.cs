using UnityEngine;

public class Door : MonoBehaviour {
	public bool interactableByPlayer = true;
	[SerializeField] private float animationTime = 1f;
	[SerializeField] private float openAngle = -110f;
	[SerializeField] private float closeAngle = 0f;
	[SerializeField] private LeanTweenType easeType = LeanTweenType.easeInOutCubic;
	[Space, SerializeField] private Transform doorObject;
	[SerializeField] private AudioSource doorOpenSfx;
	[SerializeField] private AudioSource doorCloseSfx;

	private void OnTriggerEnter(Collider other) {
		if(other.TryGetComponent<PlayerController>(out PlayerController player)) {
			if(!interactableByPlayer) {
				return;
			}
			OpenDoor();
		}
	}

	private void OnTriggerExit(Collider other) {
		if(other.TryGetComponent<PlayerController>(out PlayerController player)) {
			if(!interactableByPlayer) {
				return;
			}
			CloseDoor();
		}
	}

	private void OpenDoor() {
		LeanTween.cancelAll();
		doorOpenSfx.Play();
		LTDescr tween = LeanTween.rotateLocal(doorObject.gameObject, new Vector3(0, openAngle, 0), animationTime);
		tween.setEase(easeType);
	}

	private void CloseDoor() {
		LeanTween.cancelAll();
		doorCloseSfx.Play();
		LTDescr tween = LeanTween.rotateLocal(doorObject.gameObject, new Vector3(0, closeAngle, 0), animationTime);
		tween.setEase(easeType);
	}
}