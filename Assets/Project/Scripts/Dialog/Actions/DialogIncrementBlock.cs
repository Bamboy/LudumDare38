using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialog/Increment Dialog Block")]
public class DialogIncrementBlock : DialogAction 
{
	public override void DoAction( DialogSequence caller )
	{
		caller.blockIndex++;
	}
}
