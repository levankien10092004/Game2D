using UnityEngine;
using TMPro;
using System.Collections;
public class UpgradeManager : MonoBehaviour
{
    [Header("Text Hiển thị")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;

    [Header("Chỉ số hiện tại")]
    private int damage;
    private int health;
    private int mana;

    [Header("Giá nâng cấp")]
    private int damageCost = 5;
    private int healthCost = 5;
    private int manacost = 5;
    [SerializeField] private TextMeshProUGUI minusCoinText;
    [SerializeField] private TextMeshProUGUI minusHpText;
    [SerializeField] private TextMeshProUGUI minusATText;
    [SerializeField] private TextMeshProUGUI minusMNText;
    [SerializeField] private GameObject ErrorText;

    AudioManager AudioManager;
    private void Start()
    {
        LoadStats();
        UpdateUI();
      
    }
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void LoadStats()
    {
        damage = PlayerPrefs.GetInt("PlayerDamage", 10);
        health = PlayerPrefs.GetInt("PlayerHealth", 100);
        mana = PlayerPrefs.GetInt("PlayerMana", 50);
        damageCost = PlayerPrefs.GetInt("DamageCost", damageCost);
        healthCost = PlayerPrefs.GetInt("HealthCost", healthCost);

        minusATText.gameObject.SetActive(false);
        minusCoinText.gameObject.SetActive(false);
        minusHpText.gameObject.SetActive(false);
        minusMNText.gameObject.SetActive(false);
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("PlayerDamage", damage); 
        PlayerPrefs.SetInt("PlayerHealth", health);
        PlayerPrefs.SetInt("PlayerMana", mana);
        PlayerPrefs.SetInt("DamageCost", damageCost);
        PlayerPrefs.SetInt("HealthCost", healthCost);
        PlayerPrefs.SetInt("ManaCost", manacost);

        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        coinText.text = PlayerCoins.Instance.coins.ToString();
        damageText.text = damage.ToString();
        healthText.text = health.ToString();
        manaText.text = mana.ToString();
    }

    // ======================= NÂNG CẤP =======================

    public void UpgradeDamage()
    {
        if (PlayerCoins.Instance.SpendCoins(damageCost))
        {
            damage += 5;

            StartCoroutine(ShowMinusCoinAT(damageCost,5));
            SaveStats();
            UpdateUI();
            damageCost += 5;
        }
        else
        {
            StartCoroutine(Showerror());
        }
        audioManager.PlaySFX(audioManager.Chose);
    }

    public void UpgradeHealth()
    {
        if (PlayerCoins.Instance.SpendCoins(healthCost))
        {
            health += 10;

            StartCoroutine(ShowMinusCoinHP(healthCost,10));
            SaveStats();
            UpdateUI();
            healthCost += 5;
        }
        else {
            StartCoroutine(Showerror());
        }
        audioManager.PlaySFX(audioManager.Chose);
    }
    public void UpgradeMana()
    {
        if (PlayerCoins.Instance.SpendCoins(manacost))
        {
            mana += 10;

            StartCoroutine(ShowMinusCoinMN(manacost, 10));
            SaveStats();
            UpdateUI();
            manacost += 5;
        }
        else
        {
            StartCoroutine(Showerror());
        }
        audioManager.PlaySFX(audioManager.Chose);
    }
    IEnumerator ShowMinusCoinHP(int amount,int health)
    {
        minusCoinText.text = "-" + amount.ToString();
        minusHpText.text = "+" + health.ToString();
        minusCoinText.gameObject.SetActive(true);
        minusHpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        minusCoinText.gameObject.SetActive(false);
        minusHpText.gameObject.SetActive(false);
    }
    IEnumerator ShowMinusCoinAT(int amount, int Attack)
    {
        minusCoinText.text = "-" + amount.ToString();
        minusATText.text = "+" + Attack.ToString(); 
        minusCoinText.gameObject.SetActive(true);
        minusATText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        minusCoinText.gameObject.SetActive(false);
        minusATText.gameObject.SetActive(false);
    }
    IEnumerator ShowMinusCoinMN(int amount, int mn)
    {
        minusCoinText.text = "-" + amount.ToString();
        minusMNText.text = "+" + mn.ToString();
        minusCoinText.gameObject.SetActive(true);
        minusMNText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        minusCoinText.gameObject.SetActive(false);
        minusMNText.gameObject.SetActive(false);
    }
    IEnumerator Showerror()
    {
    
        ErrorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);     
        ErrorText.gameObject.SetActive(false);
    }
} 
