using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    List<string> collisionTags;
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    Transform t;
    GameObject thisObject;
    // Start is called before the first frame update
    void Start()
    {
        collisionTags = new List<string> { "Enemy", "Scorpio", "Throwable", "Wall", "Dialogue"};
        t = transform;
        thisObject = GetComponent<GameObject>();
        r2d = GetComponent<Rigidbody2D>();
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        mainCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (r2d.velocity.x != 0 || r2d.velocity.y != 0)
        {
            Debug.Log("Here");
            if (collisionTags.Contains(collision.gameObject.tag))
            {
                Debug.Log("Here2");
                r2d.velocity = new Vector2(0.0f, 0.0f);
                mainCollider.isTrigger = false;
            }
        }
    }
}
