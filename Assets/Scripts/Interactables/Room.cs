using UnityEngine;
using UnityEngine.UI;

public class Room : Interactable
{
	public bool isUnlocked = true;
	public bool isOccupied = false;
	public bool isDirty = false;

	private AudioSource roomUnlocked;

	[SerializeField] private BoxCollider doorCollider;
	[SerializeField] private GameObject cleanUI;
	[SerializeField] private Image progressFill;
	[SerializeField] private AudioSource clock;

	private void Awake() {
		if(!isUnlocked) {
			transform.localScale = Vector3.right;
		}
		roomUnlocked = GetComponent<AudioSource>();
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			SetDirty(true);
		}
	}

	protected override void PlayerInteracted(PlayerController player) {
		if(isDirty) {
			StartCleaning();
		}
	}

	protected override void PlayerStoppedInteracting(PlayerController player) {
        if (isDirty) {
			CancelCleaning();
        }
    }

	private void StartCleaning() {
		const float CLEAN_TIME = 5f;
		clock.Play();
		LTDescr tween = LeanTween.value(gameObject, 0f, 1f, CLEAN_TIME);
		tween.setOnUpdate((float fillAmount) => {
			progressFill.fillAmount = fillAmount;
		});
		tween.setEase(LeanTweenType.linear);
		tween.setOnComplete(() => SetDirty(false));
	}

	private void CancelCleaning() {
		LeanTween.cancelAll();
		progressFill.fillAmount = 0f;
		clock.Stop();
	}

	public void UnlockRoom() {
		doorCollider.enabled = true;
		roomUnlocked.Play();
	}

	public void SetDirty(bool value) {
		progressFill.fillAmount = 0f;
		isDirty = value;
		cleanUI.SetActive(value);
	}
}