using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public string nextSceneName;

    void Start()
    {
        StartCoroutine(FadeIn()); // CHỈ fade in
    }

    public void FadeToNextScene()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeIn()
    {
        yield return Fade(1, 0);
    }

    IEnumerator FadeOutAndLoad()
    {
        yield return Fade(0, 1);
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        fadeImage.color = new Color(0, 0, 0, from);

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, to);
    }
}
