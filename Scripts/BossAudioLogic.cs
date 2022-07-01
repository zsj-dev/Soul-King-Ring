using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioLogic : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip m_skill1;
    public AudioClip m_skill2;
    public AudioClip m_skill3;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skill1() {
        audio.PlayOneShot(m_skill1);

    }
    public void Skill2() {
        audio.PlayOneShot(m_skill2);

    }
    public void Skill3() {
        audio.PlayOneShot(m_skill3);

    }

}
