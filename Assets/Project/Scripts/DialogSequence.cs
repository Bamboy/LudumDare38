using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSequence : InteractSequence 
{

	[TextArea]
	public List<string> dialog = new List<string>();
	//TODO update dialog array when game state changes

	void Start () 
	{
		foreach (string item in dialog) 
		{
			base.AddStep( delegate() { return WaitForRelease(); } );
			base.AddStep( delegate() { return DisplayDialog( item ); } );

		}
	}


	public bool WaitForRelease( KeyCode code = KeyCode.E )
	{
		if( Input.GetKeyUp( code ) )
			return true;
		else
			return false;
	}



	public bool DisplayDialog( string text )
	{

		if( Input.GetButtonDown("Use") && DisplayString.dialogIsOpen == false )
		{
			DisplayString.StartDialog( this );

			return true;
		}
		else
			return false;

	}

}
