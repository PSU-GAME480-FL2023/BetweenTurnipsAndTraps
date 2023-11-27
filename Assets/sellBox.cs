using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sellBox : MonoBehaviour
{

	private CropData curCrop;
	private CropData sellPrice;
	private CropData purchasePrice;
	private int tempCash;
	
	// Start is called before the first frame update
    void Start()
    {
        
    }
	
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Crop")
        {
				Destroy(gameObject);
				tempCash = GameManager.instance.money + sellPrice;
				GameManager.instance.money = tempCash;
			
			
        }
    }
}
