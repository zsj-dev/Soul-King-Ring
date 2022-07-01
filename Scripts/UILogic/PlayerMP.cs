using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMP : MonoBehaviour
{

    public Image MP;
    public GameObject m_player;
    StateManager m_statemanager;

    // Start is called before the first frame update
    void Start(){
        m_statemanager = m_player.GetComponent<StateManager>();
        
    }

    // Update is called once per frame
    void Update(){
        MP.fillAmount = (float)m_statemanager.MP / (float)100;
        
    }
}
