using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    //Radius of effect
    public float effectRadius = 3f;

    //Damage caused
    public int damage = 1;

    List<string> collisionTags;
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    Transform t;
    GameObject thisObject;

    // Start is called before the first frame update
    void Start()
    {
        collisionTags = new List<string> { "Enemy", "Wall", "Trap" };
        t = transform;
        thisObject = GetComponent<GameObject>();
        r2d = GetComponent<Rigidbody2D>();
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        mainCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Activate effect of fruit
        if (collisionTags.Contains(collision.gameObject.tag))
        {
            ActivateEffect();
        }
    }

    public virtual void ActivateEffect()
    {
        //Create a circle collider and get every collider within that circle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, effectRadius);

        //Go through each object
        foreach (Collider2D collider in colliders)
        {
            //If it is an enemy, damage it
            if (collider.tag == "Enemy")
            {
                //If enemy has elemental aspect (found in enum in Enemy script), change damage

                //Damage enemy
                collider.GetComponent<Enemy>().Hurt(damage);
            }

            //If it is a trap
            else if (collider.tag == "Trap")
            {
                //Check which trap it is and call specific script
            }
        }
    }
}
