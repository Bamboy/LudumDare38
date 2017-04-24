using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class DialogCondition : DialogAction
{
	public int jumpToIndex = 0;
	public bool SwitchDialogBlockOnSuccess = true;
	public override void DoAction( DialogSequence caller )
	{
		bool state = Evaluate( caller );
		if( SwitchDialogBlockOnSuccess )
		{
			if( state )
			{
				OnSuccess( caller );
			}
		}
		else
		{
			if( state == false )
			{
				OnSuccess( caller );
			}
		}
	}
	public abstract bool Evaluate( DialogSequence caller );
	public abstract void OnSuccess( DialogSequence caller );
	//public abstract void OnFailure( DialogSequence caller );
}

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
		caller.blockIndex = jumpToIndex;
	}
}