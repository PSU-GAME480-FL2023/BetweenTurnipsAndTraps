using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingSolidObject : MonoBehaviour
{
    public int damage;
    public float knockback;

    //On player collision
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Jo")
        {
            var Jo = other.gameObject.GetComponent<JoController>();

            //Get player Rigidbody
            Rigidbody2D joRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

            Vector2 knockbackDirection = (other.contacts[0].point - (Vector2)joRigidbody.transform.position).normalized;

            //getting the angle on the velocity of the attacking object

            joRigidbody.AddForce(knockback * -knockbackDirection, ForceMode2D.Impulse);

            //Jo.hurtJo(knockbackVector, damage);
        }

        if (other.gameObject.tag == "Throwable")
        {
            Debug.Log(other.gameObject);
            //Activate effect of throwable object
            other.gameObject.GetComponent<ThrowableItem>().ActivateEffect();
        }
    }
}
