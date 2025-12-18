using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public void Show(string name, string text)
    {
        panel.SetActive(true);
        nameText.text = name;
        dialogueText.text = text;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
