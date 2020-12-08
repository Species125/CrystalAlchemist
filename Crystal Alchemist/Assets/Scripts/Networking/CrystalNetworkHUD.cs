using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Mirror;

[DisallowMultipleComponent]
[AddComponentMenu("Network/NetworkManagerHUD")]
//[RequireComponent(typeof(NetworkManager))]
public class CrystalNetworkHUD : MonoBehaviour
{
    /*
    private NetworkManager manager;

    //You can also change scenes while the game is active by calling ServerChangeScene. 
    //This makes all the currently connected clients change Scene too, and updates networkSceneName 
    //so that new clients also load the new Scene.

    private void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    private void Start()
    {
        GameEvents.current.OnOnlineSceneChanged += ChangeScene;
        if (!NetworkClient.active) manager.StartHost();        
        else NetworkServer.SpawnObjects();
    }

    private void OnDestroy()
    {
        GameEvents.current.OnOnlineSceneChanged -= ChangeScene;
    }

    private void ChangeScene(string test)
    {
        manager.ServerChangeScene(test);
    }*/
}
