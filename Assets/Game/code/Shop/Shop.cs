using UnityEngine;
using TMPro;
using System.Collections;

public class shop : MonoBehaviour
{
    [Header("Text Hi?n th?")]
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Potion UI")]
    [SerializeField] private TextMeshProUGUI hpPotionText;
    [SerializeField] private TextMeshProUGUI manaPotionText;

    [Header("chi so hien tai")]

    private int hpPotion;
    private int manaPotion;


    [Header("Giá potion")]
    [SerializeField] private int hmPotionCostx1 = 10;
    [SerializeField] private int hmPotionCostx5 = 40;

    [SerializeField] private TextMeshProUGUI minusCoinText;
    [SerializeField] private TextMeshProUGUI hpCoinText;
    [SerializeField] private TextMeshProUGUI manaCoinText;

    [SerializeField] private GameObject ErrorText;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        LoadStats();
        UpdateUI();
    }

    void LoadStats()
    {
        hpPotion = PlayerPrefs.GetInt("PlayerHPotion", 0);
        manaPotion = PlayerPrefs.GetInt("PlayerMPotion", 0);

        minusCoinText.gameObject.SetActive(false);

        hpCoinText.gameObject.SetActive(false);

        manaCoinText.gameObject.SetActive(false);
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("PlayerHPotion", hpPotion);
        PlayerPrefs.SetInt("PlayerMPotion", manaPotion);

        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        coinText.text = PlayerCoins.Instance.coins.ToString();
        hpPotionText.text = hpPotion.ToString();
        manaPotionText.text = manaPotion.ToString();
    }

  
    public void BuyHPPotionx1()
    {
        if (PlayerCoins.Instance.SpendCoins(hmPotionCostx1))
        {
            hpPotion += 1;

            StartCoroutine(ShowMinusCoinHP(hmPotionCostx1, 1));

            SaveStats();
            UpdateUI();
        }
        else
        {
            StartCoroutine(Showerror());
        }

        audioManager.PlaySFX(audioManager.Chose);
    }
    public void BuyHPPotionx5()
    {
        if ((PlayerCoins.Instance.SpendCoins(hmPotionCostx5)))
        {
            hpPotion += 5;

            StartCoroutine(ShowMinusCoinHP(hmPotionCostx5, 5));

            SaveStats();
            UpdateUI();
        }
        else
        {
            StartCoroutine(Showerror());
        }

        audioManager.PlaySFX(audioManager.Chose);
    }

    public void BuyManaPotionx1()
    {
        if ((PlayerCoins.Instance.SpendCoins(hmPotionCostx1)))
        {
            manaPotion += 1;

            StartCoroutine(ShowMinusCoinMana(hmPotionCostx1, 1));

            SaveStats();
            UpdateUI();
        }
        else
        {
            StartCoroutine(Showerror());
        }

        audioManager.PlaySFX(audioManager.Chose);
    }
    public void BuyManaPotionx5()
    {
        if ((PlayerCoins.Instance.SpendCoins(hmPotionCostx5)))
        {

            manaPotion += 5;

            StartCoroutine(ShowMinusCoinMana(hmPotionCostx5, 5));

            SaveStats();
            UpdateUI();
        }
        else
        {
            StartCoroutine(Showerror());
        }

        audioManager.PlaySFX(audioManager.Chose);
    }

    // ======================= HI?U ?NG =======================

    IEnumerator ShowMinusCoinHP(int amount, int health)
    {
        minusCoinText.text = "-" + amount.ToString();
        hpCoinText.text = "+" + health.ToString();

        minusCoinText.gameObject.SetActive(true);
        hpCoinText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        minusCoinText.gameObject.SetActive(false);
        hpCoinText.gameObject.SetActive(false);
    }
    IEnumerator ShowMinusCoinMana(int amount, int health)
    {
        minusCoinText.text = "-" + amount.ToString();
        manaCoinText.text = "+" + health.ToString();

        minusCoinText.gameObject.SetActive(true);
        manaCoinText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        minusCoinText.gameObject.SetActive(false);
        manaCoinText.gameObject.SetActive(false);
    }


    IEnumerator Showerror()
    {
        ErrorText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        ErrorText.gameObject.SetActive(false);
    }
}