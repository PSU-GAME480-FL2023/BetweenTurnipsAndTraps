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

    //triggers and actions
    GameObject actionTrigger;
    BoxCollider2D actionCollider;
    GameObject heldObject;
    Rigidbody2D heldR2d;
    List<Collider2D> objectsToAction;
    ContactFilter2D actionFilter;

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

        //grabbing out grab trigger
        actionTrigger = transform.Find("JoActionTrigger").gameObject;
        actionCollider = actionTrigger.GetComponent<BoxCollider2D>();
        objectsToAction = new List<Collider2D>();

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
                actionTrigger.transform.localPosition = new Vector3(0.075f, 0.0f, 0.0f);
                actionTrigger.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                changeAnimationState(direction, JoWalkEast);
            }
        }
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            x_direction = -1;
            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = 'W';
                actionTrigger.transform.localPosition = new Vector3(-0.075f, 0.0f, 0.0f);
                actionTrigger.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
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
                actionTrigger.transform.localPosition = new Vector3(0.0f, 0.075f, 0.0f);
                actionTrigger.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                changeAnimationState(direction, JoWalkNorth);
            }
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            y_direction = -1;
            if (Input.GetKeyDown(KeyCode.S))
            {
                direction = 'S';
                actionTrigger.transform.localPosition = new Vector3(0.0f, -0.075f, 0.0f);
                actionTrigger.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                changeAnimationState(direction, JoWalkSouth);
            }
        }
        else
        {
            y_direction = 0;
        }

        //Action Controls
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if no held object try to talk/pickup
            if(heldObject == null)
            {
                //getting the object to action
                actionCollider.OverlapCollider(actionFilter.NoFilter(), objectsToAction);

                foreach (var contents in objectsToAction){
                    if (contents.gameObject.tag == "NPC")
                    {
                        continue;
                    }
                    else if(contents.gameObject.tag == "Throwable")
                    {
                        heldObject = contents.gameObject;
                        heldR2d = heldObject.GetComponent<Rigidbody2D>();
                        heldR2d.bodyType = RigidbodyType2D.Kinematic;
                        heldR2d.isKinematic = false;
                        continue;
                    }
                }
            }
            //throw held object
            else
            {
                heldR2d.velocity = new Vector2( x_direction * 2, y_direction * 2);
                heldR2d.bodyType = RigidbodyType2D.Dynamic;
                heldR2d.isKinematic = true;

                heldR2d = null;
                heldObject = null;
            }
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
        if (heldObject != null)
        {
            heldObject.transform.position = new Vector3(t.position.x, t.position.y + 0.1f, 0.0f);
        }

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
