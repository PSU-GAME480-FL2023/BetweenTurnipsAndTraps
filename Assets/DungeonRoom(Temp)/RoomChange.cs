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

            //Get new position for player to respawn in
            Vector3 newPosition = CreateNewRespawnPoint(collision);

            //Update respawn point
            collision.GetComponent<JoController>().UpdateRespawnPoint(newPosition);

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

    private Vector3 CreateNewRespawnPoint(Collider2D collision)
    {
        //New spawn position
        Vector3 newRespawnPoint = new Vector3(0, 0, 0);

        // Calculate the direction vector from the center of the collider to the other object
        Vector2 direction = this.transform.position - collision.transform.position;

        // Determine the side based on the direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Collided from the left or right side
            if (direction.x > 0)
            {
                Debug.Log("Entered from the left side");
                newRespawnPoint = collision.transform.position - new Vector3(1, 0, 0);
            }
            else
            {
                Debug.Log("Entered from the right side");
                newRespawnPoint = collision.transform.position + new Vector3(1, 0, 0);
            }
        }
        else
        {
            // Collided from the top or bottom side
            if (direction.y > 0)
            {
                Debug.Log("Entered from the bottom side");
                newRespawnPoint = collision.transform.position - new Vector3(0, 1, 0);
            }
            else
            {
                Debug.Log("Entered from the top side");
                newRespawnPoint = collision.transform.position + new Vector3(0, 1, 0);
            }
        }

        return newRespawnPoint;
    }
}
