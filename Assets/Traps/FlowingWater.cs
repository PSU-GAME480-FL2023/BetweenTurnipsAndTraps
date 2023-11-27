using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingWater : MonoBehaviour
{
    //enum flowDirection
    public enum FlowDirection
    {
        Up,
        Down,
        Left,
        Right
    };

    public FlowDirection flowDirection;
    private Vector2 flowVector;

    //On Start of script, get vector that will be used for push on player object and set default
    public void Start()
    {
        //If direction is flowing up
        if(flowDirection == FlowDirection.Up)
        {
            flowVector = new Vector2(0.0f, 20.0f);
        }
        //If direction is flowing down
        else if (flowDirection == FlowDirection.Down)
        {
            flowVector = new Vector2(0.0f, -20.0f);
        }
        //If direction is flowing left
        else if (flowDirection == FlowDirection.Left)
        {
            flowVector = new Vector2(-20.0f, 0.0f);
        }
        //If direction is flowing right
        else if (flowDirection == FlowDirection.Right)
        {
            flowVector = new Vector2(20.0f, 0.0f);
        }
    }

    //Note: On top of grate, maybe just make water neutral, but slow player movement to mimic water

    //When player/object is in collider for flowing water
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Jo" && collider.GetComponent<JoController>().GetFlying() == false)
        {
            //Push player/object in that direction
            collider.attachedRigidbody.AddForce(flowVector);
        }
    }
}
