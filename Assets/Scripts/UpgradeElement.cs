using UnityEngine;
using UnityEngine.UI;

public class UpgradeElement : MonoBehaviour
{
	private AudioSource spendMoney;

	[SerializeField] private float upgradeCost = 50;
	[SerializeField] private Image progressFill;
	[SerializeField] private MoneyController moneyController;
	[SerializeField] private Room room;
	[SerializeField] private ParticleSystem particles;

	private void Awake() {
		spendMoney = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other) {
		if(other.TryGetComponent<PlayerController>(out PlayerController player)) {
			if(moneyController.Money >= upgradeCost) {
				StartUpgrade();
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if(other.TryGetComponent<PlayerController>(out PlayerController player)) {
			CancelUpgrade();
		}
	}

	private void CancelUpgrade() {
		particles.Stop();
		spendMoney.Stop();
		LeanTween.cancelAll();
		progressFill.fillAmount = 0;
	}

	private void StartUpgrade() {
		particles.Play();
		const float UPGRADE_TIME = 5f;
		spendMoney.Play();
		LTDescr tween = LeanTween.value(gameObject, 0f, 1f, UPGRADE_TIME);
		tween.setOnUpdate((float fillAmount) => {
			progressFill.fillAmount = fillAmount;
		});
		tween.setEase(LeanTweenType.linear);
		tween.setOnComplete(() => UnlockRoom());
	}

	private void UnlockRoom() {
		const float SCALE_Z_TIME = 0.4f;
		const float SCALE_Y_TIME = 0.2f;
		LeanTween.scaleZ(room.gameObject, 1f, SCALE_Z_TIME).setEase(LeanTweenType.easeInOutCubic).setOnComplete(() => {
			LeanTween.scaleY(room.gameObject, 1f, SCALE_Y_TIME).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => { 
				room.UnlockRoom();
				moneyController.SpendMoney(upgradeCost);
				Destroy(gameObject);
			});
		});
	}
}