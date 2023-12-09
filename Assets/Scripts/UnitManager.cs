using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] MinionObject[] minionObjects;
    [SerializeField] MinionObject selectedMinion;
    Vector2 unitSummonPos;
    [SerializeField] GameObject minionPrefab;
    [SerializeField] TMP_Text unitLimitText;
    [SerializeField] int unitLimit, curUnitWeight;

    [SerializeField] float summonDeadZone;

    void OnEnable()
    {
        buttons[0].onClick.AddListener(Minion1);
        buttons[1].onClick.AddListener(Minion2);
        buttons[2].onClick.AddListener(Minion3);
        buttons[3].onClick.AddListener(Minion4);
        buttons[4].onClick.AddListener(Minion5);
    }

    void OnDisable()
    {
        buttons[0].onClick.RemoveListener(Minion1);
        buttons[1].onClick.RemoveListener(Minion2);
        buttons[2].onClick.RemoveListener(Minion3);
        buttons[3].onClick.RemoveListener(Minion4);
        buttons[4].onClick.RemoveListener(Minion5);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateUnitCount();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = minionObjects[i].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedMinion != null)
        {
            if (SoulManager.Instance.soulCount >= selectedMinion.soulCost && unitLimit >= selectedMinion.weight + curUnitWeight){
                if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    unitSummonPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (!CheckSummonDeadzone(unitSummonPos))
                    {
                        GameObject newMinion = Instantiate(minionPrefab, unitSummonPos, quaternion.identity);
                        newMinion.GetComponent<Minion>().characterScriptableObject = selectedMinion;
                        
                        SoulManager.Instance.ReduceSoul((int)selectedMinion.soulCost);
                        curUnitWeight += selectedMinion.weight;
                        UpdateUnitCount();
                    }
                }
            }
        }
    }

    void UpdateUnitCount(){
        unitLimitText.text = $"{curUnitWeight}/{unitLimit}";
    }

    bool CheckSummonDeadzone(Vector2 center)
    {
        var colliders = Physics2D.OverlapCircleAll(center, summonDeadZone);
        foreach(Collider2D c in colliders)
        {
            if(c.gameObject.GetComponent<Hero>() != null)
                return true;
        }

        return false;
    }

    void Minion1()
    {
        selectedMinion = minionObjects[0];
    }
    void Minion2()
    {
        selectedMinion = minionObjects[1];
    }
    void Minion3()
    {
        selectedMinion = minionObjects[2];
    }
    void Minion4()
    {
        selectedMinion = minionObjects[3];
    }
    void Minion5()
    {
        selectedMinion = minionObjects[4];
    }
}
