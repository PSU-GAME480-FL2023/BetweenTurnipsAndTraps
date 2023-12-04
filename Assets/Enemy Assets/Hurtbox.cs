using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    //Attributes
    public int damage;
    public float knockback;

    List<string> collisionTags;
    BoxCollider2D hurtCollider;
    Transform location;
    Rigidbody2D body;

    void Start()
    {
        location = transform;
        hurtCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();

        collisionTags = new List<string> { "Scorpio", "Trigger", "Untagged", "Dialogue", "Trap"};
    }

    //If Jo gets hit by the hurtbox hurt Jo
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Jo")
        {
            var Jo = other.gameObject.GetComponent<JoController>();

            //getting the angle on the velocity of the attacking object
            float degrees = Mathf.Atan2(body.velocity.y, body.velocity.x);

            var knockbackVector = new Vector2(Mathf.Cos(degrees) * knockback, Mathf.Sin(degrees) * knockback);

            if(!Jo.hurting)
                Jo.hurtJo(knockbackVector, damage);

            if (this.tag == "Projectile" && !collisionTags.Contains(other.gameObject.tag))
            {
                Destroy(this.gameObject);
            }
        }
        else if (this.tag == "Projectile" && !collisionTags.Contains(other.gameObject.tag))
        {
            Destroy(this.gameObject);
        }
    }
}
