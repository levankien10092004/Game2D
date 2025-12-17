using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager =GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        audioManager.PlaySFX(audioManager.Chose);
    }
    public void OutGame()
    {
        Application.Quit();
        audioManager.PlaySFX(audioManager.Chose);
    }
    public void ResetProgress()
    {
        audioManager.PlaySFX(audioManager.Chose);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}