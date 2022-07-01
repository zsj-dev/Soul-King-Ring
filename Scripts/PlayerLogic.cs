using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour{

    [Header("==== Key Settings ====")]
    // move keys
    public string key_Up = "w";
    public string key_Down = "s";
    public string key_Left = "a";
    public string key_Right = "d";
    public string key_Running = "left shift";
    public string key_Jumping = "space";
    // attack keys
    public string key_Attack = "mouse 0";
    public string key_Defense = "left ctrl";

    // camera keys
    public string key_CUp = "up";
    public string key_CDown = "down";
    public string key_CRight = "right";
    public string key_CLeft = "left";

    public string key_Skill1 = "1";
    public string key_Skill2 = "2";

    [Header("==== Move Signals ====")]
    // vertical move 
    public float vertical_Target;
    private float vertical_Input;
    private float vertical_Velocity;
    public float vertical_Camera;
    // horizontal move
    public float horizontal_Target;
    private float horizontal_Input;
    private float horizontal_Velocity;
    public float horizontal_Camera;
    // move informations
    public bool is_Running;
    public bool is_Jumping;
    public bool move_Enabled = true;
    public float move_Magnitude;
    public Vector3 move_Direction;
    // attack informations
    public bool is_Attack;
    // camera informations
    public bool is_Lock;
    public bool is_Defense;
    public bool is_Skill1;
    public bool is_Skill2;

    public bool CursorLock = true;

    public GameObject savepoint1;
    public GameObject savepoint2;
    public GameObject savepoint3;

    public bool if_save = false;
    public int save_num = 1;

    public CameraLogic m_cameraLogic; 

    public Animator m_Animator;


    void Update() {
        Move();
        Attack();


        if (if_save && Input.GetKeyDown(KeyCode.C)) {
            Save();
            if(save_num == 1) {
                savepoint1.SetActive(true);
            }
            else if(save_num == 2) {
                savepoint2.SetActive(true);
            }
            else if(save_num == 3) {
                savepoint3.SetActive(true);

            }
        }
    }
    private void FixedUpdate() {
        vertical_Camera = Input.GetAxisRaw( "Mouse Y" );
        horizontal_Camera = Input.GetAxisRaw( "Mouse X" );
    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Move() {

        //camera



        //vertical_Camera = (Input.GetKey(key_CUp) ? 1.0f : 0) - (Input.GetKey(key_CDown) ? 1.0f : 0);
        //horizontal_Camera = (Input.GetKey(key_CRight) ? 1.0f : 0) - (Input.GetKey(key_CLeft) ? 1.0f : 0);
        // input
        vertical_Input = (Input.GetKey(key_Up) ? 1.0f : 0.0f) - (Input.GetKey(key_Down) ? 1.0f : 0.0f);
        horizontal_Input = (Input.GetKey(key_Right) ? 1.0f : 0.0f) - (Input.GetKey(key_Left) ? 1.0f : 0.0f);
        // move enable check
        if (move_Enabled == false) {
            vertical_Input = 0;
            horizontal_Input = 0;
        }
        // smoothdamp
        vertical_Target = Mathf.SmoothDamp(vertical_Target, vertical_Input, ref vertical_Velocity, 0.1f);
        horizontal_Target = Mathf.SmoothDamp(horizontal_Target, horizontal_Input, ref horizontal_Velocity, 0.1f);
        // normalized
        Vector2 move_Transform = CoordinateTransform(horizontal_Target, vertical_Target);
        move_Magnitude = move_Transform.sqrMagnitude;
        // rotation
        move_Direction = move_Transform.x * transform.right + move_Transform.y * transform.forward;
        is_Running = Input.GetKey(key_Running);
        is_Jumping = Input.GetKeyDown(key_Jumping);
        is_Lock = Input.GetMouseButtonDown(2); // middle mouse button
        is_Defense = Input.GetKey(key_Defense);
        is_Skill1 = Input.GetKeyDown(key_Skill1);
        is_Skill2 = Input.GetKeyDown(key_Skill2);
    }

    public Vector2 CoordinateTransform(float x, float y) {
        float new_x, new_y;
        new_x = x * Mathf.Sqrt(1 - (y * y) / 2.0f);
        new_y = y * Mathf.Sqrt(1 - (x * x) / 2.0f);
        return new Vector2(new_x,new_y);
    }

    void Attack() {
        is_Attack = Input.GetKeyDown(key_Attack);
    }

    // save and load
    public void Save() {
        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);

        PlayerPrefs.SetFloat("PlayerRotX", transform.rotation.eulerAngles.x);
        PlayerPrefs.SetFloat("PlayerRotY", transform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("PlayerRotZ", transform.rotation.eulerAngles.z);
    }

    public void Load() {
        float playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
        float playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
        float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");

        float playerRotX = PlayerPrefs.GetFloat("PlayerRotX");
        float playerRotY = PlayerPrefs.GetFloat("PlayerRotY");
        float playerRotZ = PlayerPrefs.GetFloat("PlayerRotZ");

        transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
        transform.rotation = Quaternion.Euler(playerRotX, playerRotY, playerRotZ);

        m_cameraLogic.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
