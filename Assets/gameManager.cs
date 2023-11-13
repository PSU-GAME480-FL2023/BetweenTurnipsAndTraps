using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int curDay = 1;
    public int money;
    public int cropInventory;
    public CropData selectedCropToPlant;
    
	
    // Singleton
    public static GameManager instance;
    void Awake ()
    {
        // Initialize the singleton.
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
	    void OnEnable ()
    {
        Crop.onPlantCrop += OnPlantCrop;
        Crop.onHarvestCrop += OnHarvestCrop;
    }
    void OnDisable ()
    {
        Crop.onPlantCrop -= OnPlantCrop;
        Crop.onHarvestCrop -= OnHarvestCrop;
    }
    // Called when a crop has been planted.
    // Listening to the Crop.onPlantCrop event.
    public void OnPlantCrop (CropData cop)
    {
    }
    // Called when a crop has been harvested.
    // Listening to the Crop.onCropHarvest event.
    public void OnHarvestCrop (CropData crop)
    {
    }
    // Called when we want to purchase a crop.
    public void PurchaseCrop (CropData crop)
    {
    }
    // Do we have enough crops to plant?
    public bool CanPlantCrop ()
    {
    }
    // Called when the buy crop button is pressed.
    public void OnBuyCropButton (CropData crop)
    {
    }
	
}