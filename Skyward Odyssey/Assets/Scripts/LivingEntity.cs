using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {

    public float startingHealth;

    public event System.Action OnDeath;

    protected float currentHealth;
    protected bool m_isDead;
	protected bool m_isMarkedDead;

    protected virtual void Start () {
        currentHealth = startingHealth;
	}

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection){
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage){
        currentHealth -= damage;
        if(currentHealth <= 0 && !m_isDead){
			StartCoroutine(Die());
        }
    }
    
    public bool isDead(){
        return m_isDead;
    }

	public void markDead(){
		//Cancel the update loop
		m_isMarkedDead = true;
		//Stop the enemy
		Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = Vector2.zero;
		rb2d.bodyType = RigidbodyType2D.Static;
        //Disable colliders
        foreach (Collider2D col in GetComponents<Collider2D>()) {
            col.enabled = false;
        }
	}

	[ContextMenu("Self Destruct")]
	public virtual IEnumerator Die(){
		markDead();
		if(GetComponent<Enemy>() != null){
			SoundManager.Instance.PlayFX(SoundManager.SoundFX.EnemyDeath);
		}
		else if(GetComponent<CapsuleController>() != null){
            SoundManager.Instance.PlayFX(SoundManager.SoundFX.PlayerDeath);
		}
        yield return StartCoroutine(disappear(1));
        currentHealth = 0;
        m_isDead = true;

        //Broadcast death if there are any subscribers aka "observers"
        if(OnDeath != null){
            OnDeath();
        }
        Destroy(gameObject);
    }

	IEnumerator disappear(float duration) {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        float alphaReduceSpeed = 1 / duration;
        float percent = 0;

        while (percent < 1f) {
            percent += Time.deltaTime * alphaReduceSpeed;
            foreach (SpriteRenderer sr in spriteRenderers) {
                Color temp = sr.color;
                temp.a = Mathf.Lerp(1f, 0f, percent);
                sr.color = temp;
            }
            yield return null;
        }
    }
}
