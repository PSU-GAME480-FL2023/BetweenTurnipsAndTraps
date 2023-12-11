using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canBuy : MonoBehaviour
{

	public int buyAt;
	public CropData purchasePrice;
	public int tempMoney;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	public bool canBuyIf()
	{
	if(buyAt <= GameManager.instance.totalPlant)
	{
		return true;
	
	}
	
	else
	{
	
		return false;
		
	}
}

	/*public void purchase()
	{
		if(canBuyIf = "true" && purchasePrice >= GameManager.instance.money)
		// give player the item
		
		tempMoney = GameManager.instance.money - purchasePrice;
		
		GameManager.instance.money = tempMoney;
	}*/
}
