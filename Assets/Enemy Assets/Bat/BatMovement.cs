using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    public enum FlightDirection
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    private FlightDirection flightDirection;

    private Vector2 velocity;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        int startDirection = Random.Range(0, 3);

        if (startDirection == 0)
        {
            flightDirection = FlightDirection.UpLeft;
        }
        else if (startDirection == 1)
        {
            flightDirection = FlightDirection.UpRight;
        }
        else if (startDirection == 2)
        {
            flightDirection = FlightDirection.DownLeft;
        }
        else
        {
            flightDirection = FlightDirection.DownRight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (flightDirection)
        {
            case FlightDirection.UpLeft:
                velocity = new Vector2(-1f, 1f).normalized;
                break;
            case FlightDirection.UpRight:
                velocity = new Vector2(1f, 1f).normalized;
                break;
            case FlightDirection.DownLeft:
                velocity = new Vector2(-1f, -1f).normalized;
                break;
            case FlightDirection.DownRight:
                velocity = new Vector2(1f, -1f).normalized;
                break;
        }

        transform.Translate(velocity * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if bat has collided with wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Change direction
            switch (flightDirection)
            {
                case FlightDirection.UpLeft:
                    flightDirection = FlightDirection.DownLeft;
                    break;
                case FlightDirection.UpRight:
                    flightDirection = FlightDirection.DownRight;
                    break;
                case FlightDirection.DownRight:
                    flightDirection = FlightDirection.UpRight;
                    break;
                case FlightDirection.DownLeft:
                    flightDirection = FlightDirection.UpLeft;
                    break;
            }
        }
    }
}
