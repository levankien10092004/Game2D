using UnityEngine;

public class PlayerItems : MonoBehaviour
{
     private int hpPotion;
     private int manaPotion;

    void Start()
    {
        LoadItems();
    }

    public void LoadItems()
    {
        hpPotion = PlayerPrefs.GetInt("PlayerHPotion", 0);
        manaPotion = PlayerPrefs.GetInt("PlayerMPotion", 0);
    }

    public void SaveItems()
    {
        PlayerPrefs.SetInt("PlayerHPotion", hpPotion);
        PlayerPrefs.SetInt("PlayerMPotion", manaPotion);
        PlayerPrefs.Save();
    }

    public bool UseHPPotion()
    {
        if (hpPotion > 0)
        {
            hpPotion--;
            SaveItems();
            return true;
        }
        return false;
    }

    public bool UseManaPotion()
    {
        if (manaPotion > 0)
        {
            manaPotion--;
            SaveItems();
            return true;
        }
        return false;
    }
}