using UnityEngine;

public class HealthPickUp : MonoBehaviour {


	public GameObject particleEffect;
    [Range(0.1f, 1f)]
	public float healsPercent;

	void OnTriggerEnter2D(Collider2D collision) {
		CapsuleController player = collision.GetComponent<CapsuleController>();

		if(player != null){
			player.IncreaseHealth(healsPercent);
			Instantiate(particleEffect, transform.position, Quaternion.identity);
			SoundManager.Instance.PlayFX(SoundManager.SoundFX.HealthPickUp);
			Destroy(gameObject);
		}
	}
}
