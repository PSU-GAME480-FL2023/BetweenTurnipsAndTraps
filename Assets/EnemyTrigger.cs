using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject barrier;
    Collider2D collide;
    ContactFilter2D actionFilter;
    List<Collider2D> objectsToAction;

    public void Awake()
    {
        collide = GetComponent<Collider2D>();
        objectsToAction = new List<Collider2D>();
    }

    public void Update()
    {
        collide.OverlapCollider(actionFilter.NoFilter(), objectsToAction);

        var live = false;
        foreach (var contents in objectsToAction)
        {
            if (contents.gameObject.tag == "Enemy")
                live = true;
        }

        if (!live)
        {
            Destroy(barrier);
            Destroy(this);
        }

    }
}
