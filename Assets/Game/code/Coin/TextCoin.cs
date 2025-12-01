using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCoin : MonoBehaviour
{
    public static TextCoin Instance;

    public TextMeshProUGUI coinText; // kéo text vào đây

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateCoinText(int coins)
    {
        coinText.text =  coins.ToString();
    }
}
