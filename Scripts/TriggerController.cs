using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour{

    private Animator m_Animator;
    
    void Awake(){
        m_Animator = GetComponent<Animator>();
    }

    public void ResetTrigger(string trigger) {
        m_Animator.ResetTrigger(trigger);
    }
    
}
