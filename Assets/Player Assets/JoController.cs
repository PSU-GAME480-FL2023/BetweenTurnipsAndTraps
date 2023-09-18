using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script Handles the Basic moverments and actions of the player character
public class JoController : MonoBehaviour
{
    public float max_speed = 20.0f;
    public float walk_speed = 10.0f;
    public float x_speed = 0.0f;
    public float y_speed = 0.0f;
    public Camera mainCamera;

    Vector3 cameraPos;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;

    void Start()
    {
        t = transform;
        mainCollider = GetComponent<CapsuleCollider2D>();

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if (Input.GetKey(KeyCode.D))
        {
            t.localPosition = new Vector2(t.localPosition.x + .02f, t.localPosition.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            t.localPosition = new Vector2(t.localPosition.x - .02f, t.localPosition.y);
        }
        if (Input.GetKey(KeyCode.W))
        {
            t.localPosition = new Vector2(t.localPosition.x, t.localPosition.y + .02f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            t.localPosition = new Vector2(t.localPosition.x, t.localPosition.y - .02f);
        }
    }
}
