using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMasterSpawn : MonoBehaviour
{
    public GameObject dungeonMasterPrefab;
    public Transform spawnLocation;
    public MinionObject dungeonMasterBase;

    [Header("Debug & Communication")]
    public DungeonMasterLevel dungeonMasterObject;

    void Start()
    {
        InitializeDungeonMaster();
        SpawnDungeonMaster();
    }

    public void InitializeDungeonMaster()
    {
        dungeonMasterObject = new DungeonMasterLevel();

        dungeonMasterObject.minionBase = dungeonMasterBase;
        dungeonMasterObject.SetLevel(1);
    }

    public void SpawnDungeonMaster()
    {
        DungeonMaster minionScript = Instantiate(dungeonMasterPrefab, spawnLocation.position, spawnLocation.rotation).GetComponent<DungeonMaster>();
        minionScript.characterScriptableObject = dungeonMasterObject;
    }
}
