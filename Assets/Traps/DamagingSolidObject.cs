using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingSolidObject : MonoBehaviour
{
    public int damage;
    public float knockback;

    //On player collision
    /*void OnCollisionEnter2D(Collision2D col)
    {
        if (other.gameObject.tag == "Jo")
        {
            var Jo = other.gameObject.GetComponent<JoController>();

            //getting the angle on the velocity of the attacking object
            float degrees = Mathf.Atan2(body.velocity.y, body.velocity.x);

            var knockbackVector = new Vector2(Mathf.Cos(degrees) * knockback, Mathf.Sin(degrees) * knockback);

            Jo.hurtJo(knockbackVector, damage);
        }
    }*/
}
