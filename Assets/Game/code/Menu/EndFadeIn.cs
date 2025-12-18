using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndFadeIn : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeImage CHƯA được gán trong Inspector!");
            return;
        }

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }
}
