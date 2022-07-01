using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour{

    public ActorManager m_ActorManager;
    public Collider weapon_ColliderLeft;
    public Collider weapon_ColliderRight;
    
    public GameObject weapon_Left;
    public GameObject weapon_Right;
    

    void Start() {
        weapon_ColliderLeft = weapon_Left.GetComponent<Collider>();
        weapon_ColliderRight = weapon_Right.GetComponent<Collider>();
        weapon_ColliderLeft.enabled = false;
        weapon_ColliderRight.enabled = false;
    }

    public void WeaponEnable() {
        weapon_ColliderRight.enabled = true;
    }

    public void WeaponDisable() {
        weapon_ColliderRight.enabled = false;
    }


}
