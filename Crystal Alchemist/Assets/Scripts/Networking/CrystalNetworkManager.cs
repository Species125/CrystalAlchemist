using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Sirenix.OdinInspector;

public class CrystalNetworkManager : NetworkManager
{
    [ButtonGroup("Test")]
    public void Insert()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Skills");
        foreach(GameObject prefab in prefabs)
        {
            ClientScene.UnregisterPrefab(prefab);
            ClientScene.RegisterPrefab(prefab);
        }
    }
}
