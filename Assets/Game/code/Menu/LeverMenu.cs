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
        // CHỈ CHO PHÉP LevelMenu chạy trong Menu (scene 0)
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Destroy(gameObject);
            return;
        }

        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = false;

        for (int i = 0; i < unlockedLevel && i < buttons.Length; i++)
            buttons[i].interactable = true;

        audioManager = GameObject.FindGameObjectWithTag("Audio")
            .GetComponent<AudioManager>();
    }


    //public void OpenLever(int levelId)
    //{
    //    string leverName = "LV" + levelId;
    //    SceneManager.LoadScene(leverName);
    //    audioManager.PlaySFX(audioManager.Chose);
    //}
    public void OpenLever(int levelId)
    {

        audioManager.PlaySFX(audioManager.Chose);

        // Nếu là LV1 và CHƯA xem Intro
        if (levelId == 1 && PlayerPrefs.GetInt("HasSeenIntro", 0) == 0)
        {
            SceneManager.LoadScene("IntroScene");
            return;
        }

        // Các level còn lại hoặc đã xem Intro
        string levelName = "LV" + levelId;
        SceneManager.LoadScene(levelName);


        audioManager.PlaySFX(audioManager.Chose);
        string leverName = "LV" + levelId;
        SceneManager.LoadScene(leverName);
       
        

    }

}
