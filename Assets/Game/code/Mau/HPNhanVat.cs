using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPNhanVat : ThanhMau
{
    [SerializeField] private TextMeshProUGUI textMau;
    public override void capNhatMau(float mauht, float mautd)
    {
        base.capNhatMau(mauht, mautd);
        mau.fillAmount =(float) mauht / mautd;
        textMau.text = mauht.ToString()+"/" +mautd.ToString();
    }
}
