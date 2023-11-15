using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool InDungeon;
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        if (!InDungeon)
        {
            transform.position = new Vector3(player.position.x, player.position.y, -1.0f);
        }
    }
}
