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
		Debug.Log("Give");
		Inventory.GiveItem( giveItem );

		switch ( giveItem ) 
		{
		case "Brush":
			caller.blockIndex++;
			return;
		default:
			return;
		}
	}

}

[System.Serializable][CreateAssetMenu(menuName = "Dialog/TakeItem")]
public class DialogTakeItem : DialogAction
{
	[Tooltip("The item that will be taken from the player")]
	public string takeItem;

	public override void DoAction( DialogSequence caller )
	{
		Debug.Log("Take");
		Inventory.TakeItem( takeItem );


	}
}

