using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightFruit : ConsumableItem
{
    public float flightTime;

    public override void ActivateEffect(GameObject Jo)
    {
        StartCoroutine(ActivateFlight(Jo));
    }

    private IEnumerator ActivateFlight(GameObject Jo)
    {
        //Activate flight status
        Jo.GetComponent<JoController>().SetFlying(true);
        //Activate ice controls
        Jo.GetComponent<JoController>().SetOnIce(true);

        //Leave effect active for flightTime
        yield return new WaitForSeconds(flightTime);

        //Deactivate flight status
        Jo.GetComponent<JoController>().SetFlying(false);
        //Deactivate ice controls
        Jo.GetComponent<JoController>().SetOnIce(false);

        //Destroy instance of fruit
        Destroy(this.gameObject);
    }
}
