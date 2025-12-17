using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeverMenu : MonoBehaviour
{
    public Button[] buttons;

    AudioManager audioManager;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);

        // Tắt tất cả button
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        // Bật đúng số lượng level đã mở
        for (int i = 0; i < unlockedLevel && i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OpenLever(int levelId)
    {
        string leverName = "LV" + levelId;
        SceneManager.LoadScene(leverName);
        audioManager.PlaySFX(audioManager.Chose);
    }
}
