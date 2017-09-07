using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Planet : MonoBehaviour 
{
	public static List<Planet> planets;
	public static LayerMask layer;

	public string displayName = "Unnamed";
	public CircleCollider2D col;
	public float radius{ get{ return col.radius; } }
	public float gravityForce { get{ return col.radius * 2f; } }



	void Awake() 
	{
		if( planets == null )
		{
			planets = new List<Planet>();
			layer = 1 << LayerMask.NameToLayer("Planet");
		}
		
		planets.Add( this );
	}

	public Vector2 GetPlayerSurfacePosition()
	{
		Vector2 output = VectorExtras.OffsetPosInPointDirection(
			new Vector2(transform.position.x, transform.position.y), 
			new Vector2(PlayerController.singleton.transform.position.x, PlayerController.singleton.transform.position.y),
			radius + PlayerController.singleton.col.radius
		);
		return output;
	}
	public Vector2 GetPlayerSurfacePosition( Vector2 forPos )
	{
		Vector2 output = VectorExtras.OffsetPosInPointDirection(
			new Vector2(transform.position.x, transform.position.y), forPos,
			radius + PlayerController.singleton.col.radius
		);
		return output;
	}

	public float DistanceToSurface()
	{
		return Vector3.Distance(transform.position, PlayerController.singleton.transform.position) - radius;
	}

	public Vector2 GravityForce()
	{
		float dist = DistanceToSurface(); 

		Vector2 playerPos = new Vector2( PlayerController.singleton.transform.position.x, PlayerController.singleton.transform.position.y );

		Vector2 thisPos = new Vector2( transform.position.x, transform.position.y );

		return ((PlayerController.singleton.col.radius * radius) / (dist * dist) * PlayerController.singleton.gravityConstant) * VectorExtras.Direction(playerPos, thisPos); //6.674 gravitational constant
	}

	void OnDrawGizmos()
	{
		if( Application.isPlaying )
		{
			Vector2 playerSurf = GetPlayerSurfacePosition();
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere( new Vector3(playerSurf.x, playerSurf.y, 0f), PlayerController.singleton.col.radius );
		}
	}
}
