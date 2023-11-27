using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterFruit : ThrowableItem
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
                            //Fire
                            if (collider.GetComponent<Fire>() != null)
                            {
                                //Put out fire
                                collider.GetComponent<Fire>().PutOut(trapPosition);
                            }

                            //Lava (Pit)
                            else if (collider.GetComponent<Lava>() != null)
                            {
                                //Freeze pit over
                                collider.GetComponent<Lava>().Harden(trapPosition);
                            }

                            //Heated Floor
                            else if (collider.GetComponent<HeatedFloor>() != null)
                            {
                                Debug.Log(trapPosition);
                                //Remove tile from heated floor
                                collider.GetComponent<HeatedFloor>().DeleteTile(trapPosition);
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
