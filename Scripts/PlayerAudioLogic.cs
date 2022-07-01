using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioLogic : MonoBehaviour{



    AudioSource m_audioSource;

    [SerializeField]
    AudioClip m_stepSound;
    [SerializeField]
    AudioClip m_beat1;
    [SerializeField]
    AudioClip m_beat2;
    [SerializeField]
    AudioClip m_beat3;
    [SerializeField]
    AudioClip m_brandish;
    [SerializeField]
    AudioClip m_storm;

    [SerializeField]
    AudioClip m_heavy_brandish;
    [SerializeField]
    AudioClip m_block;

    [SerializeField]
    AudioClip m_yell;

    public GameObject player;
    PlayerAnimation m_playeranimation;
    PlayerLogic playerLogic;

    // Start is called before the first frame update
    void Start()
    {
        m_playeranimation = player.GetComponent<PlayerAnimation>();
        m_audioSource = GetComponent<AudioSource>();
        playerLogic = player.GetComponent<PlayerLogic>();
        
    }
    public void Load() {
        playerLogic.Load();
    }
    public void SetSkill1() {
        m_playeranimation.SetSkill1();
    }

    public void SetSkill2() {
        m_playeranimation.SetSkill2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirstBeat()
    {
        m_audioSource.PlayOneShot(m_beat1);
    }
    public void SecondBeat(){
        m_audioSource.PlayOneShot(m_beat2);
    }

    public void ThirdBeat(){
        m_audioSource.PlayOneShot(m_beat3);
    }

    public void Skill1(){
        m_audioSource.PlayOneShot(m_brandish);
    }

    public void Skill2(){
        m_audioSource.PlayOneShot(m_heavy_brandish);
    }
    public void Skill3(){
        FirstBeat();
        
        SecondBeat();
        
    }

    public void Skill4(){
        m_audioSource.PlayOneShot(m_storm);
    }

    public void Block() {
        m_audioSource.PlayOneShot(m_block);
    }

    public void yell(){
        m_audioSource.PlayOneShot(m_yell);
    }
    public void step(){
        m_audioSource.PlayOneShot(m_stepSound);
    }


}
