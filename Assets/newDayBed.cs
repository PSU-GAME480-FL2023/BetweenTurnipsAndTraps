using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newDayBed : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Jo")
        {
            GameManager.instance.curDay++;
        }
    }
}
