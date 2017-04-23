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
	public float maxDrag = 1f;

	public static float maxFuelTime = 1f;
	public static float fuel = 0f;
	public static bool fuelDepleted = false;

	public static bool grounded;

	public float fuelForce = 10f;
	public float moveForce = 1f;

	public static void IncreaseFuel()
	{
		maxFuelTime += 1f;
	}


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

		text = GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
	}

	void Update()
	{
		if( closest != null )
		{
			Vector3 dirToPlayer = VectorExtras.Direction(closest.transform.position, transform.position);
			transform.rotation = Quaternion.LookRotation( Vector3.forward, dirToPlayer); //Rotate so our down is always toward the closest planet
		}
		else
		{
			rb.drag = minDrag;
			grounded = false;
		}

	}

	private float movePenalty = 0.65f;

	void FixedUpdate () 
	{
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

		float distToSurface = 0f;
		if( closestCollider != null )
			distToSurface = Vector3.Distance( closest.transform.position, transform.position ) - closestCollider.radius;


		grounded = Physics2D.CircleCast( new Vector2(transform.position.x, transform.position.y), 0.16f, VectorExtras.Direction(transform.position, closest.transform.position), 0f, -LayerMask.NameToLayer("Planet") );

		if( grounded )
		{
			//More drag if we are on the ground
			rb.drag = Mathf.Lerp( maxDrag / 2f, minDrag / 2f, VectorExtras.ReverseLerp(distToSurface, 0f, minDragDistance) );
			rb.velocity = rb.velocity * 0.95f;


			Vector3 force = Vector3.zero;
			if( Input.GetKey( KeyCode.D ) )
				force = transform.right * moveForce * movePenalty;
			else if( Input.GetKey( KeyCode.A ) )
				force = -transform.right * moveForce * movePenalty;


			rb.AddForce( new Vector2( force.x, force.y ) );


			if( rb.velocity.magnitude >= 1.0f )
				rb.velocity = rb.velocity * 0.5f;
			else if( rb.velocity.magnitude <= 0.8f )
				rb.velocity = rb.velocity * 1.05f;
		}
		else
		{
			rb.drag = Mathf.Lerp( maxDrag, minDrag, VectorExtras.ReverseLerp(distToSurface, 0f, minDragDistance) );

			Vector3 force = Vector3.zero;
			if( Input.GetKey( KeyCode.D ) )
				force = transform.right * moveForce;
			else if( Input.GetKey( KeyCode.A ) )
				force = -transform.right * moveForce;

			rb.AddForce( new Vector2( force.x, force.y ) );
		}



		if( Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.W ) )
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



	}

	public static bool interacting = false;
	void OnCollisionStay2D( Collision2D collision ) //Recharge fuel while on the ground.
	{
		fuel = Mathf.Clamp( fuel + (Time.fixedDeltaTime * 2f), 0f, maxFuelTime );
		if( fuelDepleted && fuel >= maxFuelTime )
			fuelDepleted = false;

		if( Input.GetKey(KeyCode.E) && interacting == false )
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
	public static UnityEngine.UI.Text text;
	public static void SetDialogText( string str )
	{
		text.text = str;
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
