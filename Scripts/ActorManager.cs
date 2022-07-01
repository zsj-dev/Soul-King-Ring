using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour{

    public PlayerAnimation m_PlayerAnimation;
    public BattleManager m_BattleManager;
    public WeaponManager m_WeaponManager;
    public StateManager m_StateManager;
   
    void Awake() {

        m_PlayerAnimation = GetComponent<PlayerAnimation>();
        GameObject player = m_PlayerAnimation.player;
        GameObject sensor = transform.Find("Sensor").gameObject;

        m_BattleManager = sensor.GetComponent<BattleManager>();
        if(m_BattleManager == null) {
            m_BattleManager = sensor.AddComponent<BattleManager>();
        }
        m_BattleManager.m_ActorManager = this;

        m_WeaponManager = player.GetComponent<WeaponManager>();
        if (m_WeaponManager == null) {
            m_WeaponManager = player.AddComponent<WeaponManager>();
        }
        m_WeaponManager.m_ActorManager = this;

        m_StateManager = gameObject.GetComponent<StateManager>();
        if(m_StateManager == null) {
            m_StateManager = gameObject.GetComponent<StateManager>();
        }
        m_StateManager.m_ActorManager = this;
    }

    public void TryDoDamage(float damage) {
        if (m_StateManager.isImmortal) {
            // immortal
        }
        else if (m_StateManager.isDefense) {
            Defense();
        }
        else {
            if(m_StateManager.HP <= 0) {
                // die
            }
            else {
                m_StateManager.AddHP(-damage);
                if (m_StateManager.HP > 0) {
                    Hit();
                }
                else {
                    Die();
                    m_PlayerAnimation.player_Input.move_Enabled = false;
                }
            }
        }
    }

    private void FixedUpdate() {
        TryDefense();
    }

    public void TryDefense() {
        
        if(m_StateManager.MP == 0) {
            m_PlayerAnimation.can_defense = false;
        }
        else if(m_StateManager.MP == 100){
            m_PlayerAnimation.can_defense = true;
        }

        m_StateManager.MP += Time.fixedDeltaTime * 50;
        if (m_PlayerAnimation.m_Animator.GetBool("Defense")) {
            m_StateManager.AddMp(-Time.deltaTime * 100);
        }
        
        
    }


    public void Defense() {
        m_PlayerAnimation.IssueTrigger("Block");
    }

    public void Hit() {
        m_PlayerAnimation.IssueTrigger("Hit");
    }

    public void Die() {
        m_PlayerAnimation.IssueTrigger("Die");
        if (m_PlayerAnimation.camera_Access.is_Locked) {
            m_PlayerAnimation.camera_Access.Lock();
        }
        m_PlayerAnimation.camera_Access.enabled = false;
        m_PlayerAnimation.IssueTrigger("Born");
        Invoke("fullblood",3.5f);
    }

    public void fullblood() {
        m_StateManager.HP = 100.0f;
    }

}
