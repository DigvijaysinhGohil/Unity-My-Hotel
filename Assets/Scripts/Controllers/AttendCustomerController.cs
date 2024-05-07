using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendCustomerController : Interactable {
	private bool isPlayerAttending = false;
	private NPC customer;

	[SerializeField] private List<Room> rooms;
	[SerializeField] private WaitingQueueController waitingQueueController;
	[SerializeField] private CanvasGroup attendUI;
	[SerializeField] private Image progressFill;
	[SerializeField] private AudioSource clock;
	[SerializeField] private MoneyBundle moneyBundle;
	[SerializeField] private RandomPositionGenerator moneySpawnArea;

	private bool IsNpcInArea { get { return customer != null && waitingQueueController.IsCustomer(customer); } }

	private void Update() {
		attendUI.alpha = CanAttendCustomer() ? 1f : 0.2f;
	}

	protected override void PlayerInteracting(PlayerController player) {
		if(CanAttendCustomer()) {
			StartAttending();
		}
	}

	protected override void PlayerStoppedInteracting(PlayerController player) {
		if(CanAttendCustomer()) {
			CancelAttending();
		}
	}

	protected override void NpcInteracated(NPC npc) {
		customer = npc;
	}

	protected override void NpcStoppedInteracting(NPC npc) {
		customer = null;
	}

	private bool CanAttendCustomer() {
		Room availableRoom = rooms.Find(room => room.isUnlocked && !room.isOccupied && !room.isDirty);
		return IsNpcInArea && availableRoom != null;
	}

	private void StartAttending() {
		if(isPlayerAttending)
			return;

		isPlayerAttending = true;
		LeanTween.cancelAll();
		const float ATTEND_TIME = 3f;
		clock.Play();
		LTDescr tween = LeanTween.value(gameObject, 0f, 1f, ATTEND_TIME);
		tween.setOnUpdate((float fillAmount) => {
			progressFill.fillAmount = fillAmount;
		});
		tween.setEase(LeanTweenType.linear);
		tween.setOnComplete(() => {
			SpawnMoney();
			progressFill.fillAmount = 0f;
			clock.Stop();
			NPC customer = waitingQueueController.GetFirstCustomer();
			if(customer != null) {
				GotoRoom(customer);
			}
			isPlayerAttending = false;
		});
	}

	private void GotoRoom(NPC customer) {
		Room availableRoom = rooms.Find(room => room.isUnlocked && !room.isOccupied && !room.isDirty);
		if(availableRoom != null) {
			availableRoom.isOccupied = true;
			customer.MoveTo(availableRoom.GetSleepPos());
		}
	}

	private void CancelAttending() {
		LeanTween.cancelAll();
		progressFill.fillAmount = 0f;
		clock.Stop();
		isPlayerAttending = false;
	}

	private void SpawnMoney() {
		Instantiate(moneyBundle, moneySpawnArea.GetPosition(), Quaternion.identity);
	}

}