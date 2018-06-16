using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using EZCameraShake;
using UnityEngine.UI;

[RequireComponent(typeof(Sword))]
public class CapsuleController : LivingEntity {

	[SerializeField]
	bool GOD_MODE;

    [SerializeField]
    float m_horizontalSpeed = 9f;
    [SerializeField]
    float m_jumpForce = 400f;
    [SerializeField]
    float damage = 40f;
    [SerializeField]
    GameObject swordObject;
	[SerializeField]
	Image healthBar;
    //[SerializeField] 
    //LayerMask m_WhatIsGround;


    Animator m_animator;
    Rigidbody2D m_rigidBody2D;
    bool m_isFacingRight;
    bool m_isGrounded;
    bool m_isJumping;
    CircleCollider2D groundCollider;
    Sword sword;
    Flasher flasher;
    SpriteRenderer spriteRenderer;


	// Use this for initialization
 	protected override void Start () {
        base.Start();
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        groundCollider = GetComponent<CircleCollider2D>();
        m_isGrounded = false;
        m_isFacingRight = true;
        m_animator = GetComponentInChildren<Animator>();
        sword = swordObject.GetComponent<Sword>();
        sword.setDamage(damage);
        flasher = GetComponent<Flasher>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	// Update is called once per frame
	void FixedUpdate(){
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        move(horizontalInput);
	}

	void Update(){
		if (m_isMarkedDead) return;

        if (!m_isJumping && m_isGrounded){
            // Read the jump input in Update so button presses aren't missed.
            m_isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
            m_animator.SetBool("is_jumping", m_isJumping);
        }


        if (Input.GetKeyDown(KeyCode.F5)) { currentHealth = startingHealth; }
        healthBar.fillAmount = currentHealth / startingHealth;


        handleAttackInput();

	}

    public void handleAttackInput(){
        /* Handle Attack Input if currently not attacking */
        if (!sword.isAttacking()) {
            //Perform ground attacks
            if (m_isGrounded) {
                if (Input.GetKeyDown(KeyCode.J)) {
                    sword.attack(Sword.AttackMode.Light);
					SoundManager.Instance.PlayFX(SoundManager.SoundFX.SwordSwing1);
                }
                else if (Input.GetKeyDown(KeyCode.K)) {
                    sword.attack(Sword.AttackMode.Heavy);
					SoundManager.Instance.PlayFX(SoundManager.SoundFX.SwordSwing2);
                }
                else if (Input.GetKeyDown(KeyCode.L)) {
                    sword.attack(Sword.AttackMode.Combo);
					SoundManager.Instance.PlayFX(SoundManager.SoundFX.SwordSwing3);
                }
            }
            //Perform air attack
            else if (Input.GetKeyDown(KeyCode.J)
                     || Input.GetKeyDown(KeyCode.K)
                     || Input.GetKeyDown(KeyCode.L)
                    ) {
                sword.attack(Sword.AttackMode.Air);
				SoundManager.Instance.PlayFX(SoundManager.SoundFX.SwordSwing1);
            }
        }
    }

	public void move(float horizontalInput){
        //Set the velocity according to input
        m_rigidBody2D.velocity = new Vector2(horizontalInput * m_horizontalSpeed
                                             , m_rigidBody2D.velocity.y);
        m_animator.SetFloat("Speed", System.Math.Abs(m_rigidBody2D.velocity.x));


        //Jumping
        m_animator.SetBool("is_jumping", m_isJumping);
        if (m_isGrounded && m_isJumping){
            m_isGrounded = false;
            m_isJumping = false;
            m_rigidBody2D.velocity = new Vector2(0, m_rigidBody2D.velocity.y);
            m_rigidBody2D.AddForce(new Vector2(0f, m_jumpForce));
        }


        m_animator.SetBool("is_grounded", m_isGrounded);


        // Handle Character direction
        if (horizontalInput > 0 && !m_isFacingRight){
            Flip();
        }
        else if (horizontalInput < 0 && m_isFacingRight){
            Flip();
        }
    }

    void Flip(){
        m_isFacingRight = !m_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public override void TakeDamage(float damage) {
		if (GOD_MODE) return;
        flasher.flash(spriteRenderer);
		CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);
        base.TakeDamage(damage);
		SoundManager.Instance.PlayFX(SoundManager.SoundFX.PlayerGotHit);
		healthBar.fillAmount = currentHealth / startingHealth;
    }

	public void IncreaseHealth(float percent){
		float value = Mathf.Clamp(percent, 0, 1);
		currentHealth += startingHealth * value;
		currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
		healthBar.fillAmount = currentHealth / startingHealth;
	}

	void OnCollisionStay2D(Collision2D collision){
        if (collision.collider.Equals(groundCollider)
            || collision.otherCollider.Equals(groundCollider)) {
            m_isGrounded = true;
        }
	}

	void OnCollisionExit2D(Collision2D collision){
        if (collision.collider.Equals(groundCollider)
            || collision.otherCollider.Equals(groundCollider)){
            m_isGrounded = false;
        }
	}
}
