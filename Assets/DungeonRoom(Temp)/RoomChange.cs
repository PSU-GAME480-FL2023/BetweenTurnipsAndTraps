using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomChange : MonoBehaviour
{
    //Every Enemy Spawner that is part of this room.
    public GameObject[] enemySpawners;

    //When the Room is instantiated
    private void Start()
    {
        //Get size of entire grid
        BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);

        Tilemap[] tilemaps = transform.parent.GetComponentsInChildren<Tilemap>();

        bounds = tilemaps[0].cellBounds;

        foreach (Tilemap tilemap in tilemaps)
        {
            Vector3Int minPosition = Vector3Int.Min(bounds.min, tilemap.cellBounds.min);
            Vector3Int maxPosition = Vector3Int.Max(bounds.max, tilemap.cellBounds.max);

            bounds.SetMinMax(minPosition, maxPosition);
        }

        //Get size of each cell
        Debug.Log(transform.parent);
        Vector3 cellSize = transform.parent.GetComponent<Grid>().cellSize;

        //Get width of room
        float widthFromCenter = (bounds.size.x * cellSize.x) / 2;
        //Get height of room
        float heightFromCenter = (bounds.size.y * cellSize.y) / 2;

        //Get top right and bottom left coordinates of this room
        Vector3 topRightCorner = new Vector3();
        Vector3 bottomLeftCorner = new Vector3();

        //Tell every room what the top right and bottom left coordinates of this room are
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawners[i].GetComponent<EnemySpawner>().SetCorners(topRightCorner, bottomLeftCorner);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If Jo enters the room collider
        if (collision.tag == "Jo")
        {
            //Change the camera to be positioned at this room
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.GetComponent<DungeonCamera>().ChangeCameraPosition(this.transform.position + new Vector3(0, 0, camera.transform.position.z));

            //Spawn enemies in enemySpawners list
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                enemySpawners[i].GetComponent<EnemySpawner>().SpawnEnemy();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If Jo exits the room collider
        if (collision.tag == "Jo")
        {
            //Despawn enemies in enemySpawners list
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                enemySpawners[i].GetComponent<EnemySpawner>().DespawnEnemy();
            }
        }
    }
}
