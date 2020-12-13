using AssetIcons;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/Item Drop")]
public class ItemDrop : ScriptableObject
{
    [SerializeField]
    public float duration = 60f;

    [SerializeField]
    public ItemStats stats;

    [SerializeField]
    public Collectable collectable;

    [AssetIcon]
    private Sprite GetSprite()
    {
        if(this.stats != null) return stats.getSprite();
        return null;
    }

    public ItemDrop Instantiate(int amount)
    {
        ItemDrop clone = Instantiate(this);
        clone.name = this.name;
        clone.Initialize(amount); //Set correct stats name for unique items
        return clone;
    }

    public void Initialize(int amount)
    {
        ItemStats temp = Instantiate(this.stats);
        temp.name = this.name;
        temp.Initialize(amount);
        this.stats = temp;
    }

    public Collectable Instantiate(Vector2 position)
    {
        return Instantiate(position, false, Vector2.zero);
    }

    public Collectable Instantiate(Vector2 position, bool bounce)
    {
        return InstantiateItem(position, bounce, Vector2.zero);
    }

    /// <summary>
    /// Creates an item gameobject of type Collectable
    /// </summary>
    /// <param name="position">Start Position where to spawn the item</param>
    /// <param name="bounce">True, if the item should bounce</param>
    /// <param name="playerPosition">Needed if the item should bounce in a direction</param>
    /// <returns>Type of Collectable</returns>
    public Collectable Instantiate(Vector2 position, bool bounce, Vector2 playerPosition)
    {
        Vector2 direction = position - playerPosition;

        return InstantiateItem(position, bounce, direction);
    }

    /*public Collectable Instantiate(Vector2 position, bool bounce, bool random)
    {
        Vector2 direction = Random.insideUnitCircle;

        return InstantiateItem(position, bounce, direction);
    }*/

    private Collectable InstantiateItem(Vector2 position, bool bounce, Vector2 direction)
    {
        Collectable temp = Instantiate(this.collectable, position, Quaternion.identity);
        temp.SetBounce(bounce, direction);
        temp.name = this.name;
        temp.SetItem(this);
        temp.SetSelfDestruction(this.duration);
        return temp;
    }
}