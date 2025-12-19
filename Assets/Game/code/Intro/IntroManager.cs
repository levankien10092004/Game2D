using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    void Start()
    {
        // Nếu đã xem intro rồi → vào menu
        if (PlayerPrefs.GetInt("IntroPlayed", 0) == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
        // Nếu chưa → ở lại scene Intro
    }

    // Gọi khi intro kết thúc
    public void IntroFinished()
    {
        PlayerPrefs.SetInt("IntroPlayed", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("MainMenu");
    }
}
