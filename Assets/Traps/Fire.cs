using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fire : DamagingSolidObject
{
    //Put out fire
    public void PutOut(Vector3Int fireCell)
    {
        //Erase cell from fire tile map
        GetComponent<Tilemap>().SetTile(fireCell, null);
    }
}
