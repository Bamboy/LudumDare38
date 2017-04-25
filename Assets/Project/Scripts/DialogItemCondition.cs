using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] [CreateAssetMenu(menuName = "Dialog/Item Condition")]
public class DialogItemCondition : DialogCondition
{
	public string RequiredItem = "";

	public override bool Evaluate( DialogSequence caller )
	{
		return Inventory.HasItem( RequiredItem );
	}

	public override void OnSuccess( DialogSequence caller )
	{
		caller.blockIndex = jumpToBlockIndex;
		DisplayString.singleton.blockChanged = true;
		//DisplayString.singleton.charBreak = true;
		//DisplayString.singleton.charIndex = 0;
	}
}