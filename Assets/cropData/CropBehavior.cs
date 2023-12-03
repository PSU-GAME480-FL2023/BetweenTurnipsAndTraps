using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehavior : MonoBehaviour
{
    //Global Prefab settings
    public string crop;
    public int daysToGrow;
    public Sprite[] growProgressSprites;
    public Sprite readyToHarvestSprite;
    public int purchasePrice;
    public int sellPrice;
    public Sprite onHarvestSprite;

    //Individual instance settings
    private int plantDay;
    private int daysSinceLastWatered;
    public SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
