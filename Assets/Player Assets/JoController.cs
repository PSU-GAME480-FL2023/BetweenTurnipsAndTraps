using System.Collections.Generic;
using UnityEngine;

//This Script Handles the Basic moverments and actions of the player character
public class JoController : MonoBehaviour
{
    //Jo's attributes
    public int health;
    public int cash;
    //public object action1
    //public object action2
    public float max_speed;
    public float current_speed;
    public int x_direction = 0;
    public int y_direction = 0;
    public float maxSpeedOnIce;
    public float accelerationOnIce;
    private bool onIce = false;
    public bool busy = false;
    public bool inVilliage = false;
    public bool isFarming = false;
    public bool isAttacking = false;
    private bool flying = false;
    private bool fruitActive = false;

    //Animation
    Animator animator;
    public char direction = 'S';
    public string cur_anim;
    const string JoSouthIdle = "JoIdleS";
    const string JoWalkSouth = "JoWalkS";
    const string JoWalkNorth = "JoWalkN";
    const string JoWalkEast = "JoWalkE";
    const string JoWalkWest = "JoWalkW";

    //Iteraction and Throw objects
    GameObject actionTrigger;
    BoxCollider2D actionCollider;
    List<Collider2D> objectsToAction;
    ContactFilter2D actionFilter;
    GameObject heldObject;
    public GameObject[] heldObjects;
    public int[] heldObjectsAmmo;
    Rigidbody2D heldR2d;
    Collider2D heldCollider;

    //Basics
    Vector3 cameraPos;
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    Transform t;
    Vector3 respawnPoint;

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
        // If we are busy do not allow the user input
        if (!busy)
        {
            // Movement controls and actions
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                x_direction = 1;
                if (Input.GetKeyDown(KeyCode.D))
                {
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
                if (heldObject == null)
                {
                    //getting the object to action
                    actionCollider.OverlapCollider(actionFilter.NoFilter(), objectsToAction);

                    foreach (var contents in objectsToAction)
                    {
                        if (contents.gameObject.tag == "Dialogue")
                        {
                            contents.gameObject.GetComponent<InteractPrompt>().PrintDialogue();
                            continue;
                        }
                        else if (contents.gameObject.tag == "Throwable")
                        {
                            heldObject = contents.gameObject;
                            heldR2d = heldObject.GetComponent<Rigidbody2D>();
                            heldCollider = heldObject.GetComponent<Collider2D>();

                            heldCollider.isTrigger = true;
                            continue;
                        }
                    }
                }
                //throw held object
                else
                {
                    if (x_direction == 0 && y_direction == 0)
                    {
                        heldR2d.velocity = new Vector2(0.0f, 0.0f);
                        heldCollider.isTrigger = false;
                    }
                    else
                    {
                        heldR2d.isKinematic = true;
                        heldR2d.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                        heldR2d.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                        heldR2d.velocity = new Vector2(x_direction * 7f, y_direction * 7f);
                    }

                    heldCollider = null;
                    heldR2d = null;
                    heldObject = null;
                }
            }

            if (Input.GetKey(KeyCode.Q))
            {

            }

            //Left click
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (heldObject == null)
                {
                    animator.SetBool("isAttack", true);
                    isAttacking = true;
                }
            }

