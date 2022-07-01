using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDLogic : MonoBehaviour
{

    public Image CD_Image1;
    public Text CD_Text1;

    public Image CD_Image2;
    public Text CD_Text2;

    public Image CD_Image3;
    public Text CD_Text3;

    public Image CD_Image4;
    public Text CD_Text4;




    public GameObject m_player;
    PlayerAnimation m_playerLogic;

    // Start is called before the first frame update
    void Start()
    {
        m_playerLogic = m_player.GetComponent<PlayerAnimation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CD_Image1.fillAmount = (float)m_playerLogic.cd_skill1 / (float)m_playerLogic.max_cd_skill1;
        CD_Image2.fillAmount = (float)m_playerLogic.cd_skill2 / (float)m_playerLogic.max_cd_skill2;
        CD_Image3.fillAmount = (float)m_playerLogic.cd_skill3 / (float)m_playerLogic.max_cd_skill3;
        CD_Image4.fillAmount = (float)m_playerLogic.cd_skill4 / (float)m_playerLogic.max_cd_skill4;
        UpdateAmmoText();
        
    }
    void UpdateAmmoText()
    {
        if(m_playerLogic.cd_skill1>0){
            CD_Text1.text=(float)((int)(m_playerLogic.cd_skill1*10)*1.0)/(10)+" s";
        }else{
            CD_Text1.text=" ";
        }

        if (m_playerLogic.cd_skill2 >0) {
            CD_Text2.text = (float)((int)(m_playerLogic.cd_skill2 * 10) * 1.0) / (10) + " s";
        }
        else {
            CD_Text2.text = " ";
        }

        if (m_playerLogic.cd_skill3 > 0) {
            CD_Text3.text = (float)((int)(m_playerLogic.cd_skill3 * 10) * 1.0) / (10) + " s";
        }
        else {
            CD_Text3.text = " ";
        }

        if (m_playerLogic.cd_skill4 > 0) {
            CD_Text4.text = (float)((int)(m_playerLogic.cd_skill4 * 10) * 1.0) / (10) + " s";
        }
        else {
            CD_Text4.text = " ";
        }

    }
}
