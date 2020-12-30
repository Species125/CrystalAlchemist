using UnityEngine;
using Sirenix.OdinInspector;
using Mirror;

public class CrystalNetworkManager : NetworkManager
{
    [ButtonGroup("Test")]
    public void Insert()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Skills");
        foreach(GameObject prefab in prefabs)
        {
            //ClientScene.UnregisterPrefab(prefab);
            //ClientScene.RegisterPrefab(prefab);
        }
    }
}
