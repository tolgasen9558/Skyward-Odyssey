using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

    //Wrapper function for "takeDamage()" to carry out hit deatils 
    // TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection);

    //The function that actually deals damage
    void TakeDamage(float damage);

}
