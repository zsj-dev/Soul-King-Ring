using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitTest : MonoBehaviour{


    Animator m_Animator;
    // Start is called before the first frame update
    void Start(){
        m_Animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.J)) {
            m_Animator.SetTrigger("Hit");
        }
        
    }
}
