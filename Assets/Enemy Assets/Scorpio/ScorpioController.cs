using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpioController : MonoBehaviour
{
    bool isShooting;
    public int firerate;
    public float firespeed;
    public GameObject projectile;

    public enum FireDirection
    {
        Up,
        Left,
        Right,
        Down
    }

    public FireDirection fireDirection;

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
        //Fire in specified direction
        if (fireDirection.ToString() == "Up")
        {
            var newProj = Instantiate(projectile, new Vector3(t.localPosition.x, t.localPosition.y + 0.375f, -2), Quaternion.Euler(0, 0, 0));
            var temp = newProj.GetComponent<Rigidbody2D>();
            temp.velocity = new Vector2(0, firespeed);
        }

        else if (fireDirection.ToString() == "Left")
        {
            var newProj = Instantiate(projectile, new Vector3(t.localPosition.x - 0.375f, t.localPosition.y + 0.5F, -2), Quaternion.Euler(0, 0, 90));
            var temp = newProj.GetComponent<Rigidbody2D>();
            temp.velocity = new Vector2(-firespeed, 0);
        }

        else if (fireDirection.ToString() == "Right")
        {
            var newProj = Instantiate(projectile, new Vector3(t.localPosition.x + 0.375f, t.localPosition.y + 0.5f, -2), Quaternion.Euler(0, 0, 270));
            var temp = newProj.GetComponent<Rigidbody2D>();
            temp.velocity = new Vector2(firespeed, 0);
        }

        else
        {
            var newProj = Instantiate(projectile, new Vector3(t.localPosition.x, t.localPosition.y - 0.375f, -2), Quaternion.Euler(0, 0, 180));
            var temp = newProj.GetComponent<Rigidbody2D>();
            temp.velocity = new Vector2(0, -firespeed);
        }
    }
}
