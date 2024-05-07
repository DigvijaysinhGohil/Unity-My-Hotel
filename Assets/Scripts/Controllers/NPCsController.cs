using System.Collections.Generic;
using UnityEngine;

public class NPCsController : MonoBehaviour
{
	public List<NPC> freeRoamingNpc;
	public List<NPC> customerNpc;

	[SerializeField] private WaitingQueueController waitingQueueController;

	private void Awake() {
		freeRoamingNpc = new List<NPC>(GetComponentsInChildren<NPC>());
		customerNpc = new List<NPC>();
	}

	[ContextMenu("Make Customer")]
	public void MakeNpcCustomer() {
		if(freeRoamingNpc.Count < 0)
			return;

		if(!waitingQueueController.CanAddCustomerToQueue())
			return;

		NPC npc = freeRoamingNpc[Random.Range(0, freeRoamingNpc.Count)];
		freeRoamingNpc.Remove(npc);
		customerNpc.Add(npc);

		waitingQueueController.AddCustomerToQueue(npc);
	}

	[ContextMenu("Make Free Roamer")]
	public void MakeNpcFreeRoamer() {
		if(customerNpc.Count < 0)
			return;

		NPC npc = customerNpc[0];
		customerNpc.Remove(npc);
		freeRoamingNpc.Add(npc);

		waitingQueueController.RemoveCustomerFromQueue(npc);
	}
}