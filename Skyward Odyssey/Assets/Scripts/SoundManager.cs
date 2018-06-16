using UnityEngine;

public class SoundManager : MonoBehaviour {

    [HideInInspector]
	public static SoundManager Instance;

	public enum SoundFX {
        SwordSwing1, SwordSwing2, SwordSwing3, HealthPickUp, PlayerGotHit,
        PlayerDeath, EnemyDeath
    }
	[SerializeField] AudioClip gameMusic;
	[SerializeField] AudioClip swordSwing1;
	[SerializeField] AudioClip swordSwing2;
	[SerializeField] AudioClip swordSwing3;
	[SerializeField] AudioClip healthPickUp;
	[SerializeField] AudioClip playerGotHit;
	[SerializeField] AudioClip playerDeath;
	[SerializeField] AudioClip enemyDeath;
    

	public AudioSource soundFXSource;
	public AudioSource musicSource;


	void Awake() {
		if(Instance == null){
			Instance = this;
		}
		else if(Instance != this){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

	}

	void Start() {
		musicSource.loop = true;
		musicSource.clip = gameMusic;
		musicSource.Play();
	}

	public void PlayFX(SoundFX soundFX){
		AudioClip clip = null;
		switch (soundFX) {
			case SoundFX.SwordSwing1:
				clip = swordSwing1;
				break;
			case SoundFX.SwordSwing2:
                clip = swordSwing2;
                break;
			case SoundFX.SwordSwing3:
                clip = swordSwing1;
                break;
			case SoundFX.HealthPickUp:
                clip = healthPickUp;
                break;
			case SoundFX.PlayerGotHit:
				clip = playerGotHit;
                break;
			case SoundFX.PlayerDeath:
				clip = playerDeath;
				musicSource.Stop();
				musicSource.clip = clip;
				musicSource.loop = false;
				musicSource.Play();
				break;
			case SoundFX.EnemyDeath:
                clip = enemyDeath;
                break;
		}
		soundFXSource.Stop();
		soundFXSource.clip = clip;
		soundFXSource.Play();
	}

}
