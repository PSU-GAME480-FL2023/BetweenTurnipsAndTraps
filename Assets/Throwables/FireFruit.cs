using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireFruit : ThrowableItem
{
    public override void ActivateEffect()
    {
        //Create a circle collider and get every collider within that circle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, effectRadius);

        //Go through each object
        foreach (Collider2D collider in colliders)
        {
            //If it is an enemy, damage it
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<Enemy>().Hurt(damage);
            }

            //If it is a trap
            else if (collider.tag == "Trap")
            {
                //Get trap tilemap
                Tilemap trapTilemap = collider.GetComponent<Tilemap>();

                //Get bounds of it
                BoundsInt bounds = trapTilemap.cellBounds;

                //Draw the effect collider again, and get every tile in the tilemap that falls under it
                foreach (var trapPosition in bounds.allPositionsWithin)
                {
                    if (trapTilemap.HasTile(trapPosition))
                    {
                        //Tilemap coordinates in world coordinates
                        Vector3 trapPositionCenter = trapTilemap.GetCellCenterWorld(trapPosition);

                        //If the tile is within the effect radius, delete it
                        float distance = Vector3.Distance(transform.position, trapPositionCenter);

                        if (distance <= effectRadius)
                        {
                            //Check which trap it is and call specific script
                            //IceSpikes
                            if (collider.GetComponent<IceSpikes>() != null)
                            {
                                //Melt ice spikes
                                collider.GetComponent<IceSpikes>().Melt(trapPosition);
                            }

                            //IceWall
                            if (collider.GetComponent<IceWall>() != null)
                            {
                                //Melt ice spikes
                                collider.GetComponent<IceWall>().Melt(trapPosition);
                            }
                        }
                    }
                }
            }
        }

        //Destroy Fruit
        Destroy(this.gameObject);
    }
}
