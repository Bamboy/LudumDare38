using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogSequence : Interactable 
{

	public List<DialogCondition> dialogConditions
	{
		get{ return dialogObjects[blockIndex].dialogConditions; }
	}
	public List<DialogAction> dialogActions
	{
		get{ return dialogObjects[blockIndex].dialogActions; }
	}


	public List<string> dialog
	{
		get{ return dialogObjects[blockIndex].dialog; }
	}

	private int bIndex = 0;

	public int blockIndex
	{
		get{ return bIndex; }
		set{ 

			int newValue = Mathf.Clamp( value, 0, dialogObjects.Count - 1 ); 
			if( newValue != bIndex )
			{
				bIndex = newValue;
				DisplayString.dialogIndex = 0;
			}
		}
	}
	[SerializeField]
	public List< DialogBlock > dialogObjects = new List< DialogBlock >();


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
			src.clip = PlayerController.singleton.genericInteract;


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
