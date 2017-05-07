using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private static PlayerController instance;
	public static PlayerController singleton { get{ return instance; } }

	public AudioClip genericInteract;
	public float gravityConstant = 6.674f;
	[HideInInspector]
	public CircleCollider2D col;

	public float moveForce = 1f;
	public float fuelForce = 1f;
	public float maxFuelTime = 0.5f;
	public Vector2 velocity = Vector2.zero;
	public float maxAirborneTime;
	public bool grounded;
	//public float takeOffOffset = 0.1f;
	public float takeOffForce = 2f;
	public float gravityMuliplier = 0.4f;

	public float groundDrag = 1f;
	public float airborneDrag = 0.1f;

	private float airTime = 0f;
	public static float currentFuel;
	public static bool fuelDepleted;

	private int closestPlanetIndex = 0;
	public Planet closest { get{ return Planet.planets[closestPlanetIndex]; } }
	private CircleCollider2D closestCollider { get{ return Planet.planets[closestPlanetIndex].col; } }
	private bool takeOff = false;

	public float takeOffGravityLerpTime = 0.2f;
	private float takeOffTime;

	private bool doingFuelForce
	{
		get{ return Input.GetButtonDown("Jump") && fuelDepleted == false; }
	}

	void Awake()
	{
		instance = this;
		col = GetComponent<CircleCollider2D>();
	}

	void Update () 
	{
		//grounded = CheckGrounded();
		if( grounded )
		{
			Vector3 force = Vector3.zero;
			if( Mathf.Abs(Input.GetAxis("Horizontal")) > 0.05f )
			{
				force = Input.GetAxis("Horizontal") * transform.right * moveForce * Time.deltaTime;// * movePenalty;
			}
			velocity += new Vector2( force.x, force.y );

			//Recharge fuel
			currentFuel = Mathf.Clamp( currentFuel + (Time.deltaTime * 2f), 0f, maxFuelTime );
			if( fuelDepleted && currentFuel >= maxFuelTime )
				fuelDepleted = false;

			airTime = 0f;
		}
		else
		{
			/*
			airTime += Time.deltaTime;
			if( airTime >= maxAirborneTime )
			{
				Vector3 dir = VectorExtras.Direction(transform.position, closest.transform.position);

				velocity += new Vector2(dir.x, dir.y) * airborneForcePenalty;
			} */
			if( fuelDepleted == false )
			{
				if( currentFuel >= 0f )
				{
					
					Vector3 force = Vector3.zero;
					if( Mathf.Abs(Input.GetAxis("Horizontal")) > 0.05f )
					{
						force = Input.GetAxis("Horizontal") * transform.right * (fuelForce / 2f) * Time.deltaTime;


						currentFuel -= Time.deltaTime;
						if( currentFuel <= 0f )
						{
							fuelDepleted = true;
						}
					}

					velocity += new Vector2( force.x, force.y );


				}
			}
		}

		if( Input.GetButton("Jump") )
		{
			if( fuelDepleted )
			{
				//TODO make smoke or something here
			}
			else
			{
				if( currentFuel >= 0f )
				{
					
					currentFuel -= Time.deltaTime;
					Vector3 force = transform.up * fuelForce * Time.deltaTime;
					velocity += new Vector2( force.x, force.y );

					if( grounded == true ) //Takeoff!
					{
						//transform.position += transform.up * takeOffOffset;

						force = transform.up * takeOffForce;
						velocity += new Vector2( force.x, force.y );
						takeOff = true;
						grounded = false;
						takeOffTime = Time.time;
					}
					

					if( currentFuel <= 0f )
					{
						fuelDepleted = true;
					}
				}
			}
		}
			
		PlanetForces();

		Move();

		FacePlanet();

		TryInteract();

		takeOff = false;
	}


	void Move()
	{

		velocity *= grounded ? groundDrag : airborneDrag;

		Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 newPosition = playerPos + velocity;


		/*
		//move the player
		transform.position = new Vector3( newPosition.x, newPosition.y, 0f );

		grounded = CheckGrounded();
		if( grounded ) //See if our new position intersects a planet
		{
			newPosition = closest.GetPlayerSurfacePosition();
			transform.position = new Vector3( newPosition.x, newPosition.y, 0f );

			if( doingFuelForce == false )
				velocity = Vector2.zero;
		}
		*/


		if( grounded )
		{
			//move along planet surface
			newPosition = closest.GetPlayerSurfacePosition( newPosition );
		}
		else
		{
			//Check to see if we will collide with a planet by moving.
			RaycastHit2D data = Physics2D.CircleCast( playerPos, col.radius, VectorExtras.Direction(playerPos, newPosition), velocity.magnitude, Planet.layer );

			if( data && takeOff == false ) //Did we hit something?
			{
				//newPosition = ;
				newPosition = closest.GetPlayerSurfacePosition( data.centroid );

				if( doingFuelForce == false )
					velocity = Vector2.zero;

				grounded = true;
			}
		}
		transform.position = new Vector3( newPosition.x, newPosition.y, 0f );
	}






	bool CheckGrounded()
	{
		Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
		return Physics2D.CircleCast( playerPos, col.radius, VectorExtras.Direction(playerPos, new Vector2(closest.transform.position.x, closest.transform.position.y)), 0.0f, Planet.layer);
	}






	void PlanetForces()
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
				closestPlanetIndex = i;
			}

			forces.Add( Planet.planets[i].GravityForce() );
		}

		//

		if( grounded == false ) //Dont apply gravity if we are grounded
		{
			//if( doingFuelForce )
			//	forces[closestPlanetIndex] *= 0.4f; //Reduce gravity of nearest planet to help us escape it

			float gravLerp = Mathf.Lerp(0f, gravityMuliplier, VectorExtras.ReverseLerp(Time.time, takeOffTime, takeOffTime + takeOffGravityLerpTime));;

			foreach (Vector2 force in forces) 
			{
				velocity += force * gravLerp * Time.deltaTime;
			}
		}
	}


	void FacePlanet()
	{
		Vector3 dirToPlayer = VectorExtras.Direction(closest.transform.position, transform.position);
		transform.rotation = Quaternion.LookRotation( Vector3.forward, dirToPlayer); //Rotate so our down is always toward the closest planet
	}

	void OnDrawGizmos()
	{
		if( Application.isPlaying )
		{
			Gizmos.color = Color.red;

			Gizmos.DrawRay(transform.position, new Vector3( velocity.x, velocity.y, 0f ));

		}
	}


	[HideInInspector]
	public bool interacting = false;
	void TryInteract()
	{
		if( Input.GetButtonDown("Use") && interacting == false )
		{
			//Look for things to interact with

			RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), col.radius, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("Interactable") );

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
}
