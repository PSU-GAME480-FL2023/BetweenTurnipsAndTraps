using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //The prefab of the enemy we are spawning
    public GameObject enemyPrefab;
    //The actual enemy spawned into the scene
    private GameObject enemy;
    //Bool that states if the enemy was killed or not
    public bool enemyDead;

    private Vector3 topRightCorner;
    private Vector3 bottomLeftCorner;

    public void SpawnEnemy()
    {
        //If enemy is alive
        if (enemyDead == false)
        {
            //Spawn enemy into scene
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);


            //Clamp enemy within bounds of room
            enemy.GetComponent<Enemy>().SetClampValues(topRightCorner, bottomLeftCorner);
        }
    }

    public void DespawnEnemy()
    {
        //If enemy is still alive
        if (enemy.GetComponent<Enemy>().health > 0)
        {
            //Despawn enemy object
            Destroy(enemy);
        }

        //If enemy is dead
        else
        {
            //Mark enemy as dead
            enemyDead = true;
        }
    }

    public void SetCorners(Vector3 topRightCorner, Vector3 bottomLeftCorner)
    {
        this.topRightCorner = topRightCorner;
        this.bottomLeftCorner = bottomLeftCorner;
    }
}
