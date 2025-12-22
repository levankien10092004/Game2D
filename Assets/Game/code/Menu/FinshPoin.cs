using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinshPoin : MonoBehaviour
{
    public GameObject victory;

    AudioManager audioManager;

    [Header("Star UI")]
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCoins>()?.SaveCoins();
            audioManager.PlaySFX(audioManager.VicTory);
            victory.gameObject.SetActive(true);
            ShowStars();
            UnlockNewLevel();
            Time.timeScale = 0f;
          
        }
    }
    public void OnNextLevelButton()
    {
        Time.timeScale = 1f;
        
        SceneController.instance.NextLevel();
        audioManager.StopSFX();
        audioManager.PlaySFX(audioManager.Chose);
    }

    void UnlockNewLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int unlocked = PlayerPrefs.GetInt("unlockedLevel", 1);

        if (current+1 > unlocked)
        {
            PlayerPrefs.SetInt("unlockedLevel", current+1);
            PlayerPrefs.Save();
        }
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        audioManager.StopSFX();
        audioManager.PlaySFX(audioManager.Chose);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        audioManager.StopSFX();
        audioManager.PlaySFX(audioManager.Chose);
    }
    void ShowStars()
    {
        int stars = ChestManager.instance.GetStarResult();

        star1.SetActive(stars >= 1);
        star2.SetActive(stars >= 2);
        star3.SetActive(stars >= 3);
    }
}
