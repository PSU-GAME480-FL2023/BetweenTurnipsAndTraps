using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //On player enter
    void OnTriggerEnter2D(Collider2D col)
    {
        //Pause control for a brief second
        //Child script will use its own animator here to play animation of player falling
        //Damage player
        //Respawn player at solid ground tile they were at (Maybe this can handle it, maybe a manager can.
    }
}
