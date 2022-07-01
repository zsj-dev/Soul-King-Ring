using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour{

    public float move_Speed = 2.0f;
    public float jump_Velocity = 3.5f;
    public bool move_Locked = false;
    public CameraLogic camera_Access;

    public GameObject player;
    public PlayerLogic player_Input;

    [SerializeField]
    public Animator m_Animator;
    private Rigidbody m_Rigibody;
    private Vector3 move_Vector;
    private Vector3 offset_Vector;
    private Vector3 offset_RootMotion;
    private bool lock_Track;


    public bool can_defense = true;

    public float cd_skill1 = 0.0f;
    public float max_cd_skill1 = 1.5f;

    public float cd_skill2 = 0.0f;
    public float max_cd_skill2 = 1.5f;

    public float cd_skill3 = 0.0f;
    public float max_cd_skill3 = 6.0f;

    public float cd_skill4 = 0.0f;
    public float max_cd_skill4 = 20.0f;


    // Start is called before the first frame update
    void Awake(){
        m_Rigibody = GetComponent<Rigidbody>();
        player_Input = GetComponent<PlayerLogic>();
        m_Animator = player.GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update(){
        MoveAnimation();
        JumpAnimation();
        SkillAnimation();
        AttackAnimation();
        DefenseAnimation();
        CameraController();
    }

    void FixedUpdate() {
        m_Rigibody.position += offset_RootMotion;
        m_Rigibody.velocity = new Vector3(move_Vector.x, m_Rigibody.velocity.y, move_Vector.z) + offset_Vector;
        offset_Vector = Vector3.zero;
        offset_RootMotion = Vector3.zero;


        cd_skill1 = (cd_skill1 > 0) ? cd_skill1 - Time.fixedDeltaTime : 0;
        cd_skill2 = (cd_skill2 > 0) ? cd_skill2 - Time.fixedDeltaTime : 0;
        cd_skill3 = (cd_skill3 > 0) ? cd_skill3 - Time.fixedDeltaTime : 0;
        cd_skill4 = (cd_skill4 > 0) ? cd_skill4 - Time.fixedDeltaTime : 0;
    }

    void MoveAnimation() {
        

        if (m_Animator.GetBool("Attacking")) {
            player_Input.move_Enabled = false;
        }

        float run_Multiplier = player_Input.is_Running ? 2.0f : 1.0f;

        if (camera_Access.is_Locked == false) {
            m_Animator.SetFloat("MoveV", player_Input.move_Magnitude * Mathf.Lerp(m_Animator.GetFloat("MoveV"), run_Multiplier, 0.1f));
            m_Animator.SetFloat("MoveH", 0);
            m_Animator.SetFloat("Speed", m_Animator.GetFloat("MoveV"));
        }
        else {
            Vector3 localDirection = transform.InverseTransformVector(player_Input.move_Direction);
            m_Animator.SetFloat("MoveV", Mathf.Abs(localDirection.z * run_Multiplier));
            m_Animator.SetFloat("MoveH", Mathf.Abs(localDirection.x * run_Multiplier));
            m_Animator.SetFloat("Speed", player_Input.CoordinateTransform(m_Animator.GetFloat("MoveV"), m_Animator.GetFloat("MoveH")).sqrMagnitude);
        }
        
        
        if(camera_Access.is_Locked == false) {
            if (player_Input.move_Magnitude > 0.1f) {
                player.transform.forward = Vector3.Slerp(player.transform.forward, player_Input.move_Direction, 0.1f);
            }
            if (!move_Locked) {
                move_Vector = ((player_Input.move_Magnitude * player.transform.forward) * move_Speed) * run_Multiplier;
            }
        }
        else {
            if (lock_Track == false) {
                player.transform.forward = transform.forward;
            }
            else {
                player.transform.forward = move_Vector.normalized;
            }
            if (!move_Locked) {
                move_Vector = player_Input.move_Direction * move_Speed * run_Multiplier;
            }
        }

    }

    void JumpAnimation() {
        if (player_Input.is_Jumping) {
            m_Animator.SetTrigger("Jump");
        }
    }

    void AttackAnimation() {
        if (player_Input.is_Attack && (CheckState("BaseMove")||CheckTag("NormalAttack")|| CheckState("Roll") ||CheckState("Jump")) && !m_Animator.IsInTransition(m_Animator.GetLayerIndex("MoveLayer"))) {
            m_Animator.SetTrigger("IsAttack");
        }
    }

    void DefenseAnimation() {
        if (can_defense) {
            m_Animator.SetBool("Defense", player_Input.is_Defense);
        }
        else {
            m_Animator.SetBool("Defense", false);
        }
        //player_Input.move_Enabled = false;
    }

    public void SetSkill1() {
        cd_skill1 = max_cd_skill1;
    }

    public void SetSkill2() {
        cd_skill2 = max_cd_skill2;
    }

    void SkillAnimation() {
        if (player_Input.is_Skill1 && cd_skill3 == 0) {
            cd_skill3 = max_cd_skill3;
            m_Animator.SetTrigger("Skill1");
            m_Animator.SetTrigger("IsAttack");
        }
        else if (player_Input.is_Skill2 & cd_skill4 == 0) {
            cd_skill4 = max_cd_skill4;
            m_Animator.SetTrigger("Skill2");
            m_Animator.SetTrigger("IsAttack");
        }
    }

    public bool CheckState(string stateName, string layername = "MoveLayer") {
        return m_Animator.GetCurrentAnimatorStateInfo(m_Animator.GetLayerIndex(layername)).IsName(stateName);
    }
    public bool CheckTag(string tagName, string layername = "MoveLayer") {
        return m_Animator.GetCurrentAnimatorStateInfo(m_Animator.GetLayerIndex(layername)).IsTag(tagName);
    }

    void CameraController() {
        if (player_Input.is_Lock) {
            camera_Access.Lock();
        }
    }

    public bool Check_Attack() {
        return m_Animator.GetBool("Attacking");
    }

    //
    // Fsm Events
    //
    public void OnJumpEnter() {
        offset_Vector = new Vector3(0, jump_Velocity, 0);
        move_Locked = true;
        lock_Track = true;
        player_Input.move_Enabled = false;
    }
    public void OnRollEnter() {
        move_Locked = true;
        lock_Track = true;
        player_Input.move_Enabled = false;
    }
    public void OnRollUpdate() {
        offset_Vector = player.transform.forward * m_Animator.GetFloat("roll_Velocity");
    }

    public void OnJabEnter() {
        move_Locked = true;
        player_Input.move_Enabled = false;
    }

    public void OnJabUpdate() {
        offset_Vector = player.transform.forward * m_Animator.GetFloat("jab_Velocity");
    }

    public void IsOnGround() {
        m_Animator.SetBool("IsGround", true);
    }

    public void NotOnGround() {
        m_Animator.SetBool("IsGround", false);
        if(m_Rigibody.velocity.magnitude > 5.0f) {
            m_Animator.SetTrigger("Roll");
        }
    }
    public void OnGroundEnter() {
        lock_Track = false;
        move_Locked = false;
        player_Input.move_Enabled = true;
        m_Animator.SetBool("Attacking", false);
    }

    public void OnNormalAttack1Enter() {
        m_Animator.SetBool("Attacking", true);
    }

    public void OnDefenseEnter() {
        player_Input.move_Enabled = false;
    }

    public void OnUpdateRootMotion(object offset) {
        offset_RootMotion += (Vector3)offset;
    }

    public void OnHitEnter() {
        player_Input.move_Enabled = false;
    }

    public void IssueTrigger(string triggerName) {
        m_Animator.SetTrigger(triggerName);
    }

    public void OnDieEnter() {
        player_Input.move_Enabled = false;
    }

    public void OnDieExit() {
        player_Input.move_Enabled = true;
    }


    
}
