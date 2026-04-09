using UnityEngine;
using TMPro;

public class vatPhamUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpPotionText;
    [SerializeField] private TextMeshProUGUI manaPotionText;

    int hpPotion;
    int manaPotion;

    void Update()
    {
        hpPotion = PlayerPrefs.GetInt("PlayerHPotion", 0);
        manaPotion = PlayerPrefs.GetInt("PlayerMPotion", 0);

        hpPotionText.text = hpPotion.ToString();
        manaPotionText.text = manaPotion.ToString();
    }
}