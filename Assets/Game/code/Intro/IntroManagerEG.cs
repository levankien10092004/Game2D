using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManagerEG : MonoBehaviour
{
    [Header("Scene chuyển sau khi video kết thúc")]
    public string mainMenuScene = "MainMenu";

    // Gọi khi video kết thúc hoặc khi skip
    public void IntroFinished()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}

