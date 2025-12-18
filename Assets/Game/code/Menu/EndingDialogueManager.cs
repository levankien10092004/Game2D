using UnityEngine;
using TMPro;
using System.Collections;

public class EndingDialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;                  // Panel chứa text
    public TextMeshProUGUI nameText;          // Text hiển thị tên nhân vật
    public TextMeshProUGUI dialogueText;      // Text hiển thị lời thoại

    private bool clicked = false;

    void Update()
    {
        // Nhận click chuột trái để chuyển câu thoại
        if (Input.GetMouseButtonDown(0) && panel.activeSelf)
        {
            clicked = true;
        }
    }

    /// <summary>
    /// Hiển thị một dòng thoại và chờ người chơi click mới kết thúc
    /// </summary>
    /// <param name="name">Tên nhân vật</param>
    /// <param name="text">Lời thoại</param>
    public IEnumerator ShowLine(string name, string text)
    {
        if (panel != null)
            panel.SetActive(true);

        if (nameText != null)
            nameText.text = name;

        if (dialogueText != null)
            dialogueText.text = text;

        clicked = false;

        // Chờ người chơi click
        while (!clicked)
            yield return null;

        if (panel != null)
            panel.SetActive(false);
    }
}
