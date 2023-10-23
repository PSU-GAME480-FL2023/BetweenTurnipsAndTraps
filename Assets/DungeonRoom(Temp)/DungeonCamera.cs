using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    //Move the camera to Vector3 position passed
    public void ChangeCameraPosition(Vector3 newPosition)
    {
        //Change the camera to be positioned at this room
        this.transform.position = newPosition;
    }
}