            //Right click
            if (Input.GetKey(KeyCode.Mouse1))
            {

            }
        }

        //Use an item in tool bar
        if (Input.GetKeyDown(KeyCode.Alpha1) && heldObject == null && heldObjects[0] != null)
        {
            //If object is throwable
            if (heldObjects[0].tag == "Throwable")
            {
                //Create instance of fruit
                GameObject fruitInstance = Instantiate(heldObjects[0]);

                //Make Jo hold object
                heldObject = fruitInstance;
                heldR2d = heldObject.GetComponent<Rigidbody2D>();
                heldCollider = heldObject.GetComponent<Collider2D>();

                heldCollider.isTrigger = true;

                //Remove one from ammo count
                heldObjectsAmmo[0] = heldObjectsAmmo[0] - 1;

                //Remove item if we are  out of ammo
                if (heldObjectsAmmo[0] <= 0)
                {
                    heldObjects[0] = null;
                }
            }

            //If object is consumable
            else if (heldObjects[0].tag == "Consumable" && fruitActive == false)
            {
                //Mark fruit as in use
                fruitActive = true;

                //Create instance of fruit
                GameObject fruitInstance = Instantiate(heldObjects[0]);

                //Activate the effect of the item
                fruitInstance.GetComponent<ConsumableItem>().ActivateEffect(this.transform.gameObject);

                //Stop using fruit
                fruitActive = false;

                //Remove one from ammo count
                heldObjectsAmmo[0] = heldObjectsAmmo[0] - 1;

                //Remove item if we are  out of ammo
                if (heldObjectsAmmo[0] <= 0)
                {
                    heldObjects[0] = null;
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2) && heldObject == null && heldObjects[1] != null)
        {
            //If object is throwable
            if (heldObjects[1].tag == "Throwable")
            {
                //Create instance of fruit
                GameObject fruitInstance = Instantiate(heldObjects[1]);

                //Make Jo hold object
                heldObject = fruitInstance;
                heldR2d = heldObject.GetComponent<Rigidbody2D>();
                heldCollider = heldObject.GetComponent<Collider2D>();

                heldCollider.isTrigger = true;

                //Remove one from ammo count
                heldObjectsAmmo[1] = heldObjectsAmmo[1] - 1;

                //Remove item if we are  out of ammo
                if (heldObjectsAmmo[1] <= 0)
                {
                    heldObjects[1] = null;
                }
            }

            //If object is consumable
            else if (heldObjects[1].tag == "Consumable" && fruitActive == false)
            {
                //Mark fruit as in use
                fruitActive = true;

                //Create instance of fruit
                GameObject fruitInstance = Instantiate(heldObjects[0]);

                //Activate the effect of the item
                fruitInstance.GetComponent<ConsumableItem>().ActivateEffect(this.transform.gameObject);

                //Stop using fruit
                fruitActive = false;

                //Remove one from ammo count
                heldObjectsAmmo[1] = heldObjectsAmmo[1] - 1;

                //Remove item if we are  out of ammo
                if (heldObjectsAmmo[1] <= 0)
                {
                    heldObjects[1] = null;
                }
            }
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
        var newVelocity = r2d.velocity;
        if(x_direction != 0 && ((x_direction == 1  && newVelocity.x < max_speed) || (x_direction == -1 && newVelocity.x > -max_speed)))
        {
            newVelocity.x = r2d.velocity.x + (.5f * x_direction);
        }
        if (y_direction != 0 && ((y_direction == 1 && newVelocity.y < max_speed) || (y_direction == -1 && newVelocity.y > -max_speed)))
        {
            newVelocity.y = r2d.velocity.y + (.5f * y_direction);
        }

        if(x_direction == 0 && busy == false && Mathf.Abs(r2d.velocity.x) > .01 )
        {
            newVelocity.x = r2d.velocity.x * .75f;
        }
        if (y_direction == 0 && busy == false && Mathf.Abs(r2d.velocity.y) > .01)
        {
            newVelocity.y = r2d.velocity.y * .75f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (isAttacking)
        {
            actionCollider.OverlapCollider(actionFilter.NoFilter(), objectsToAction);

            foreach (var contents in objectsToAction)
            {
                if (contents.gameObject.tag == "Enemy" || contents.gameObject.tag == "Scorpio")
                {
                    if(contents.gameObject.GetComponent<Enemy>().isHurt == false)
                    {
                        contents.gameObject.GetComponent<Enemy>().Hurt(5);
                    }
                }
            }
        }

        Vector2 inputDirection = new Vector2(horizontalInput, verticalInput).normalized;

        //If the player is not on ice, make them move normally
        if (onIce == false)
        {
            r2d.velocity = newVelocity;
        }
        //Otherwise, apply ice physics
        else
        {
            Vector2 targetVelocity = inputDirection * maxSpeedOnIce;
            r2d.velocity = Vector2.Lerp(r2d.velocity, targetVelocity, Time.deltaTime * accelerationOnIce);
        }

        animator.SetFloat("verticalSpeed", r2d.velocity.y);
        animator.SetFloat("horizontalSpeed", r2d.velocity.x);

        if (heldObject != null)
        {
            heldObject.transform.position = new Vector3(t.position.x, t.position.y + (t.localScale.y * 0.1f), this.transform.position.z);
        }
    }

    public void endAttack()
    {
        animator.SetBool("isAttack", false);
        isAttacking = false;
    }
    public void hurtJo(Vector2 knockback, int damage)
    {
        // Damage Jo
        health -= damage;
        Debug.Log("Hurt");

        // Drop item
        if (heldObject != null)
        {
            heldR2d.velocity = new Vector2(0.0f, 0.0f);
            heldCollider.isTrigger = false;
        }

        // Apply the knockback to Jo's velocity
        r2d.velocity = knockback;

        if(health <= 0)
        {
            Debug.Log("DEATH");
            //KILL JO
        }

        //busy = true;
    }

    public Vector2 GetColliderCenter()
    {
        return new Vector2(mainCollider.transform.position.x + mainCollider.offset.x, mainCollider.transform.position.y + mainCollider.offset.y);
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

    //Set if the player is using ice controls or not
    public void SetOnIce(bool onIce)
    {
        this.onIce = onIce;
    }

    //Set flying status of player
    public void SetFlying(bool flying)
    {
        this.flying = flying;
    }

    //Get flying status of player
    public bool GetFlying()
    {
        return flying;
    }

    //Update where the player will respawn.
    public void UpdateRespawnPoint()
    {
        this.respawnPoint = this.transform.position;
    }

    public void Respawn()
    {
        this.transform.position = respawnPoint;
    }
}
