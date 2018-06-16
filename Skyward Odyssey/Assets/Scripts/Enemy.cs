using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : LivingEntity, IDamageable{

    public enum AttackType { Melee, Ranged, Tank };

    [SerializeField] AttackType attackType;
    [SerializeField] int hitsToKillPlayer = 2;
    [SerializeField] float chaseSpeed = 8f;
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float attackRange = 10f;
    [SerializeField] bool isPatrollingType = true;
    [SerializeField] float patrolLineWidth;
    [SerializeField] GameObject swordObject;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileForce;
    [SerializeField] float fireRatePerSecond = 1f;
	[SerializeField] Image healthBarImage;
    


    LivingEntity targetEntity;
    Flasher flasher;
    SpriteRenderer spriteRenderer;
    Transform target;
    Vector3 initialPosition;
    Rigidbody2D rb2d;
    float yDistanceToleration = 2f;
    float timeBetweenAttacks = 1f;
    float nextAttackTime;
    float damage;
    bool isFacingRight;
    Sword sword;

    void Awake(){
        if (GameObject.FindGameObjectWithTag("Player") != null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            damage = targetEntity.startingHealth / hitsToKillPlayer;
        }
        if(!isPatrollingType){
            patrolLineWidth = 0;
        }
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        flasher = GetComponent<Flasher>();
        isFacingRight = true;
        initialPosition = transform.position;
        rb2d.velocity = Vector2.right * patrolSpeed;
        if(attackType == AttackType.Melee){
            sword = swordObject.GetComponent<Sword>();
            sword.setDamage(damage);
        } 
        nextAttackTime = Time.time;
        timeBetweenAttacks = 1 / fireRatePerSecond;
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
        //Set a random initial direction
        Vector2 direction = Random.Range(0, 1) > 0.5 ? 
                                  Vector2.left : Vector2.right;
        rb2d.velocity = direction * chaseSpeed;
		healthBarImage.fillAmount = currentHealth / startingHealth;
    }

	void Update() {
		if (m_isMarkedDead) return;

        if (rb2d.velocity.x * transform.localScale.x < 0) {
            Flip();
        }
        if(!targetEntity.isDead() && IsPlayerInSight() && IsSelfInsidePatrolRange()){
            if(IsPlayerInAttackRange()){
                rb2d.velocity = Vector2.zero;
                Attack();
            }
            else if(isPatrollingType) {
                Chase();
            }
        }
        else{
            if (attackType == AttackType.Melee) { sword.cancelAttack(); }
            
            if(isPatrollingType){
                Patrol();
            }
            else{
                rb2d.velocity = Vector2.zero;
            }
        }

        // Tank Flashes when ready to attack
        if (attackType == AttackType.Tank) {
            if (Time.time > nextAttackTime)
            {
                GetComponents<Flasher>()[1].flash(spriteRenderer);
            }
        }
        /*
        If player is in patrolling area plus attack range
            assign player as target
            if target is in range
                attack  
            else if patrolling type
                move towards target
        else 
            has no target anymore
            if patrolling type
                patrol
            else
                do nothing (stand still)
        */	    
	}

	bool IsPlayerInSight(){
        Vector3 offset = target.position - initialPosition;
        if(Mathf.Abs(offset.y) < yDistanceToleration 
           && Mathf.Abs(offset.x) < attackRange + patrolLineWidth / 2){
            return true;
        }

        return false;
    } 

    bool IsPlayerInAttackRange(){
        Vector3 offset = target.position - transform.position;
        if (Mathf.Abs(offset.x) < attackRange) {
            return true;
        }

        return false;
    }

    void Attack(){
        if(attackType == AttackType.Melee){
            sword.attack(Sword.AttackMode.Light);
        }
        else if(attackType == AttackType.Ranged){
            if(Time.time > nextAttackTime){
                LaunchProjectile();
                nextAttackTime = Time.time + timeBetweenAttacks;
            }
        } else if (attackType == AttackType.Tank){
            if(Time.time > nextAttackTime){
                LaunchWave();
                nextAttackTime = Time.time + timeBetweenAttacks;
            }
        }
    }

    void Chase(){
        Vector2 direction = target.transform.position.x < transform.position.x ?
                                  Vector2.left : Vector2.right;
        
        rb2d.velocity = direction * chaseSpeed;
    }

    void Patrol(){
        Vector3 offset =  transform.position - initialPosition;
        //If the enemy is on the left end of patrol line
        if(offset.x < -patrolLineWidth / 2){
            rb2d.velocity = Vector2.right * patrolSpeed;
        }
        //If the enemy is on the right end of patrol line
        else if(offset.x > patrolLineWidth / 2){
            rb2d.velocity = Vector2.left * patrolSpeed;
        }
        else{
            Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
            rb2d.velocity = direction * patrolSpeed;
        }
    }

    void LaunchProjectile(){
        GameObject projectileObj = Instantiate(projectilePrefab
                                            , transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        Vector2 direction = target.transform.position.x < transform.position.x ?
                                  Vector2.left : Vector2.right;
        projectile.setDirection(direction);
        projectile.setDamage(damage);
        projectile.setKnockback(projectileForce);
    }

    void LaunchWave(){
        Debug.Log("Wave Launched");

        // launch 3 projectiles to the right
        for (int i = 0; i < 3; ++i)
        {
            Vector3 pos = transform.position;
            pos.x += (i * 1.5f);
            pos.y -= (i * .5f);

            GameObject projectileObj = Instantiate(projectilePrefab, pos, Quaternion.identity);

            Projectile projectile = projectileObj.GetComponent<Projectile>();
            
            projectile.setDirectionDiagonal(true);

            projectile.setDamage(damage);
            projectile.setKnockback(projectileForce);
        }
        // launch 3 projectile to the left
        for (int i = 0; i < 3; ++i)
        {
            Vector3 pos = transform.position;
            pos.x -= (i * 1.5f);
            pos.y -= (i * .5f);

            GameObject projectileObj = Instantiate(projectilePrefab, pos, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
   
            projectile.setDirectionDiagonal(false);

            projectile.setDamage(damage);
            projectile.setKnockback(projectileForce);
        }
    }

    bool IsSelfInsidePatrolRange(){
        if (attackType == AttackType.Ranged || attackType == AttackType.Tank) return true;
        Vector3 offset = transform.position - initialPosition;

        return (offset.x >= -patrolLineWidth / 2 
                && offset.x <= patrolLineWidth / 2);
    }

    void Flip() {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
		healthBarImage.transform.localScale = new Vector3(
			-healthBarImage.transform.localScale.x, 
			healthBarImage.transform.localScale.y, 
			healthBarImage.transform.localScale.z);
    }

	public override void TakeDamage(float damage) {
        flasher.flash(spriteRenderer);
        base.TakeDamage(damage);
		healthBarImage.fillAmount = currentHealth / startingHealth;
	}

    void OnDrawGizmosSelected() {
        if(isPatrollingType){
            Vector3 patrolRangeLeftEnd = transform.position + (Vector3.left * patrolLineWidth / 2);
            Vector3 patrolRangeRightEnd = transform.position + (Vector3.right * patrolLineWidth / 2);


            Gizmos.color = Color.red;
            Gizmos.DrawLine(patrolRangeLeftEnd, patrolRangeRightEnd);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, attackRange / 2);
    }
}
