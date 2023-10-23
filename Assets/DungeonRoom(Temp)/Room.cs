using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Bools for every possible door position. Mark every door this room has as "true"
    public bool upDoor;
    public bool leftDoor;
    public bool rightDoor;
    public bool downDoor;

    public enum RoomType
    {
        start,
        danger,
        treasure,
        stairs
    }

    public RoomType roomType;

    //Return the doors that this room has as a string formatted as "ULRD"
    public string GetOpenings()
    {
        string openings = "";

        if (upDoor)
        {
            openings = openings + "U";
        }
        if (leftDoor)
        {
            openings = openings + "L";
        }
        if (rightDoor)
        {
            openings = openings + "R";
        }
        if (downDoor)
        {
            openings = openings + "D";
        }

        return openings;
    }

    public string GetRoomType()
    {
        return roomType.ToString();
    }
}
