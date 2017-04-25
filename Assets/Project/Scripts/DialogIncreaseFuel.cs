using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] [CreateAssetMenu(menuName = "Dialog/Increase Fuel")]
public class DialogIncreaseFuel : DialogAction
{
	public float addedFuelTime = 0.5f;
	//public string removeItem = "";

	public override void DoAction( DialogSequence caller )
	{

		Player.singleton.maxFuelTime += addedFuelTime;
		//Inventory.TakeItem( removeItem );

		caller.blockIndex++;
	}
}