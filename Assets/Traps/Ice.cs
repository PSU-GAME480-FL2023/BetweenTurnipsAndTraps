using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    //When player is in collider for ice
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Change Jo's controls to more floaty controls with less drag
        if (collider.tag == "Jo")
        {
            Debug.Log("Ice On");
            //Tell Jo to switch to ice controls
            GameObject.FindWithTag("Jo").GetComponent<JoController>().SetOnIce(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //Change Jo's controls to more floaty controls with less drag
        if (collider.tag == "Jo")
        {
            Debug.Log("Ice Off");
            //Tell Jo to switch to ice controls
            GameObject.FindWithTag("Jo").GetComponent<JoController>().SetOnIce(false);
        }
    }
}
