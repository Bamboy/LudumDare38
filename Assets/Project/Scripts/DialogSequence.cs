using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSequence : Interactable 
{

	[TextArea]
	public List<string> dialog = new List<string>();
	//TODO update dialog array when game state changes


	public bool WaitForRelease( KeyCode code = KeyCode.E )
	{
		if( Input.GetKeyUp( code ) )
			return true;
		else
			return false;
	}



	public override void Interact()
	{
		base.Interact();
		if( Input.GetButtonDown("Use") && DisplayString.dialogIsOpen == false )
		{
			DisplayString.StartDialog( this );
		}

	}


}
