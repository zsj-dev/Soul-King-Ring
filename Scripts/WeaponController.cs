using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour{
    
   
    public float m_currentDamage = 20.0f;
    public float m_currentPoiseDamage = 30.0f;

    
    void OnTriggerEnter(Collider other) {

        IDamageable dmg = other.gameObject.GetComponent<IDamageable>();
        //Debug.Log(dmg);
        if (dmg!=null) {
            dmg.TakeHit(m_currentDamage, m_currentPoiseDamage);
        }
    }

}
