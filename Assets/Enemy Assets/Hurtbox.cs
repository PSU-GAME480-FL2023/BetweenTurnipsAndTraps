using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    //Attributes
    int damage;
    Vector2 knockback;

    Collider2D hurtCollider;

    void Start()
    {
        hurtCollider = GetComponent<Collider2D>();
    }

    //If Jo gets hit by the hurtbox hurt Jo
    private void OnTriggerEnter2d(Collider2D other)
    {
        if(other.gameObject.tag == "Jo")
        {
            var Jo = other.gameObject.GetComponent<JoController>();
            Jo.hurtJo(knockback, damage);
        }
    }
}
