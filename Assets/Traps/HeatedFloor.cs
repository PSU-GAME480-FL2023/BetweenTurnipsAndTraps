using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeatedFloor : MonoBehaviour
{
    //Amount of time the floor will stay cool
    public float coolTime;
    //Amount of time the floor will stay heated
    public float heatTime;

    public float knockback;

    public TileBase coolFloor;
    public TileBase heatedFloor;

    private bool heated;

    // Start is called before the first frame update
    void Start()
    {
        heated = false;

        StartCoroutine(HeatLoop());
    }

    IEnumerator HeatLoop()
    {
        while (true)
        {
            //Start with cool tilemaps
            //Turn hurt off
            heated = false;
            //Change appearance
            ReplaceTiles(coolFloor);

            //Wait for coolTime
            yield return new WaitForSeconds(coolTime);

            //Go to heated tilemaps
            //Turn hurt on
            heated = true;
            //Change appearance
            ReplaceTiles(heatedFloor);

            //Wait for heatTime
            yield return new WaitForSeconds(heatTime);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Jo" && heated == true)
        {
            var Jo = other.gameObject.GetComponent<JoController>();

            //Get player Rigidbody
            Rigidbody2D joRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

            Vector2 knockbackDirection = (Vector2)joRigidbody.transform.position.normalized;

            joRigidbody.AddForce(knockback * -knockbackDirection, ForceMode2D.Impulse);

            //Jo.hurtJo(knockbackVector, damage);
        }
    }

    private void ReplaceTiles(TileBase newTile)
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                tilemap.SetTile(position, newTile);
            }
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
