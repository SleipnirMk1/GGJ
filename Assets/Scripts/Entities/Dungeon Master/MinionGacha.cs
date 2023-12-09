using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGacha : MonoBehaviour
{
    public List<MinionObject> minionRewards = new List<MinionObject>();
    public GameObject gachaPanel;

    public GachaCard option1;
    public GachaCard option2;
    public GachaCard option3;

    public List<MinionObject> GetRandomMinions(int amount)
    {
        List<MinionObject> retList = new List<MinionObject>();
        List<MinionObject> unOwnedMinions = FilterOwned(DungeonMasterObject.Instance.ownedMinions);

        if (unOwnedMinions.Count < amount)
        {
            unOwnedMinions = retList;
        }

        List<int> selectedIdx = new List<int>();

        for (int i = 0; i < amount; ++i)
        {
            int randIdx = Random.Range(0, unOwnedMinions.Count-1);
            while (selectedIdx.Contains(randIdx))
            {
                randIdx = Random.Range(0, unOwnedMinions.Count-1);
            }
            selectedIdx.Add(randIdx);
            retList.Add(unOwnedMinions[randIdx]);
        }

        return retList;
    }

    List<MinionObject> FilterOwned(List<MinionObject> inputList)
    {
        List<MinionObject> retList = new List<MinionObject>();

        foreach(MinionObject c in minionRewards)
        {
            if (!inputList.Contains(c))
            {
                retList.Add(c);
            }
        }

        return retList;
    }

    public void InitializeGacha()
    {
        gachaPanel.SetActive(true);

        List<MinionObject> choices = GetRandomMinions(3);

        option1.referencedMinion = choices[0];
        option2.referencedMinion = choices[1];
        option3.referencedMinion = choices[2];

        option1.InitCard();
        option2.InitCard();
        option3.InitCard();
    }
}
