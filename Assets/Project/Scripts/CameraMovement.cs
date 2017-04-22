using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
	public Player player;
	public float rotateSpeed;
	public float moveSpeed;

	void Start () 
	{
		
	}

	void Update()
	{



	}
	Vector3 lastPos = Vector3.one;
	void LateUpdate () 
	{

		transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, Time.deltaTime * rotateSpeed);

		Vector3 target = Vector3.MoveTowards( lastPos, player.transform.position + new Vector3(0,0,-5), Time.deltaTime * moveSpeed );
		//Keep the camera from sinking into the planet

		lastPos = MinMovePosTowardTarget(player.closestCollider.transform.position, target, player.closestCollider.radius); 
		transform.position = lastPos;



	}

	public static Vector3 MinMovePosTowardTarget( Vector3 origin, Vector3 target, float minDistance )
	{
		float distanceToTarget = Vector3.Distance( origin, target );
		if( distanceToTarget < minDistance )
			return VectorExtras.OffsetPosInPointDirection( origin, target, minDistance );
		else
			return target;
	}
}
