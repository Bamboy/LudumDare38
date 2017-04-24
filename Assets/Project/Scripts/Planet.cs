using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Planet : MonoBehaviour 
{
	public static List<Planet> planets;


	public string displayName = "Unnamed";
	public CircleCollider2D col;
	public float radius{ get{ return col.radius; } }
	public float gravityForce { get{ return col.radius * 2f; } }
	void Awake() 
	{
		if( planets == null )
			planets = new List<Planet>();
		
		planets.Add( this );
	}

	public float DistanceToSurface()
	{
		return Vector3.Distance(transform.position, Player.singleton.transform.position) - radius;
	}
	public Vector2 GravityForce()
	{
		float dist = DistanceToSurface(); 

		Vector2 playerPos = new Vector2( Player.singleton.transform.position.x, Player.singleton.transform.position.y );

		Vector2 thisPos = new Vector2( transform.position.x, transform.position.y );

		return ((Player.radius * radius) / (dist * dist) * 6.674f) * VectorExtras.Direction(playerPos, thisPos); //6.674 gravitational constant
	}


}
