using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        m_animator.SetTrigger("Attack");
    }

    public void DoDamage() {
        m_animator.SetTrigger("Hit");
    }
}
