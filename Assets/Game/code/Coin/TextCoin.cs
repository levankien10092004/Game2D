using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCoin : MonoBehaviour
{
    public static TextCoin Instance;

    [SerializeField] public TextMeshProUGUI coinText; 

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateCoinText(int coins)
    {
        coinText.text =  coins.ToString();
    }
}
