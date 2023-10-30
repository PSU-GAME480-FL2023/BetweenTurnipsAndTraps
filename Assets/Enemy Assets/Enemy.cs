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

    //Clamp values
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
