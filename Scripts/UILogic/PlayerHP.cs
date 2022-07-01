using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{

    public Image HP;
    public GameObject m_player;
    StateManager m_statemanager;

    // Start is called before the first frame update
    void Start()
    {
        m_statemanager = m_player.GetComponent<StateManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HP.fillAmount = (float)m_statemanager.HP / (float)100;
        
    }
}
