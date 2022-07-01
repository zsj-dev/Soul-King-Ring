using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActorManager : MonoBehaviour{

    //public EnemyTest m_EnemyTest;
    public EnemyBattleManager m_BattleManager;
    public EnemyLogic m_enemylogic;

    void Awake() {

        // m_EnemyTest = GetComponent<EnemyTest>();
        m_enemylogic = GetComponent<EnemyLogic>(); 

        GameObject sensor = transform.Find("Sensor").gameObject;
        m_BattleManager = sensor.GetComponent<EnemyBattleManager>();
        if (m_BattleManager == null) {
            m_BattleManager = sensor.AddComponent<EnemyBattleManager>();
        }
        m_BattleManager.m_ActorManager = this;
    }

    public void DoDamage() {
        //print("aaa");
        m_enemylogic.TakeHit(10, 10); 
    }
}
