using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : MonoBehaviour{

    float damage = 0.0f;
    public ActorManager m_ActorManager;
    private CapsuleCollider defense_Collider;

    [SerializeField]
    WeaponLogic m_weaponlogic;


    void Start() {

        defense_Collider = GetComponent<CapsuleCollider>();
        defense_Collider.center = Vector3.up;
        defense_Collider.height = 2.0f;
        defense_Collider.radius = 0.5f;
        defense_Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider col) {
        if(col.tag == "Weapon") {
            //Debug.Log("aaa");
            damage = (m_weaponlogic.Damage > 0) ? m_weaponlogic.Damage : 10.0f;
            m_ActorManager.TryDoDamage(damage);
        }
    }


}
