using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerComponent : MonoBehaviour
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
