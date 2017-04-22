using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private static Player instance;
	public static Player singleton{ get{ return instance; } }
	public float maxGravDist = 4.0f;
	public float maxGravity = 35.0f;

	[Tooltip("Distance at which drag will be at its min")]
	public float minDragDistance = 5f;
	public float minDrag = 0.2f;
	public float maxDrag = 0.8f;
		

	GameObject[] planets;
	GameObject closest = null;
	[HideInInspector]
	public CircleCollider2D closestCollider = null;
	[HideInInspector]
	public Transform cameraTarget;
	[HideInInspector]
	public Rigidbody2D rb;

	void Awake()
	{
		if( instance == null )
			instance = this;
	}
	void Start () 
	{
		planets = GameObject.FindGameObjectsWithTag("Planet");
		rb = GetComponent<Rigidbody2D>();
		cameraTarget = transform.FindChild("CameraTarget");
	}

	void Update()
	{
		if( closest != null )
		{
			Vector3 dirToPlayer = VectorExtras.Direction(closest.transform.position, transform.position);
			transform.rotation = Quaternion.LookRotation( Vector3.forward, dirToPlayer); //Rotate so our down is always toward the closest planet
		
			float distToSurface = Vector3.Distance( closest.transform.position, transform.position ) - closestCollider.radius;
			rb.drag = Mathf.Lerp( maxDrag, minDrag, VectorExtras.ReverseLerp(distToSurface, 0f, minDragDistance) );
		}
		else
		{
			rb.drag = minDrag;
		}

	}

	void FixedUpdate () 
	{



		if( Input.GetKey( KeyCode.D ) )
		{
			Vector3 force = transform.right;
			rb.AddForce( new Vector2( force.x, force.y ) );
		}
		else if( Input.GetKey( KeyCode.A ) )
		{
			Vector3 force = -transform.right;
			rb.AddForce( new Vector2( force.x, force.y ) );
		}


		float closeDist = Mathf.Infinity;
		foreach(GameObject planet in planets) //Simulate Gravity
		{
			float dist = Vector3.Distance(planet.transform.position, transform.position);

			if( dist < closeDist )
			{
				closeDist = dist;
				closest = planet;
				closestCollider = closest.GetComponent<CircleCollider2D>();
			}

			if (dist <= maxGravDist)
			{
				Vector3 v = planet.transform.position - transform.position;
				rb.AddForce(v.normalized * (1.0f - dist / maxGravDist) * maxGravity);
			}
		}


	}

	void OnDrawGizmos()
	{
		if( !Application.isPlaying )
			return;
		
		foreach (GameObject planet in planets) 
		{
			Gizmos.DrawWireSphere( planet.transform.position, maxGravDist );
		}
	}
}
