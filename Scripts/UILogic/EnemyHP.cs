using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //Slider的调用需要引用UI源文件

public class EnemyHP : MonoBehaviour
{
    Slider HP;  //实例化一个Slider

    public GameObject m_enemy;
    public GameObject m_camera;
    EnemyLogic m_enemyLogic;


    private void Start()
    {
        HP = this.GetComponent<Slider>();
        m_enemyLogic = m_enemy.GetComponent<EnemyLogic>();

    }

    void Update()
    {
        
        HP.value=m_enemyLogic.Health/100.0f;
        transform.LookAt(m_camera.transform.position);
        
    }

}
