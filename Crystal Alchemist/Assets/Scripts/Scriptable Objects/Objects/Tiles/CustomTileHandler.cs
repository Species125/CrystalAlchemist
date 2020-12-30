using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTileHandler : MonoBehaviour
{
    /*
    public TileBase tileA;
    public TileBase tileB;

    [SerializeField]
    private TimeValue timeValue;
    */

    [SerializeField]
    private List<CustomTile> customTiles = new List<CustomTile>();

    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        RefreshMap();
    }

    public void RefreshMap()
    {
        if (tilemap != null)
        {
            tilemap.RefreshAllTiles();
        }
    }
    /*
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        
        foreach(CustomTile tile in customTiles)
        {
            if(tile.get)
        }


    tilemap.SwapTile(tileA, tileB);
    }*/
}
