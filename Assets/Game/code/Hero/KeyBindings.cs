using UnityEngine;

public class KeyBindings : MonoBehaviour
{
    public static KeyBindings Instance;

    [SerializeField] public KeyCode AttackKey = KeyCode.J;
    [SerializeField] public KeyCode JumpKey = KeyCode.K;
    [SerializeField] public KeyCode RollKey = KeyCode.L;
    [SerializeField] public KeyCode UseHPKey = KeyCode.U;
    [SerializeField] public KeyCode UseManaKey = KeyCode.I;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadKeys();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetKey(string action, KeyCode newKey)
    {
        switch (action)
        {
            case "Attack":
                AttackKey = newKey;
                break;

            case "Jump":
                JumpKey = newKey;
                break;

            case "Roll":
                RollKey = newKey;
                break;

            case "HP":
                UseHPKey = newKey;
                break;

            case "Mana":
                UseManaKey = newKey;
                break;
        }

        SaveKeys();
    }

    public void SaveKeys()
    {
        PlayerPrefs.SetString("AttackKey", AttackKey.ToString());
        PlayerPrefs.SetString("JumpKey", JumpKey.ToString());
        PlayerPrefs.SetString("RollKey", RollKey.ToString());
        PlayerPrefs.SetString("HPKey", UseHPKey.ToString());
        PlayerPrefs.SetString("ManaKey", UseManaKey.ToString());
    }

    void LoadKeys()
    {
        if (PlayerPrefs.HasKey("AttackKey"))
            AttackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("AttackKey"));

        if (PlayerPrefs.HasKey("JumpKey"))
            JumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpKey"));

        if (PlayerPrefs.HasKey("RollKey"))
            RollKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RollKey"));

        if (PlayerPrefs.HasKey("HPKey"))
            UseHPKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("HPKey"));

        if (PlayerPrefs.HasKey("ManaKey"))
            UseManaKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ManaKey"));
    }
    public bool GetKeyDown(string action)
    {
        switch (action)
        {
            case "Attack": return Input.GetKeyDown(AttackKey);
            case "Jump": return Input.GetKeyDown(JumpKey);
            case "Roll": return Input.GetKeyDown(RollKey);
            case "HP": return Input.GetKeyDown(UseHPKey);
            case "Mana": return Input.GetKeyDown(UseManaKey);
        }
        return false;
    }
}