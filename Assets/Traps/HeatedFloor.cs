using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeatedFloor : MonoBehaviour
{
    //Amount of time the floor will stay cool
    public float coolTime;
    //Amount of time the floor will stay heated
    public float heatTime;

    public Tilemap waterTilemap;
    public Tilemap floorTilemap;
    public Vector2 flowVector;

    private bool heated;

    // Start is called before the first frame update
    void Start()
    {
        heated = false;

        HeatLoop();
    }

    private void HeatLoop()
    {
        while (true)
        {
            //Start with cool tilemaps
            //Turn hurt off
            heated = false;
            //Change appearance

            //Wait for coolTime
            Wait(coolTime);

            //Go to heated tilemaps
            //Turn hurt on
            heated = true;
            //Change appearance

            //Wait for heatTime
            Wait(heatTime);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Jo" && heated == true)
        {
            //Push player/object in that direction
            collider.attachedRigidbody.AddForce(flowVector);
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
