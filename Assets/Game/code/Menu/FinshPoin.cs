using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinshPoin : MonoBehaviour
{
    public GameObject victory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCoins>()?.SaveCoins();
            victory.gameObject.SetActive(true);
            Time.timeScale = 0f;
          
        }
    }
    public void OnNextLevelButton()
    {
        Time.timeScale = 1f;
        UnlockNewLevel();
        SceneController.instance.NextLevel();
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
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
