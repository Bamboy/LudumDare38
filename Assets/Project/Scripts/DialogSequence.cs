using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSequence : Interactable 
{

	[TextArea]
	public List<string> dialog = new List<string>();
	//TODO update dialog array when game state changes

	private AudioSource src;
	void Start()
	{
		src = GetComponent<AudioSource>();
		if( src == null )
		{
			src = gameObject.AddComponent<AudioSource>();
			src.playOnAwake = false;
			src.volume = 0.6f;
		}
		if( src.clip == null )
			src.clip = Player.singleton.genericInteract;
	}
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
			if( src != null )
				src.Play();
			DisplayString.StartDialog( this );
		}

	}


}
