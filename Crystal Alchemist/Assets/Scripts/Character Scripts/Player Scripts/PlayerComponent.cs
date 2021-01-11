using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Player))]
public class PlayerComponent : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public Player player;

    public virtual void Initialize()
    {        
        this.player = this.GetComponent<Player>();
    }

    public virtual void Updating()
    {
    }
}
