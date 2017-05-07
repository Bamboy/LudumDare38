using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialog/Destroy Child")]
public class DialogDestroyGameObject : DialogAction
{
	public string childName = "";
	public override void DoAction( DialogSequence caller )
	{

		List<Transform> children = new List<Transform>();
		foreach (Transform child in caller.transform) 
		{
			if( child.name == childName )
				children.Add( child );
		}

		if( children.Count > 0 )
		{
			GameObject.Destroy( children[ Random.Range(0, children.Count) ].gameObject );
		}

	}

}
