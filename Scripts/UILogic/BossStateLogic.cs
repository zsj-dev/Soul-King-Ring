using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossStateLogic : MonoBehaviour
{
    public Image HP;
    public Image Po;
    public GameObject Boss;
    BossLogic m_bossLogic;

    // Start is called before the first frame update
    void Start() {
        m_bossLogic = Boss.GetComponent<BossLogic>();

    }

    // Update is called once per frame
    void Update() {
        HP.fillAmount = (float)m_bossLogic.Health / (float)100;
        Po.fillAmount = (float)m_bossLogic.Poise / (float)100;

    }
}
