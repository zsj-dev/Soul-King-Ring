using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour{

    public ActorManager m_ActorManager;

    public float MAX_HP = 100.0f;
    public float MAX_MP = 100.0f;
    public float HP = 100.0f;
    public float MP = 100.0f;

    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isDefense;

    public bool isImmortal;


    void Start() {
        HP = MAX_HP;
    }

    void Update() {
        isGround = m_ActorManager.m_PlayerAnimation.CheckState("BaseMove");
        isJump = m_ActorManager.m_PlayerAnimation.CheckState("Jump");
        isFall = m_ActorManager.m_PlayerAnimation.CheckState("Fall");
        isRoll = m_ActorManager.m_PlayerAnimation.CheckState("Roll");
        isJab = m_ActorManager.m_PlayerAnimation.CheckState("Jab");
        isAttack = m_ActorManager.m_PlayerAnimation.Check_Attack();
        isHit = m_ActorManager.m_PlayerAnimation.CheckState("Hit");
        isDie = m_ActorManager.m_PlayerAnimation.CheckState("Die");
        isDefense = m_ActorManager.m_PlayerAnimation.CheckState("Defense") || m_ActorManager.m_PlayerAnimation.CheckState("DefenseLoop");
        isImmortal = isRoll || isJab;
    }

    private void FixedUpdate() {

    }

    public void AddHP(float value) {
        HP += value;
        HP = Mathf.Clamp(HP, 0, MAX_HP);
    }

    public void AddMp(float value) {
        MP += value;
        MP = Mathf.Clamp(MP, 0, MAX_MP);
    }
}
