using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 10;

    float damage;
    float force;
    Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setDirection(Vector2 dir){
        rb2d.velocity = dir.normalized * speed;
		transform.localScale *= -dir.x;
    }

	public void setDirection(Vector3 dir) {
        rb2d.velocity = dir.normalized * speed;
        transform.Rotate(Quaternion.FromToRotation(Vector3.left, dir).eulerAngles);
        //transform.localScale *= -dir.x;
    }

    public void setDirectionDiagonal(bool isFacingRight){
        Vector2 dir; dir.x = 1; dir.y = 1;
        float angle;

        if (isFacingRight){
            angle = -135;
            dir.x = dir.y = 1;
        } else{
            angle = -45;
            dir.x = -1; dir.y = 1;
        }

        rb2d.velocity = dir * speed;
        transform.Rotate(new Vector3(0, 0, transform.rotation.z+angle));
    }

    public void setDamage(float damage){
        this.damage = damage;
    }

    public void setKnockback(float knockbackForce) {
        force = knockbackForce;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        CapsuleController player = collision.gameObject.GetComponent<CapsuleController>();

        

        if (player != null) {
            player.TakeDamage(damage);
            Destroy(gameObject);

            float knockbackDir = this.transform.localScale.normalized.x * -1;
			if(force > 0){
				player.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockbackDir * force, 400));
			}
            print(force);
        } else if (collision.gameObject.tag == "Wall") {
            Destroy(gameObject);
        }
	}
}
