using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public GameObject obstical;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Throwable")
        {
            //Put Explosion Here?
            Destroy(obstical);
        }
    }
}
