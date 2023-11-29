using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    //Attributes
    public int damage;
    public float knockback;

    BoxCollider2D hurtCollider;
    Transform location;
    Rigidbody2D body;

    void Start()
    {
        location = transform;
        hurtCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
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

            Jo.hurtJo(knockbackVector, damage);

            if (this.tag == "Projectile" && other.gameObject.tag != "Scorpio" && other.gameObject.tag != "Trigger" && other.gameObject.tag != "Untagged" && other.gameObject.tag != "Dialogue")
            {
                Destroy(this.gameObject);
            }
        }
        else if (this.tag == "Projectile" && other.gameObject.tag != "Scorpio" && other.gameObject.tag != "Trigger" && other.gameObject.tag != "Untagged" && other.gameObject.tag != "Dialogue")
        {
            Destroy(this.gameObject);
        }
    }
}
