using UnityEngine;
using TMPro;
using static ChestController;

public class ChestManager : MonoBehaviour
{
    public static ChestManager instance;

    [Header("UI")]
    public TextMeshProUGUI chest1Text;
    public TextMeshProUGUI chest2Text;

    [Header("Tổng số rương trong map")]
    public int totalChest1;
    public int totalChest2;

    private int openedChest1 = 0;
    private int openedChest2 = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    public void OpenChest(ChestType type)
    {
        switch (type)
        {
            case ChestType.Chest1:
                openedChest1++;
                break;

            case ChestType.Chest2:
                openedChest2++;
                break;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        chest1Text.text = $"{openedChest1}/{totalChest1}";
        chest2Text.text = $" {openedChest2}/{totalChest2}";
    }
    public int GetStarResult()
    {
        if (totalChest1 == 0 && totalChest2 == 0)
            return 3;

        float p1 = totalChest1 > 0 ? (float)openedChest1 / totalChest1 : 1f;
        float p2 = totalChest2 > 0 ? (float)openedChest2 / totalChest2 : 1f;

        float totalPercent = p1 * 0.5f + p2 * 0.5f;

        if (totalPercent >= 1f) return 3;
        if (totalPercent >= 0.7f) return 2;
        if (totalPercent >= 0.4f) return 1;
        return 0;
    }
}
