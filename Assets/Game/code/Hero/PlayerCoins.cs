using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    public static PlayerCoins Instance;

    [SerializeField] public int coins = 0;
    private int coinsAtLevelStart = 0;  // coin ban đầu khi vào màn

    private void Start()
    {
        // Load coin thật đã lưu từ trước
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);

        // Lưu số coin ban đầu để nếu không hoàn thành sẽ reset lại
        coinsAtLevelStart = coins;

        TextCoin.Instance.UpdateCoinText(coins);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        TextCoin.Instance.UpdateCoinText(coins);
    }

    // Gọi khi hoàn thành màn để lưu coin thật
    public void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.Save();
    }
    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveCoins();
            TextCoin.Instance.UpdateCoinText(coins);
            return true;
        }
        return false;
    }


    // Gọi khi chết/thoát/reset → trả coin về ban đầu
    public void RestoreCoins()
    {
        coins = coinsAtLevelStart;
        TextCoin.Instance.UpdateCoinText(coins);
    }
}
