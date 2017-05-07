using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class that does something during a dialog sequence
[System.Serializable]
public abstract class DialogAction : ScriptableObject
{
	//new	public string name = "Action";

	public abstract void DoAction( DialogSequence caller );
}
