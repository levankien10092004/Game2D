using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinshPoin : MonoBehaviour
{
    private void Start()
    {
        UnlockNewLevel(); // unlock ngay khi vào màn
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCoins>()?.SaveCoins();
            SceneController.instance.NextLevel();
        }
    }

    void UnlockNewLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int unlocked = PlayerPrefs.GetInt("unlockedLevel", 1);

        if (current > unlocked)
        {
            PlayerPrefs.SetInt("unlockedLevel", current);
            PlayerPrefs.Save();
        }
    }
}
