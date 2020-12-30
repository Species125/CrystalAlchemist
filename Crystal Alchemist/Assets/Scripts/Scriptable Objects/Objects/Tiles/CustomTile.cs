using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct TileSprites
{
    public Sprite sprite;
    public season season;
}

[CreateAssetMenu(menuName = "Game/Tiles/Custom Tile")]
public class CustomTile : Tile
{    
    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private List<TileSprites> tileSprites = new List<TileSprites>();
    
    [SerializeField]
    private TimeValue timeValue;
    
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = defaultSprite;
        Sprite sprite = GetSprite(this.timeValue);
        if (sprite != null) tileData.sprite = sprite;
    }

    public Sprite GetSprite(TimeValue value)
    {
        foreach(TileSprites sprites in this.tileSprites)
        {
            if(sprites.season == value.season) return sprites.sprite;            
        }

        return null;
    }
}
