using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingCutscene : MonoBehaviour
{
    [Header("Characters")]
    public Transform hero;
    public Transform princess;

    [Header("UI & Fade")]
    public EndingDialogueManager dialogue;
    public TextMeshProUGUI narrationText; // Text hiển thị thông báo tự chạy
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float narrationSpeed = 0.05f; // tốc độ chữ tự chạy

    [Header("Positions")]
    public Transform meetPosition; // Vị trí hero và princess gặp nhau

    void Start()
    {
        // Tắt DialoguePanel và narrationText ban đầu
        if (dialogue != null && dialogue.panel != null)
            dialogue.panel.SetActive(false);
        if (narrationText != null)
            narrationText.gameObject.SetActive(false);

        StartCoroutine(PlayEnding());
    }

    IEnumerator PlayEnding()
    {
        // 1️⃣ Fade từ đen sang cảnh
        yield return Fade(1f, 0f);

        // 2️⃣ Thông báo tự chạy sau khi boss bị đánh bại
        string narration = "Mối hiểm họa đã không còn.\nPháp lực của phù thủy đã tan biến.\nXiềng xích trói buộc công chúa đã biến mất...";
        yield return ShowNarration(narration);

        // 3️⃣ Di chuyển hero & princess về vị trí gặp nhau
        yield return MoveCharacters(meetPosition.position, 2f);

        // 4️⃣ Dòng thoại ending
        yield return dialogue.ShowLine("Princess", "Ta… ta cứ nghĩ mình sẽ không thoát được nữa. Cảm ơn chàng… thật lòng.");
        yield return dialogue.ShowLine("HeroKnight", "Không có gì, nàng an toàn là ta vui rồi.");
        yield return dialogue.ShowLine("Princess", "Chúng ta sẽ sống hạnh phúc bên nhau chứ?");
        yield return dialogue.ShowLine("HeroKnight", "Ta hứa, mối hiểm họa đã qua, từ nay sẽ chỉ có chúng ta.");
        yield return dialogue.ShowLine("Narrator", "Cuộc sống đã trở lại yên bình, công chúa và HeroKnight sống hạnh phúc bên nhau…");

        // 5️⃣ Fade out
        yield return Fade(0f, 1f);

        // 6️⃣ Load MainMenu hoặc scene khác
        SceneManager.LoadScene("MainMenu");
    }

    // Di chuyển 2 nhân vật về vị trí gặp nhau
    IEnumerator MoveCharacters(Vector3 targetPos, float duration)
    {
        Vector3 heroStart = hero.position;
        Vector3 princessStart = princess.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            hero.position = Vector3.Lerp(heroStart, targetPos + new Vector3(-1f, 0, 0), t);
            princess.position = Vector3.Lerp(princessStart, targetPos + new Vector3(1f, 0, 0), t);
            yield return null;
        }
    }

    // Fade hình ảnh
    IEnumerator Fade(float from, float to)
    {
        if (fadeImage == null)
            yield break;

        float t = 0f;
        Color color = fadeImage.color;
        color.a = from;
        fadeImage.color = color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = to;
        fadeImage.color = color;
    }

    // Hiển thị narration tự chạy chữ
    IEnumerator ShowNarration(string text)
    {
        if (narrationText == null)
            yield break;

        narrationText.gameObject.SetActive(true);
        narrationText.text = "";

        foreach (char c in text)
        {
            narrationText.text += c;
            yield return new WaitForSeconds(narrationSpeed);
        }

        // Chờ click chuột để tiếp tục
        bool clicked = false;
        while (!clicked)
        {
            if (Input.GetMouseButtonDown(0))
                clicked = true;
            yield return null;
        }

        narrationText.gameObject.SetActive(false);
    }
}
