using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPEnemy : ThanhMau
{
    public override void capNhatMau(float mauht, float mautd)
    {
        base.capNhatMau(mauht, mautd);
        mau.fillAmount =(float) mauht / mautd;
    }
}
