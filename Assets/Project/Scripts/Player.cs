using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Old script, do not use

public class Player : MonoBehaviour 
{
	private static Player instance;
	public static Player singleton{ get{ return instance; } }

	public AudioClip genericInteract;

	public float maxGravDist = 4.0f;
	public float maxGravity = 35.0f;

	[Tooltip("Distance at which drag will be at its min")]
	public float minDragDistance = 5f;
	public float minDrag = 0.2f;
	public float maxDrag = 1f;

	public float maxFuelTime = 12f;
	public float fuel = 0f;
	public static bool fuelDepleted = false;

	public static bool grounded;

	public float fuelForce = 10f;
	public float moveForce = 1f;

	public float maxAirborneTime{ get{ return maxFuelTime * 1.5f + 10f; } }
	public float airborneForcePenalty = 0.1f;
	public float airTime = 0f;

	public static float radius = 0.3f;


	//[HideInInspector]
	//public CircleCollider2D closestCollider = null;
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
		rb = GetComponent<Rigidbody2D>();
		cameraTarget = transform.FindChild("CameraTarget");
	}
	void Update()
	{

	}
		
	/*
	private int closeIndex = 0;
	private float movePenalty = 0.65f;
	private Planet closest{ get{ return Planet.planets[closeIndex]; } }
	void FixedUpdate () 
	{
		float closeDist = Mathf.Infinity;

		List<Vector2> forces = new List<Vector2>();
		List<float> distances = new List<float>();
		for (int i = 0; i < Planet.planets.Count; i++) 
		{

			float dist = Vector3.Distance(Planet.planets[i].transform.position, transform.position);
			distances.Add( dist );

			if( dist < closeDist )
			{
				closeDist = dist;
				closeIndex = i;
				closestCollider = closest.GetComponent<CircleCollider2D>();
			}

			forces.Add( Planet.planets[i].GravityForce() );
		}


		float distToSurface = 0f;
		if( closestCollider != null )
		{
			distToSurface = Planet.planets[closeIndex].DistanceToSurface();

			float dragRange = Planet.planets[closeIndex].radius * 0.6f;

			//Vector3 planetSurface = VectorExtras.OffsetPosInPointDirection( Planet.planets[closeIndex].transform.position, transform.position, Planet.planets[closeIndex].radius );
			//Vector3 dragEnds = VectorExtras.OffsetPosInPointDirection( Planet.planets[closeIndex].transform.position, transform.position, Planet.planets[closeIndex].radius + dragRange );

			//Calculate T...
			float progress = VectorExtras.ReverseLerp( Vector3.Distance( Planet.planets[closeIndex].transform.position, transform.position ), 
				Planet.planets[closeIndex].radius, Planet.planets[closeIndex].radius + dragRange );

			rb.drag = Mathf.Lerp( maxDrag, minDrag, progress );

			//rb.drag = Mathf.Lerp( 

		}

		Vector2 forceSum = new Vector2();
		for (int i = 0; i < Planet.planets.Count; i++) 
		{
			//forceSum += Vector2.ClampMagnitude( forces[i], 5f );
			rb.AddForce( Vector2.ClampMagnitude( forces[i], 4f ) );
		}

		Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
		grounded = Physics2D.CircleCast( playerPos, 0.16f, VectorExtras.Direction(playerPos, new Vector2(closest.transform.position.x, closest.transform.position.y)), 0.0f, LayerMask.NameToLayer("Planet") );




		if( grounded )
		{
			//More drag if we are on the ground
			//rb.drag = Mathf.Lerp( maxDrag, minDrag, VectorExtras.ReverseLerp(distToSurface, 0f, minDragDistance) );
			rb.velocity = rb.velocity * 0.98f;

			Vector3 force = Vector3.zero;
			if( Mathf.Abs(Input.GetAxis("Horizontal")) > 0.05f )
				force = Input.GetAxis("Horizontal") * transform.right * moveForce * movePenalty;


			rb.AddForce( new Vector2( force.x, force.y ) );
		}
		else
		{
			//rb.drag = Mathf.Lerp( maxDrag, minDrag, VectorExtras.ReverseLerp(distToSurface, 0f, minDragDistance) );
			airTime += Time.fixedDeltaTime;
			if( airTime >= maxAirborneTime )
			{
				Vector3 dir = VectorExtras.Direction(transform.position, closest.transform.position);
				//forceSum += new Vector2(dir.x, dir.y) * airborneForcePenalty;

				rb.AddForce(new Vector2(dir.x, dir.y) * airborneForcePenalty);
			}

			Vector3 force = Vector3.zero;
			if( Mathf.Abs(Input.GetAxis("Horizontal")) > 0.05f )
				force = Input.GetAxis("Horizontal") * transform.right * moveForce;

			rb.AddForce( new Vector2( force.x, force.y ) );
		}
			




		if( Input.GetButton("Jump") )
		{
			if( fuelDepleted )
			{
				//TODO make smoke or something here
			}
			else
			{
				if( fuel >= 0f )
				{
					fuel -= Time.fixedDeltaTime;
					Vector3 force = transform.up * fuelForce;
					rb.AddForce( new Vector2( force.x, force.y ) );

					if( fuel <= 0f )
					{
						fuelDepleted = true;
					}
				}
			}
		}



	} */

	public static bool interacting = false;
	void OnCollisionStay2D( Collision2D collision ) //Recharge fuel while on the ground.
	{
		//fuel = Mathf.Clamp( fuel + (Time.fixedDeltaTime * 2f), 0f, maxFuelTime );
		//if( fuelDepleted && fuel >= maxFuelTime )
		//	fuelDepleted = false;

		//airTime = 0f;

		if( Input.GetButtonDown("Use") && interacting == false )
		{
			//Look for things to interact with

			RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), 0.3f, Vector2.zero );

			foreach (var item in hits)
			{
				Interactable script = item.transform.GetComponent<Interactable>();
				if( script != null )
				{
					script.Interact();
					break;
				}
			}


		}
	}


	private static bool dialogOpen = false;
	public static bool DialogOpen
	{
		get{ return dialogOpen; }
		set{
			dialogOpen = value;
		}
	}















}
