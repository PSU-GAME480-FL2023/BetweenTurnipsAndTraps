using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    //
    private Vector3 lastSafePosition;


    //On player enter
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //If Jo enters the fall trap collider
        if (collider.tag == "Jo")
        {
            //Pause control for ___ seconds
            //Child script will use its own animator here to play animation of player falling
            //Damage player
            //Respawn player at solid ground tile they were at (Maybe this can handle it, maybe a manager can.
            collider.GetComponent<JoController>().Respawn();
        }
    }
}
