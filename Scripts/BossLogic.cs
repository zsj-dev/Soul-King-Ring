using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossLogic : MonoBehaviour, IDamageable
{

    enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Staggered
    }

    public enum PlayerPosition
    {
        Close,
        Mid,
        Far,
        Nil
    }

    [SerializeField]
    public float Health = 100;
    [SerializeField]
    public float Poise = 100;

    [SerializeField]
    Transform Spawn;

    EnemyState m_state = EnemyState.Idle;
    NavMeshAgent m_navMeshAgent;
    GameObject m_currentTarget;
    GameObject m_playerObject;
    float m_currentSpeed;
    float distance;
    bool m_isAttacking = false;
    float m_poiseCounter;
    bool m_isInterrupted;
    bool m_isHittable = true;
    bool m_poiseRegenerating = false;
    bool m_isRegenerating = false;
    bool m_isShielding = false;

    PlayerPosition m_playerPos;
    public PlayerPosition m_atkType = PlayerPosition.Nil;

    public bool if_enable = false;
    WeaponLogic m_weaponLogic;

    public Collider weapon_Collider;

    Animator m_animator;
    EnemyHP m_enemyHP;
    const float m_detectionAngle = 50;
    const float m_attackRadius = 10.0f;
    const float m_detectionRadius = 10;
    const float m_escapeRadius = 50.0f;
    const float m_attackCoolDown = 2.0f;
    const float m_poiseRegenSpeed = 20;
    const float m_healthRegenSpeed = 50;
    const float m_poiseWaitTime = 5;
    const float m_maxPoise = 100;
    const float m_staggerTime = 5;
    const float m_staggerMultiplier = 2.0f;
    const float m_maxHealth = 100;
    const float m_midAttackProb = 40/2;
    const float m_farAttackProb = 30/2;
    const float m_closeAttackProb = 50/2;
    const float m_midRange = 2;
    const float m_farRange = 3;
    const float m_shieldingSpeed = 1;
    const float m_chaseSpeed = 5;
    const float m_shieldingDefense = 0.3f;
    const float m_backStabMultiplier = 2.0f;
    const float m_closeAtkTime = 3.0f;
    const float m_midAtkTime = 5.0f;
    const float m_farAtkTime = 3.0f;
    const float m_comfyDist = 5.0f;

    public GameObject weapon;
    // Start is called before the first frame update
    void Start() {
        weapon_Collider.enabled = false;
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_playerObject = GameObject.FindGameObjectWithTag( "Player" );
        distance = Vector3.Distance( m_playerObject.transform.position, transform.position );
        m_animator = GetComponent<Animator>();
        m_enemyHP = GetComponent<EnemyHP>();
        m_poiseCounter = m_poiseWaitTime;
        Poise = m_maxPoise;
        m_currentTarget = m_playerObject;
        m_playerPos = PlayerPosition.Far;
        m_weaponLogic = weapon.GetComponent<WeaponLogic>();
    }

    // Update is called once per frame
    void Update() {
        if_enable = weapon_Collider.enabled;

        if ( m_currentTarget == null ) {
            HandleDetection();
        }

        if ( m_poiseCounter <= 0 ) {
            m_poiseCounter = m_poiseWaitTime;
            StartCoroutine( PoiseRegen() );
        }

        if ( m_state == EnemyState.Attacking ) {
            m_animator.SetBool( "AttackState", true );
            m_navMeshAgent.speed = m_shieldingSpeed;
        } else {
            m_animator.SetBool( "AttackState", false );
            m_navMeshAgent.speed = m_chaseSpeed;
        }
    }


    private void FixedUpdate() {
        distance = Vector3.Distance( m_playerObject.transform.position, transform.position );
        m_currentSpeed = ( m_navMeshAgent.velocity.magnitude / m_chaseSpeed );
        
        if ( Poise < m_maxPoise ) {
            if ( !m_isInterrupted ) {
                m_poiseCounter -= m_poiseWaitTime * Time.deltaTime;
            } else {
                m_poiseCounter = m_poiseWaitTime;
                m_isInterrupted = false;
            }
        }


        switch ( m_state ) {
            case EnemyState.Idle:
                if ( m_currentTarget != null ) {
                    m_state = EnemyState.Chasing;
                }
                if ( Health < m_maxHealth && !m_isRegenerating ) {
                    StartCoroutine( ForcedHealthRegen() );
                }
                break;
                m_navMeshAgent.SetDestination( Spawn.position );

            case EnemyState.Chasing:

                m_navMeshAgent.speed = m_chaseSpeed;
                if ( m_isAttacking ) {
                    StopCoroutine( HandleAttackEvent() );
                    m_isAttacking = false;
                }
                if ( distance < m_attackRadius ) {
                    m_state = EnemyState.Attacking;
                } else if ( distance > m_escapeRadius ) {
                    m_state = EnemyState.Idle;
                    m_currentTarget = null;
                } else {
                    m_navMeshAgent.SetDestination( m_playerObject.transform.position );
                }
                break;

            case EnemyState.Attacking:
                m_navMeshAgent.SetDestination( m_playerObject.transform.position );

                if ( distance < m_midRange ) {
                    m_playerPos = PlayerPosition.Close;
                } else if ( distance < m_farRange ){
                    m_playerPos = PlayerPosition.Mid;
                } else {
                    m_playerPos = PlayerPosition.Far;
                }
                
                if ( !m_isAttacking ) {
                    m_isShielding = true;
                    StartCoroutine( HandleAttackEvent() );
                }


                if ( distance > m_attackRadius ) {
                    m_state = EnemyState.Chasing;
                }
                break;
            case EnemyState.Staggered:
                break;
        }

        // Reporting to the Animator
        m_animator.SetFloat( "Speed", m_currentSpeed );

    }


    // Finding Player within FOV
    void HandleDetection() {
        if ( distance < m_detectionRadius ) {
            Vector3 playerDirection = m_playerObject.transform.position - transform.position;
            float angle = Vector3.Angle( playerDirection, transform.forward );
            if ( Mathf.Abs( angle ) < m_detectionAngle ) {
                m_currentTarget = m_playerObject;
            }
        }
    }
    IEnumerator HandleAttackEvent() {
        m_isAttacking = true;
        bool m_hasAttacked = false;
        //Debug.Log( m_hasAttacked );
        //Debug.Log( m_hasAttacked );
        while ( m_hasAttacked == false ) {
            float randNum = Random.Range( 0, 100 );
            yield return new WaitForSeconds( 1 );
            
            switch ( m_playerPos ) {
                case PlayerPosition.Close:
                    if ( randNum <= m_closeAttackProb ) {
                        PoiseInterrupt();
                        
                        m_isShielding = false;
                        StartCoroutine( CloseAttack() );
                        m_hasAttacked = true;
                    }
                    break;
                case PlayerPosition.Mid:
                    if ( randNum <= m_midAttackProb ) {
                        PoiseInterrupt();
                        m_isShielding = false;

                        StartCoroutine( MidAttack() );
                        m_hasAttacked = true;
                    }
                    break;
                case PlayerPosition.Far:
                    if ( randNum <= m_farAttackProb ) {
                        PoiseInterrupt();
                        m_isShielding = false;

                        StartCoroutine( FarAttack() );
                        m_hasAttacked = true;
                    }
                    break;
            }
        }

        while ( m_isAttacking ) {
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator CloseAttack() {
        m_weaponLogic.m_currentType = PlayerPosition.Close;
        m_animator.SetTrigger( "Close Attack" );
        m_atkType = PlayerPosition.Close;
        yield return new WaitForSeconds( m_closeAtkTime );
        m_isAttacking = false;
        m_atkType = PlayerPosition.Nil;
        m_weaponLogic.m_currentType = PlayerPosition.Nil;

    }
    IEnumerator MidAttack() {
        m_weaponLogic.m_currentType = PlayerPosition.Mid;

        m_animator.SetTrigger( "Mid Attack" );
        m_atkType = PlayerPosition.Mid;
        yield return new WaitForSeconds( m_midAtkTime );
        m_isAttacking = false;
        m_atkType = PlayerPosition.Nil;
        m_weaponLogic.m_currentType = PlayerPosition.Nil;

    }
    IEnumerator FarAttack() {
        m_weaponLogic.m_currentType = PlayerPosition.Far;

        m_animator.SetTrigger( "Far Attack" );
        m_atkType = PlayerPosition.Far;
        yield return new WaitForSeconds( m_farAtkTime );
        m_isAttacking = false;
        m_atkType = PlayerPosition.Nil;
        m_weaponLogic.m_currentType = PlayerPosition.Nil;

    }


    public void TakeHit( float damage, float poise ) {
        Debug.Log("hit!");
        Debug.Log(Health);
        float modifiedDamage = damage/5;
        if ( m_state == EnemyState.Staggered ) {
            modifiedDamage *= m_staggerMultiplier;
            Vector3 playerDirection = m_playerObject.transform.position - transform.position;
            float angle = Vector3.Angle( playerDirection, transform.forward );
            if ( Mathf.Abs( angle ) < m_detectionAngle ) {
                modifiedDamage *= m_backStabMultiplier;
            }
        } else if ( m_isShielding ) {
            Vector3 playerDirection = m_playerObject.transform.position - transform.position;
            float angle = Vector3.Angle( playerDirection, transform.forward );
            if ( Mathf.Abs( angle ) < m_detectionAngle ) {
                modifiedDamage *= m_shieldingDefense;
            }
        }


        if ( m_isHittable ) {
            if ( Health - modifiedDamage <= 0 ) {
                Health = 0.0f;
                StartCoroutine( DeathEvent() );
            } else {
                Health -= modifiedDamage;

                if ( m_state != EnemyState.Staggered
                    && Poise - poise <= 0 ) {
                    HandleStagger();
                } else {
                    if ( m_state != EnemyState.Staggered ) {
                        Poise -= poise * 1.5f;
                        m_animator.SetTrigger( "NormalReturn" );
                    } else {
                        m_animator.SetTrigger( "ExitHit" );
                    }
                    if ( m_state != EnemyState.Attacking ) {
                        m_animator.SetTrigger( "TakeHit" );
                    }
                    PoiseInterrupt();
                }
            }
            m_currentTarget = m_playerObject;
        }

    }

    void PoiseInterrupt() {
        if ( m_poiseRegenerating ) {
            StopCoroutine( PoiseRegen() );
        }
        m_isInterrupted = true;
    }

    void HandleStagger() {
        m_state = EnemyState.Staggered;
        m_animator.SetTrigger( "Stagger" );
        StartCoroutine( ForcedPoiseRegen() );
        m_navMeshAgent.isStopped = true;
        StartCoroutine( ExitStagger() );
    }

    IEnumerator DeathEvent() {
        m_animator.SetTrigger( "Death" );
        yield return new WaitForSeconds( 4 );
        // Drop Something
        Destroy( gameObject );
    }

    IEnumerator PoiseRegen() {
        m_poiseRegenerating = true;
        while ( Poise < m_maxPoise ) {
            yield return new WaitForEndOfFrame();
            Poise += m_poiseRegenSpeed * Time.deltaTime;
        }
        m_poiseRegenerating = false;
    }

    IEnumerator ForcedPoiseRegen() {
        while ( Poise < m_maxPoise ) {
            yield return new WaitForEndOfFrame();
            Poise += m_poiseRegenSpeed * Time.deltaTime;
        }

    }

    IEnumerator ExitStagger() {
        yield return new WaitForSeconds( m_staggerTime );
        m_animator.SetTrigger( "ExitStagger" );
        m_navMeshAgent.isStopped = false;
        m_state = EnemyState.Chasing;
    }


    IEnumerator ForcedHealthRegen() {
        m_isRegenerating = true;
        m_isHittable = false;
        m_navMeshAgent.isStopped = true;
        
        while ( Health < m_maxHealth ) {
            yield return new WaitForEndOfFrame();
            Health += m_healthRegenSpeed * Time.deltaTime;
        }
        m_isRegenerating = false;
        m_isHittable = true;
        m_navMeshAgent.isStopped = false;
        
    }

    public void WeaponEnable() {
        weapon_Collider.enabled = true;
    }

    public void WeaponDisable() {
        weapon_Collider.enabled = false;
    }


}
