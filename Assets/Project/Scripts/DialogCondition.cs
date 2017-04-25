using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class DialogCondition : DialogAction
{
	public int jumpToBlockIndex = 0;
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
