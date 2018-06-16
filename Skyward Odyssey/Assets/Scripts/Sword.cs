using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    [SerializeField] float knockbackForce;

    public enum AttackMode {
        Light, Heavy, Combo, Air
    }

    float damage;

    Animator m_animator;
    bool m_isAttacking;

	// Use this for initialization
	void Start () {
        m_animator = GetComponent<Animator>();
	}

    public void attack(AttackMode mode){
        m_isAttacking = true;
        switch(mode){
            case AttackMode.Light:
                m_animator.SetTrigger("attack_light");
                break;
            case AttackMode.Heavy:
                m_animator.SetTrigger("attack_heavy");
                break;
            case AttackMode.Combo:
                m_animator.SetTrigger("attack_combo");
                break;
            case AttackMode.Air:
                m_animator.SetTrigger("attack_air");
                break;
            default:
                Debug.LogWarning("Sword's attack mode not found!");
                break;
        }
    }

    public void attackFinished(){
        m_isAttacking = false;
    }

    public bool isAttacking(){
        return m_isAttacking;
    }

    public void setDamage(float damage){
        this.damage = damage;
    }

    public void cancelAttack(){
        foreach(AnimatorControllerParameter trigger in m_animator.parameters){
            if(trigger.type == AnimatorControllerParameterType.Trigger){
                m_animator.ResetTrigger(trigger.name);
            }
        }
        //m_animator.SetTrigger("idle");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        /* Player hits Enemy */
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        // Dirty hack to prevent sword colliding with other colliders in the same game object
        if (m_isAttacking && enemy != null 
            && collision.gameObject.tag.Equals("Enemy")
            && (collision as CapsuleCollider2D) != null)
        {
            enemy.TakeDamage(damage);

            float knockbackDir = this.transform.parent.transform.localScale.x;
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackDir * knockbackForce, 400));
        }

        /* Player hits a Button */
        Button button = collision.gameObject.GetComponent<Button>();
        if (m_isAttacking && button != null)
        {
            button.Press();
        }

        /* Enemy hits player */
        CapsuleController player = collision.gameObject.GetComponent<CapsuleController>();
        if(m_isAttacking && player != null
            && collision.gameObject.tag.Equals("Player")
            && (collision as CapsuleCollider2D) != null){
            player.TakeDamage(damage);

            
            float knockbackDir = this.transform.parent.transform.localScale.x;

            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackDir * knockbackForce, 400));
        }
    }

	void OnTriggerExit2D(Collider2D collision) {
        m_isAttacking = false;	
	}
}
