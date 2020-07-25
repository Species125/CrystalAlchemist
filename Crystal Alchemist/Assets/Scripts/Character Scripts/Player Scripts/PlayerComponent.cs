using Mirror;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerComponent : NetworkBehaviour
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
