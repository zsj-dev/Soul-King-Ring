using UnityEngine;
using System.Collections;

public class WeaponLogic : MonoBehaviour
{
    const float closeDmg = 30;
    const float midDmg = 40;
    const float farDmg = 50;

    BossLogic m_logic;
    void start() {
        m_logic = GetComponent<BossLogic>();
    }

    public BossLogic.PlayerPosition m_currentType = BossLogic.PlayerPosition.Nil;

    public float Damage = 0;

    // Update is called once per frame
    void Update() {
        switch ( m_currentType ) {
            case BossLogic.PlayerPosition.Close:
                Damage = closeDmg;
                break;
            case BossLogic.PlayerPosition.Mid:
                Damage = midDmg;
                break;
            case BossLogic.PlayerPosition.Far:
                Damage = farDmg;
                break;
            case BossLogic.PlayerPosition.Nil:
                Damage = 0;
                break;
        }
    }


}
