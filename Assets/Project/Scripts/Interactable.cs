using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour 
{
	public virtual void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}
	public virtual void Interact()
	{
		return;
	}
	
}
