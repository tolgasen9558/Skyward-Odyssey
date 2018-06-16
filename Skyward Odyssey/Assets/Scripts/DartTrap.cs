using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class DartTrap : MonoBehaviour {

	public enum ShootRotation { Up, Right, Down, Left };

	public GameObject projectilePrefab;
	public ShootRotation shootTowards;
	public float fireRatePerSecond = 1f;
	public int hitsToKillPlayer = 10;


	float damage;
	float nextAttackTime;
	float timeBetweenAttacks;
	Vector3 dir;

	// Use this for initialization
	void Start () {
		timeBetweenAttacks = 1 / fireRatePerSecond;
		nextAttackTime = Time.time;
		damage = GameObject.FindGameObjectWithTag("Player")
		                   .GetComponent<LivingEntity>().startingHealth / hitsToKillPlayer;

		switch(shootTowards){
			case ShootRotation.Up:
				dir = Vector3.up;
				break;
			case ShootRotation.Down:
                dir = Vector3.down;
                break;
			case ShootRotation.Left:
				dir = Vector3.left;
                break;
			case ShootRotation.Right:
                dir = Vector3.right;
                break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > nextAttackTime){
			nextAttackTime = Time.time + timeBetweenAttacks;
			LaunchProjectile();
		}
	}

	void LaunchProjectile() {
        GameObject projectileObj = Instantiate(projectilePrefab
                                            , transform.position, Quaternion.identity) as GameObject;

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.setDirection(dir);
        projectile.setDamage(damage);
        projectile.setKnockback(0);
    }
}
