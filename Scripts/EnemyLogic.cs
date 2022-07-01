using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour, IDamageable
{
    public Collider weapon_Collider;

    enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Staggered
    }

    [SerializeField]
    public float Health = 100;
    [SerializeField]
    public float Poise;

    [SerializeField]
    Transform Spawn;
    [SerializeField]
    Transform[] PatrolPoints;

    //Transform[] PatrolPoints;
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
    bool m_isPatrolling = false;
    
   
    Animator m_animator;
    EnemyHP m_enemyHP;
    const float m_detectionAngle = 50;
    const float m_attackRadius = 2.0f;
    const float m_detectionRadius = 10;
    const float m_escapeRadius = 10.0f;
    const float m_attackCoolDown = 3.0f;
    const float m_poiseRegenSpeed = 20;
    const float m_poiseWaitTime = 5;
    const float m_maxPoise = 100;
    const float m_staggerTime = 3;
    const float m_staggerMultiplier = 1.5f;
    const float m_patrolWaitTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_playerObject = GameObject.FindGameObjectWithTag( "Player" );
        distance = Vector3.Distance( m_playerObject.transform.position, transform.position );
        m_animator = GetComponent<Animator>();
        m_enemyHP = GetComponent<EnemyHP>();
        m_poiseCounter = m_poiseWaitTime;
        Poise = m_maxPoise;
        //PatrolPoints = new Transform[Spawn.childCount];
        //for ( int i = 0; i < PatrolPoints.Length; i++ ) {
        //    PatrolPoints[i] = transform.GetChild( i );
        //}
        //PatrolPoints[0] = Spawn;

    }

    // Update is called once per frame
    void Update()
    {
        if ( m_currentTarget == null ) {
            HandleDetection();
        }

        if (m_poiseCounter <= 0 ) {
            m_poiseCounter = m_poiseWaitTime;
            StartCoroutine( PoiseRegen() );
        }
       
    }


    private void FixedUpdate() {
        distance = Vector3.Distance( m_playerObject.transform.position, transform.position );
        m_currentSpeed = m_navMeshAgent.velocity.magnitude / 3;
        if ( Poise < m_maxPoise ) {
            if ( !m_isInterrupted ) {
                m_poiseCounter -= m_poiseWaitTime * Time.deltaTime;
            } else {
                m_poiseCounter = m_poiseWaitTime;
                m_isInterrupted = false;
            }
        }


        switch (m_state) {
            case EnemyState.Idle:
                if ( m_currentTarget != null ) {
                    m_state = EnemyState.Chasing;
                }
                //m_navMeshAgent.SetDestination( Spawn.position );

                //Patrolling
                if ( !m_isPatrolling ) {
                    StartCoroutine( Patrol() );

                }


                break;

            case EnemyState.Chasing:
                StopCoroutine( Patrol() );
                m_isPatrolling = false;
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
                StopCoroutine(Patrol());

                if ( !m_isAttacking ) {
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
            if ( Mathf.Abs(angle) < m_detectionAngle ) {
                m_currentTarget = m_playerObject;
            }
        }
    }

    IEnumerator HandleAttackEvent() {
        PoiseInterrupt();
        m_isAttacking = true;
        m_animator.SetTrigger( "Attack" );
        yield return new WaitForSeconds( m_attackCoolDown );
        m_isAttacking = false;
    }

    public void TakeHit( float damage, float poise ) {
        float modifiedDamage = damage;
        if ( m_state == EnemyState.Staggered ) {
            modifiedDamage *= m_staggerMultiplier;
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
                        Poise -= poise;
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
        m_navMeshAgent.isStopped = true;
        m_animator.SetTrigger( "Death" );
        yield return new WaitForSeconds(4);
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

    IEnumerator Patrol( ) {
        m_isPatrolling = true;
        while ( true ) {
            foreach ( Transform point in PatrolPoints ) {

                while ( true ) {
                    m_navMeshAgent.SetDestination( point.position );
                    if ( Vector3.Distance( transform.position, point.position ) < 5.0f ) {

                        yield return new WaitForSeconds( m_patrolWaitTime );
                        break;
                    }
                    yield return null;
                }
            
            }
        }
    }

    public void WeaponEnable() {
        weapon_Collider.enabled = true;
    }

    public void WeaponDisable() {
        weapon_Collider.enabled = false;
    }


}
