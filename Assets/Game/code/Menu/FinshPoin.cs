using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinshPoin : MonoBehaviour
{
    public GameObject victory;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.GetComponent<PlayerCoins>()?.SaveCoins();
        audioManager.PlaySFX(audioManager.VicTory);

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        // ✅ NẾU LÀ LV3 → CHUYỂN ENDING
        if (currentScene == 4) // LV3 = buildIndex 4
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("EndScene");
            return;
        }

        // ✅ CÁC LEVEL KHÁC → HÀNH VI CŨ
        victory.SetActive(true);
        UnlockNewLevel();
        Time.timeScale = 0f;
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

        if (current + 1 > unlocked)
        {
            PlayerPrefs.SetInt("unlockedLevel", current + 1);
            PlayerPrefs.Save();
        }
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");


        Time.timeScale = 1;
        audioManager.StopSFX();

        audioManager.PlaySFX(audioManager.Chose);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        Time.timeScale = 1;
        audioManager.StopSFX();

        audioManager.PlaySFX(audioManager.Chose);
    }
}
