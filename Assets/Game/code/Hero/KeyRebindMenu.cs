using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class KeyRebindMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI jumpText;
    [SerializeField] private TextMeshProUGUI rollText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI manaText;

    // phím tạm thời
    private KeyCode newAttack;
    private KeyCode newJump;
    private KeyCode newRoll;
    private KeyCode newHP;
    private KeyCode newMana;

    [SerializeField] private GameObject Errortext;

    private Coroutine blinkCoroutine;

    void Start()
    {
        // lấy phím hiện tại
        newAttack = KeyBindings.Instance.AttackKey;
        newJump = KeyBindings.Instance.JumpKey;
        newRoll = KeyBindings.Instance.RollKey;
        newHP = KeyBindings.Instance.UseHPKey;
        newMana = KeyBindings.Instance.UseManaKey;

        UpdateUI();
    }

    public void RebindAttack() => StartCoroutine(RebindKey("Attack", attackText));
    public void RebindJump() => StartCoroutine(RebindKey("Jump", jumpText));
    public void RebindRoll() => StartCoroutine(RebindKey("Roll", rollText));
    public void RebindHP() => StartCoroutine(RebindKey("HP", hpText));
    public void RebindMana() => StartCoroutine(RebindKey("Mana", manaText));

    IEnumerator RebindKey(string action, TextMeshProUGUI targetText)
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkText(targetText));

        while (!Input.anyKeyDown)
            yield return null;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                if (IsKeyUsed(key, action))
                {
                    StartCoroutine(Showerror());
                    StopCoroutine(blinkCoroutine);
                    targetText.alpha = 1f;
                    yield break;
                }

                switch (action)
                {
                    case "Attack": newAttack = key; break;
                    case "Jump": newJump = key; break;
                    case "Roll": newRoll = key; break;
                    case "HP": newHP = key; break;
                    case "Mana": newMana = key; break;
                }
                break; 
            }
        }
        StopCoroutine(blinkCoroutine);
        targetText.alpha = 1f;
        UpdateUI();
    }
    bool IsKeyUsed(KeyCode key, string currentAction)
    {
        if (currentAction != "Attack" && newAttack == key) return true;
        if (currentAction != "Jump" && newJump == key) return true;
        if (currentAction != "Roll" && newRoll == key) return true;
        if (currentAction != "HP" && newHP == key) return true;
        if (currentAction != "Mana" && newMana == key) return true;

        return false;
    }

    // nút LƯU
    public void SaveKeys()
    {
        KeyBindings.Instance.SetKey("Attack", newAttack);
        KeyBindings.Instance.SetKey("Jump", newJump);
        KeyBindings.Instance.SetKey("Roll", newRoll);
        KeyBindings.Instance.SetKey("HP", newHP);
        KeyBindings.Instance.SetKey("Mana", newMana);

        KeyBindings.Instance.SaveKeys();
    }

    // nút ĐẶT LẠI MẶC ĐỊNH
    public void ResetDefault()
    {
        newAttack = KeyCode.J;
        newJump = KeyCode.K;
        newRoll = KeyCode.L;
        newHP = KeyCode.U;
        newMana = KeyCode.I;

        UpdateUI();
    }

    void UpdateUI()
    {
        attackText.text = newAttack.ToString();
        jumpText.text = newJump.ToString();
        rollText.text = newRoll.ToString();
        hpText.text = newHP.ToString();
        manaText.text = newMana.ToString();
    }
    IEnumerator Showerror()
    {
        Errortext.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Errortext.gameObject.SetActive(false);
    }
    IEnumerator BlinkText(TextMeshProUGUI text)
    {
        while (true)
        {
            text.alpha = 0.2f;
            yield return new WaitForSeconds(0.3f);

            text.alpha = 1f;
            yield return new WaitForSeconds(0.3f);
        }
    }
}