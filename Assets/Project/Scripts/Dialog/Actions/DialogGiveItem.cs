using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] [CreateAssetMenu(menuName = "Dialog/GiveItem")]
public class DialogGiveItem : DialogAction
{
	[Tooltip("The item that will be given to the player")]
	public string giveItem;

	public override void DoAction( DialogSequence caller )
	{
		Inventory.GiveItem( giveItem );
	}

}