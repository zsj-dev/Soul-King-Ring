using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour{

    public CapsuleCollider m_Capsule;

    private Vector3 point_Low;
    private Vector3 point_High;
    private float radius_Capsule;

    // Start is called before the first frame update
    void Awake(){
        radius_Capsule = m_Capsule.radius;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(point_Low, radius_Capsule);
    }

    // Update is called once per frame
    void FixedUpdate(){
        point_Low = transform.position + transform.up * (radius_Capsule - 0.05f);
        point_High = transform.position + transform.up * m_Capsule.height - transform.up * radius_Capsule;
        Collider[] collider_Things = Physics.OverlapCapsule(point_Low, point_High, radius_Capsule, LayerMask.GetMask("Ground"));
        if(collider_Things.Length != 0) {
            SendMessageUpwards("IsOnGround");
        }
        else {
            SendMessageUpwards("NotOnGround");
        }
    }
}
