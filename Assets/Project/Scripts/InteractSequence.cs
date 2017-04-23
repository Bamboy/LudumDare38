using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractSequence : Interactable 
{
	List< Func<bool> > sequence = new List< Func<bool> >();
	internal void AddStep( Func<bool> act )
	{
		sequence.Add( act );
	}

	public override void Interact ()
	{
		base.Interact();

		if( sequence != null )
		{
			Player.interacting = true;
			StartCoroutine( Sequencer() );
		}
	}

	IEnumerator Sequencer()
	{
		for (int step = 0; step < sequence.Count; step++) 
		{
			while( sequence[step]() == false ) //Yield until the function returns true
				yield return null;
			
		}

		Player.interacting = false;
	}

}
