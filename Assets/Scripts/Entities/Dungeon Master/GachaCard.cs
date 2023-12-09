using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaCard : MonoBehaviour
{
    public TMP_Text cardName;
    public Image imageSprite;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text soulText;

    public MinionObject referencedMinion;

    public void InitCard()
    {
        cardName.text = referencedMinion.name;
        imageSprite.sprite = referencedMinion.gachaCard;
        healthText.text = referencedMinion.health.ToString();
        damageText.text = referencedMinion.atkDmg.ToString();
        soulText.text = referencedMinion.soulCost.ToString();
    }

    public void SelectCard()
    {
        DungeonMasterObject.Instance.AddMinionCard(referencedMinion);
        
        if (DayTracker.Instance.GetDay() == 1)
            DayTime.Instance.StartDay();
    }
}
