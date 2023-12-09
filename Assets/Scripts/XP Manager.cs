using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;
    [SerializeField] Image XPBar;
    [SerializeField] TMP_Text levelText, progressLevel;
    DungeonMasterObject masterLevel;

    void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        masterLevel = DungeonMasterObject.Instance;
        UpdateXP();
    }

    public void UpdateXP()
    {
        float progress = masterLevel.currentExp/masterLevel.expToNextLvl;
        XPBar.fillAmount = progress;
        levelText.text = $"Level\n<size=22>{masterLevel.level}";
        progressLevel.text = $"{Mathf.Floor(progress*100)}%";
    }

    public void AddXP(float value){
        masterLevel.AddExp(value);
        UpdateXP();
    }
}
