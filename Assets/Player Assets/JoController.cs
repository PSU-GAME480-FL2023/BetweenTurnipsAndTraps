using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script Handles the Basic moverments and actions of the player character
public class JoController : MonoBehaviour
{
    public float max_speed;
    public float walk_speed;
    public int x_direction = 0;
    public int y_direction = 0;

    //Animation
    Animator animator;
    public char direction = 'S';
    public string cur_anim;
    const string JoSouthIdle = "JoIdleS";
    const string JoWalkSouth = "JoWalkS";
    const string JoWalkNorth = "JoWalkN";
    const string JoWalkEast = "JoWalkE";
    const string JoWalkWest = "JoWalkW";

    Camera mainCamera;

    Vector3 cameraPos;
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    Transform t;

    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        mainCollider = GetComponent<BoxCollider2D>();

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }

        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            x_direction = 1;
            if(Input.GetKeyDown(KeyCode.D)){
                direction = 'E';
                changeAnimationState(direction, JoWalkEast);
            }
        }
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            x_direction = -1;
            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = 'W';
                changeAnimationState(direction, JoWalkWest);
            }
        }
        else
        {
            x_direction = 0;
        }

        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            y_direction = 1;
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = 'N';
                changeAnimationState(direction, JoWalkNorth);
            }
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            y_direction = -1;
            if (Input.GetKeyDown(KeyCode.S))
            {
                direction = 'S';
                changeAnimationState(direction, JoWalkSouth);
            }
        }
        else
        {
            y_direction = 0;
        }

        //Action Controls
        if (Input.GetKey(KeyCode.Space))
        {

        }

        if (Input.GetKey(KeyCode.Q))
        {

        }

        if (Input.GetKey(KeyCode.Space))
        {

        }

        //Left click
        if (Input.GetKey(KeyCode.Mouse0))
        {

        }

        //Right click
        if (Input.GetKey(KeyCode.Mouse1))
        {

        }

        //Menu Controls
        //Main Menu
        if (Input.GetKey(KeyCode.Escape))
        {

        }

        //Map Menu
        if (Input.GetKey(KeyCode.M))
        {

        }

        //Inventory
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log(r2d.velocity);
        }
    }

    private void FixedUpdate()
    {
        r2d.velocity = new Vector2(x_direction * max_speed, y_direction * max_speed);

        if(r2d.velocity == Vector2.zero)
        {
            changeAnimationState('S', JoSouthIdle);
        }
    }

    //Animation
    void changeAnimationState(char direction, string anim)
    {
        //current animation same do nothing
        if(cur_anim == anim)
        {
            return;
        }

        animator.Play(anim);
        cur_anim = anim;

        //directional based animation?
        //switch (direction){
        //    case 'S':
        //        break;
        //    case 'N':
        //        break;
        //    case 'E':
        //        break;
        //    case 'W':
        //        break;
        //}

        return;
    }
}
