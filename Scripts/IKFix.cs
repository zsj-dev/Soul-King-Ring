using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFix : MonoBehaviour{
    private Animator m_Animator;

    private Vector3 down_LeftArm = new Vector3(0,-35,0);


    void Awake() {
        m_Animator = GetComponent<Animator>();
    }

    void OnAnimatorIK() {
        Transform left_Arm = m_Animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);

        if (!m_Animator.GetBool("Defense")) {
            left_Arm.localEulerAngles += down_LeftArm;
            m_Animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(left_Arm.localEulerAngles));
        }

        
    }
}
