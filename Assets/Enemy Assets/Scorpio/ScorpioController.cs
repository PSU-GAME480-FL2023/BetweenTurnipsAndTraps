using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpioController : MonoBehaviour
{
    bool isShooting;
    public int firerate;
    public float firespeed;
    public GameObject projectile;

    Animator animator;
    BoxCollider2D mainCollider;
    Transform t;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        animator.SetBool("isShooting", true);

        t = transform;
    }

    void Shoot()
    {
        var newProj = Instantiate(projectile, new Vector3(t.localPosition.x - 0.375f, t.localPosition.y + 0.5F, -2), Quaternion.Euler(0, 0, 90));
        var temp = newProj.GetComponent<Rigidbody2D>();
        temp.velocity = new Vector2(-firespeed, 0);
    }
}
