using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fountain : MonoBehaviour
{
    //enum flowDirection
    public enum FlowDirection
    {
        Up,
        Down,
        Left,
        Right
    };

    public FlowDirection flowDirection;

    //The tiles for each direction of the fountain
    public TileBase waterTile;

    //The tilemaps for each direction of the fountain
    public Tilemap waterTilemap;

    public Tilemap floorTilemap;

    private Vector3 tilemapOffset;


    private void Start()
    {
        //Get the offset of the tilemap
        Debug.Log(transform.parent.gameObject + "Swag");
        //Spawn the water stream in
        SpawnWaterStreams();
    }

    private void SpawnWaterStreams()
    {
        Vector3Int[] fountainCells = GetFountainCells();

        foreach (Vector3Int fountainCell in fountainCells)
        {
            Debug.Log("Creating Stream");
            SpawnWaterStream(fountainCell);
        }
    }

    //Spawn new water tile in flowDirection from previously spawned water tile
    private void SpawnWaterStream(Vector3Int fountainCell)
    {
        //Make the direction the water is pointing into a vector
        Vector3Int waterFlowOffset = Vector3Int.zero;

        switch (flowDirection)
        {
            case FlowDirection.Up:
                waterFlowOffset.y = 1;
                break;
            case FlowDirection.Down:
                waterFlowOffset.y = -1;
                break;
            case FlowDirection.Left:
                waterFlowOffset.x = -1;
                break;
            case FlowDirection.Right:
                waterFlowOffset.x = 1;
                break;
        }

        //This line does NOT work (In this context)
        Vector3Int tilemapPosition = new Vector3Int(Mathf.FloorToInt(GetComponent<Tilemap>().transform.position.x), Mathf.FloorToInt(GetComponent<Tilemap>().transform.position.y), 0);

        Vector3Int currentCell = fountainCell + waterFlowOffset;
        Debug.Log("Current Cell: " + currentCell);
        Debug.Log(floorTilemap.transform.position);
        Debug.Log(tilemapPosition);

        while (floorTilemap.GetTile(currentCell) != null)
        {
            //Until nextWaterTilePosition is not a floor tile, the last tile a water tile was spawned on was a grate, or nextwatertile is oob
            waterTilemap.SetTile(currentCell, waterTile);
            currentCell += waterFlowOffset;
        }
    }

    public void DespawnWaterStream()
    {

    }

    private Vector3Int[] GetFountainCells()
    {
        //Establish bounds of 
        BoundsInt bounds = GetComponent<Tilemap>().cellBounds;
        List<Vector3Int> fountainCells = new List<Vector3Int>();

        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                //Debug.Log("Hrrr");
                Vector3Int cell = new Vector3Int(x, y, 0);
                TileBase tile = GetComponent<Tilemap>().GetTile(cell);

                if (tile != null )
                {
                    Debug.Log("Fountain Cell Added at " + (cell));
                    fountainCells.Add(cell);
                }
            }
        }

        return fountainCells.ToArray();
    }
}
