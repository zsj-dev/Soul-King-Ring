using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour{

    private Animator m_Animator;

    void Awake() {
        m_Animator = GetComponent<Animator>();
    }

    void OnAnimatorMove() {
        SendMessageUpwards("OnUpdateRootMotion", (object)m_Animator.deltaPosition);
    }


}
