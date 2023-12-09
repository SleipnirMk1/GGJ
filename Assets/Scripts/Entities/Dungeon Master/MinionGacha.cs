using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGacha : MonoBehaviour
{
    public List<MinionObject> minionRewards = new List<MinionObject>();

    public List<MinionObject> GetRandomMinions(int amount)
    {
        List<MinionObject> retList = new List<MinionObject>();
        List<MinionObject> unOwnedMinions = FilterOwned(DungeonMasterObject.Instance.ownedMinions);

        for (int i = 0; i < amount; ++i)
        {
            int randIdx = Random.Range(0, unOwnedMinions.Count-1);
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
}
