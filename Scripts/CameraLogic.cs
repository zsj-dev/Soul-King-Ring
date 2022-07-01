using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLogic : MonoBehaviour{

    public PlayerLogic player_Input;
    public float speed_Horizontal = 100.0f;
    public float speed_Vertical = 80.0f;
    private float temp_EulerX = 20.0f;

    private GameObject camera_Handle;
    private GameObject player_Handle;

    public GameObject player;
    public Image lock_Center;
    public GameObject lock_Target;
    public bool is_Locked;


    void Awake() {
        is_Locked = false;
        lock_Center.enabled = false;
        camera_Handle = transform.parent.gameObject;
        player_Handle = camera_Handle.transform.parent.gameObject;
    }

    void Update() {

    }

    void FixedUpdate() {
        Vector3 player_Euler = player.transform.eulerAngles;
        if(lock_Target == null) {
            player_Handle.transform.Rotate(Vector3.up, player_Input.horizontal_Camera * speed_Horizontal * Time.deltaTime);
            temp_EulerX -= player_Input.vertical_Camera * speed_Vertical * Time.deltaTime;
            temp_EulerX = Mathf.Clamp(temp_EulerX, -20, 30);
            camera_Handle.transform.localEulerAngles = new Vector3(temp_EulerX, 0, 0);
        }
        else {
            Vector3 lock_Forward = lock_Target.transform.position - player.transform.position;
            lock_Forward.y = 0;
            player_Handle.transform.forward = lock_Forward;         
        }
        player.transform.eulerAngles = player_Euler;
    }

    public void Lock() {
        Collider[] colliders = Physics.OverlapBox(player.transform.position + new Vector3(0, 1, 0) + player.transform.forward * 5.0f,
                                                    new Vector3(0.5f, 0.5f, 5f), player.transform.rotation,
                                                    LayerMask.GetMask("Enemy"));
        if (colliders.Length == 0) {
            lock_Target = null;
            is_Locked = false;
            lock_Center.enabled = false;
        }
        else {
            foreach (var collider in colliders) {
                if (lock_Target == collider.gameObject) {
                    is_Locked = false;
                    lock_Target = null;
                    lock_Center.enabled = false;
                    break;
                }
                lock_Target = collider.gameObject;
                is_Locked = true;
                lock_Center.enabled = true;
                break;
            }
        }
    }

}
