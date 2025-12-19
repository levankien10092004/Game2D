using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextEnd : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Buttons")]
    public GameObject agreeButton;
    public GameObject refuseButton;

    [Header("Dialogue")]
    [TextArea(3, 6)]
    public string[] dialogues;

    [Header("Speed")]
    public float typingSpeed = 0.05f;
    public float delayBetweenLines = 1.0f;

    private int index = 0;

    void OnEnable()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);

        agreeButton.SetActive(false);
        refuseButton.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        dialogueText.text = "";

        foreach (char c in dialogues[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // 👉 Nếu là câu cuối
        if (index == dialogues.Length - 1)
        {
            ShowChoiceButtons();
        }
        else
        {
            yield return new WaitForSeconds(delayBetweenLines);
            index++;
            StartCoroutine(TypeText());
        }
    }

    void ShowChoiceButtons()
    {
        agreeButton.SetActive(true);
        refuseButton.SetActive(true);
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        agreeButton.SetActive(false);
        refuseButton.SetActive(false);
    }
    public void OnAgree()
    {
        SceneManager.LoadScene("IntroBadEnding"); // đổi đúng tên scene của bạn
    }
}
