using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetMenuLogic : MonoBehaviour
{
    AudioSource m_audioSource;
    [SerializeField]
    AudioClip m_clickSound;

    public GameObject ingameMenu;
    // Start is called before the first frame update
    void Start()
    {
        
        m_audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnSetClicked();
        }
        
    }

    public void OnSetClicked()
    {
        m_audioSource.PlayOneShot(m_clickSound);
        Time.timeScale = 0;
        ingameMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnContinueClicked()
    {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(250);
        Time.timeScale = 1f;
        ingameMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

    }


    public void OnQuitClicked()
    {
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(150);
        Debug.Log("Clicked Quit!");
        Application.Quit();

    }
    public void OnBackClicked(){
        m_audioSource.PlayOneShot(m_clickSound);
        Thread.Sleep(150);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
