using UnityEngine;
using TMPro;

public class CoinMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Update()
    {
        UpdateCoin();
    }
   
    void UpdateCoin()
    {
        coinText.text = PlayerCoins.Instance.coins.ToString();
    }
}