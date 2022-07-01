using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class EnemyBattleManager : MonoBehaviour
{
    public EnemyActorManager m_ActorManager;
    private CapsuleCollider defense_Collider;

    void Start() {
        defense_Collider = GetComponent<CapsuleCollider>();
        defense_Collider.center = Vector3.up;
        defense_Collider.height = 2.0f;
        defense_Collider.radius = 0.5f;
        defense_Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider col) {
        if (col.tag == "Weapon") {
            m_ActorManager.DoDamage();
        }
    }
}
