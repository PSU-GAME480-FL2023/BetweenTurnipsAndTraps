using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Health of enemy
    public int health = 10;
    //Speed of enemy
    public int speed = 10;
    //Damage enemy does
    public int damage = 10;

    //Elemental aspect of enemy
    public enum ElementalAspect
    {
        Fire,
        Water,
        Ice
    };

    public ElementalAspect elementalAspect;

    public bool isHurt = false;

    //Clamp values
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    Animator animator;

    //when an enemy is killed
    public static event Action<Enemy> OnEnemyDie;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetClampValues(Vector3 topRightCorner, Vector3 bottomLeftCorner)
    {
        xMin = bottomLeftCorner.x;
        xMax = topRightCorner.x;
        yMin = bottomLeftCorner.y;
        yMax = topRightCorner.y;
    }

    public void Hurt(int damage)
    {
        health -= damage;

        isHurt = true;
        animator.SetBool("isHurt", true);

        if (health <= 0)
        {
            animator.SetBool("isDead", true);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        OnEnemyDie?.Invoke(this);
    }

    public void endHurt()
    {
        isHurt = false;
        animator.SetBool("isHurt", false);
    }
}
