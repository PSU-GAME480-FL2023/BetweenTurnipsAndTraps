using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour
{
    public bool last;
    public int destination;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Jo")
        {
            if (last)
            {
                SceneManager.LoadScene(destination);
            }

            //Get the Dungeon Manager
            DungeonManager dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonManager>();

            //Change Floor
            dungeonManager.ReloadScene();
        }
    }
}
