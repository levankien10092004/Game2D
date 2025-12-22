using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager =GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();    
    }
    public void  Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Home()
    {
        audioManager.PlaySFX(audioManager.Chose);
        audioManager.StopSFX();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
       
    }
    public void Resume()
    {
        audioManager.PlaySFX(audioManager.Chose);

        menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        audioManager.PlaySFX(audioManager.Chose);
        audioManager.StopSFX();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
