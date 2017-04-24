using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable] [CreateAssetMenu(menuName = "Dialog/Block")]
public class DialogBlock : ScriptableObject
{
	public List<DialogCondition> dialogConditions = new List<DialogCondition>();

	[SerializeField]
	public List<DialogAction> dialogActions = new List<DialogAction>();

	[TextArea(2, 10)]
	public List<string> dialog = new List<string>();
}
