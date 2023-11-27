using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColdWater : FallTrap
{
    public TileBase iceTile;

    public Tilemap iceTilemap;

    public void Freeze(Vector3Int waterCell)
    {
        //Erase cell from pit tile map
        GetComponent<Tilemap>().SetTile(waterCell, null);

        //Replace cell with ice tile in the ice tilemap
        iceTilemap.SetTile(waterCell, iceTile);
    }
}
