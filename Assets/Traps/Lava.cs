using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//When placing tilemaps, make sure to put floor tile map below lava tilemap

public class Lava : MonoBehaviour
{
    //Put out fire
    public void Harden(Vector3Int lavaCell)
    {
        //Erase cell from fire tile map
        GetComponent<Tilemap>().SetTile(lavaCell, null);
    }
}
