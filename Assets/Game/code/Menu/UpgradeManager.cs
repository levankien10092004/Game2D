using UnityEngine;
using TMPro;
using System.Collections;
public class UpgradeManager : MonoBehaviour
{
    [Header("Text Hiển thị")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI healthText;

    [Header("Chỉ số hiện tại")]
    private int damage;
    private int health;
    private int coins;
     
    [Header("Giá nâng cấp")]
    public int damageCost = 5;
    public int healthCost = 5;
    public TextMeshProUGUI minusCoinText;
    public TextMeshProUGUI minusHpText;
    public TextMeshProUGUI minusATText;
    public GameObject ErrorText;

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
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        damageCost = PlayerPrefs.GetInt("DamageCost", damageCost);
        healthCost = PlayerPrefs.GetInt("HealthCost", healthCost);

        minusATText.gameObject.SetActive(false);
        minusCoinText.gameObject.SetActive(false);
        minusHpText.gameObject.SetActive(false);
    }

    void SaveStats()
    {
        PlayerPrefs.SetInt("PlayerDamage", damage); 
        PlayerPrefs.SetInt("PlayerHealth", health);
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.SetInt("DamageCost", damageCost);
        PlayerPrefs.SetInt("HealthCost", healthCost); 

        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        coinText.text = coins.ToString();
        damageText.text = damage.ToString();
        healthText.text = health.ToString();
    }

    // ======================= NÂNG CẤP =======================

    public void UpgradeDamage()
    {
        if (coins >= damageCost)
        {
            coins -= damageCost;
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
        if (coins >= healthCost)
        {
            coins -= healthCost;
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
    IEnumerator Showerror()
    {
    
        ErrorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);     
        ErrorText.gameObject.SetActive(false);
    }
} 
