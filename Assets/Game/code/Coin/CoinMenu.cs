using UnityEngine;
using TMPro;

public class CoinMenu : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private void Update()
    {
        UpdateCoin();
    }
   
    void UpdateCoin()
    {
        int coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        coinText.text = coins.ToString();
    }
}