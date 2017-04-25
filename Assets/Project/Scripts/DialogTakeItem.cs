using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable][CreateAssetMenu(menuName = "Dialog/TakeItem")]
public class DialogTakeItem : DialogAction
{
	[Tooltip("The item that will be taken from the player")]
	public string takeItem;

	public override void DoAction( DialogSequence caller )
	{
		Inventory.TakeItem( takeItem );
	}
}
