using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IceSpikes : DamagingSolidObject
{
    //Melt Ice
    public void Melt(Vector3Int iceCell)
    {
        //Erase cell from ice tile map
        GetComponent<Tilemap>().SetTile(iceCell, null);
    }
}
