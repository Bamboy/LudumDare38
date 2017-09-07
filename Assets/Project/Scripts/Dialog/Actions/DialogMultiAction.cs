using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialog/Multi-Action")]
public class DialogMultiAction : DialogAction
{

	public List<DialogAction> actions;

	public override void DoAction( DialogSequence caller )
	{
		for (int i = 0; i < actions.Count; i++) 
		{
			actions[i].DoAction( caller );
		}
	}

}
