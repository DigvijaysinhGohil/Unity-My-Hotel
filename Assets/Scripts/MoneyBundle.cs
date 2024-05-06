using UnityEngine;

public class MoneyBundle : MonoBehaviour
{
	private AudioSource collectMoney;
	private BoxCollider boxCollider;
	
	public float money = 20f;

	[SerializeField] private MoneyController moneyController;
	[SerializeField] private ParticleSystem moneyParticles;

	private void Awake() {
		collectMoney = GetComponent<AudioSource>();
		boxCollider = GetComponent<BoxCollider>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.TryGetComponent<PlayerController>(out PlayerController player)) {
			Destroy(boxCollider);
			ParticleSystem particles = Instantiate(moneyParticles, transform.position, Quaternion.identity);
			particles.Play();
			const float DESTROY_DELAY = 2f;
			Destroy(particles.gameObject, DESTROY_DELAY);
			moneyController.AddMoney(money);
			foreach(Transform child in transform) {
				child.gameObject.SetActive(false);
			}
			collectMoney.Play();
			Destroy(gameObject, DESTROY_DELAY);
		}
	}
}