using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChange : MonoBehaviour
{
    //Every Enemy Spawner that is part of this room.
    public GameObject[] enemySpawners;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If Jo enters the room collider
        if (collision.tag == "Jo")
        {
            Debug.Log("In room now");

            //Change the camera to be positioned at this room
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.GetComponent<DungeonCamera>().ChangeCameraPosition(this.transform.position + new Vector3(0, 0, camera.transform.position.z));

            //Spawn enemies in enemySpawners list
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                //enemySpawners[i].GetComponent<EnemySpawner>().SpawnEnemy();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If Jo exits the room collider
        if (collision.tag == "Jo")
        {
            Debug.Log("Leaving room now");

            //Despawn enemies in enemySpawners list
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                //enemySpawners[i].GetComponent<EnemySpawner>().DespawnEnemy();
            }
        }
    }
}
