using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;
using TMPro;
public class MainMenuLogic : MonoBehaviour
{
    AudioSource m_audioSource;
    [SerializeField]
    AudioClip m_clickSound;

    public GameObject controlMenu;
    public GameObject mainMenu;
    public GameObject languageMenu;

    public TextMeshProUGUI main_t;
    public TextMeshProUGUI main_s;
    public TextMeshProUGUI main_c;
    public TextMeshProUGUI main_l;
    public TextMeshProUGUI main_q;
    public TextMeshProUGUI lan_t;
    public TextMeshProUGUI lan_c;
    public TextMeshProUGUI lan_e;
    public TextMeshProUGUI lan_q;
    public TextMeshProUGUI con_t;
    public TextMeshProUGUI con_b;
    // Start is called before the first frame update
    void Start()
    {
        
        m_audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartClicked()
    {
        m_audioSource.PlayOneShot(m_clickSound);
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }

    public void OnControlsClicked()
    {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(250);
        controlMenu.SetActive(true);
        languageMenu.SetActive(false);
        mainMenu.SetActive(false);
    }
    public void OnLangugeClicked() {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(250);
        languageMenu.SetActive(true);
        controlMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void OnQuitClicked()
    {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(250);
        Debug.Log("Clicked Quit!");
        Application.Quit();

    }
    public void OnBackClicked()
    {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(250);
        mainMenu.SetActive(true);
        controlMenu.SetActive(false);
        languageMenu.SetActive(false);

    }
    public void OnEnglishClicked() {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(250);
        main_c.text = "Controls";
        main_t.text = "Soul Ki Ring";
        main_s.text = "Start";
        main_l.text = "Languge";
        main_q.text = "Quit";
        con_b.text = "Back";
        con_t.text = "Operation";
        lan_t.text = "Languge";
        lan_c.text = "Chinese";
        lan_e.text = "English";
        lan_q.text = "Back";
    }
    public void OnChineseClicked() {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(250);
        main_c.text = "����";
        main_t.text = "��֮��";
        main_s.text = "��ʼ";
        main_l.text = "����";
        main_q.text = "�˳�";
        con_b.text = "����";
        con_t.text = "������";
        lan_t.text = "����";
        lan_c.text = "����";
        lan_e.text = "Ӣ��";
        lan_q.text = "����";
    }
}
