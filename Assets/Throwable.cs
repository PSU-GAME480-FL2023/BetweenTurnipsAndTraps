using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    public Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    public Transform t;
    public GameObject thisObject;
    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        thisObject = GetComponent<GameObject>();
        r2d = GetComponent<Rigidbody2D>();
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        mainCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
