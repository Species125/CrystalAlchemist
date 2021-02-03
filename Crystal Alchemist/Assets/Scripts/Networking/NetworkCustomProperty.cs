using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCustomProperty : MonoBehaviour
{

    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();

    public void SetPassword()
    {
        _customProperties["Password"] = "Test";
        
    }
}
