using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Jo")
        {
            //Get the Dungeon Manager
            DungeonManager dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonManager>();

            //Change Floor
            dungeonManager.ReloadScene();
        }
    }
}
