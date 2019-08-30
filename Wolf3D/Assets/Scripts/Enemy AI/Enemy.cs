using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public abstract class Enemy : MonoBehaviour, IDamageable<float>
{
	public abstract float enemyHealth { get; set; }
	public abstract float baseDamage { get; set; }
	public abstract float maxMoveSpeed { get; set; }
	public abstract int ammo { get; set; }
	
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public Animator anim;
	private SpriteRenderer enemySprite;
	public BoxCollider enemyCollider;
	
	private float fieldOfVision = 170;
	
	public float seekingRadius;
	public float seekingDistance;
	public float seekingDelay;
	public float searchDistance;
	public bool alerted = false;
	public bool isHit = false;
	float timer;
	
	public abstract Transform[] points { get; }
	public int currentPoint = 0;
	
	public Seeker seeker;
	public RichAI richAI;
	public AstarPath aStar;
	public Path currentPath;
	public float currentDestination;
	public ABPath abPath;

	public LayerMask layerMask;
	
	public bool reachedEndofPath;
	public bool isPathDone;
	
	[HideInInspector]
	public int idleHash, hitHash, randomHitHash, deathHash, walkHash, attackHash, xInputHash, yInputHash;
	
	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		
		anim = transform.GetComponentInChildren<Animator>();
		enemySprite = transform.GetComponentInChildren<SpriteRenderer>();
		
		richAI.funnelSimplification = true;
		
		idleHash = Animator.StringToHash("Idle");
		hitHash = Animator.StringToHash("Hit");
		randomHitHash = Animator.StringToHash("RandomHit");
		deathHash = Animator.StringToHash("Death");
		walkHash = Animator.StringToHash("Walk");
		attackHash = Animator.StringToHash("Attack");
		xInputHash = Animator.StringToHash("xInput");
		yInputHash = Animator.StringToHash("yInput");
	}
	
	private void Update()
	{
		Vector3 dirToTarget = player.transform.position - transform.position;
		float angle = Vector3.SignedAngle(dirToTarget.normalized, transform.forward, Vector3.up);
		
		if (angle < 0)
		{
			angle += 360f;
		}
		
		Vector2 angleToXY = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
		
		anim.SetFloat("xInput", angleToXY.x);
		anim.SetFloat("yInput", angleToXY.y);
	}
	
	public abstract void Damage(float damageTaken);
	
	public abstract void changeSprite(int num);
	
	private void OnTriggerStay(Collider other)
	{
		bool playerInRange = RaycastToPlayer(other);
		if (playerInRange)
		{
			//Do something...
			alerted = true;
		}
	}
	
	//private void OnTriggerExit(Collider other)
	//{
	//	if (other.gameObject.CompareTag("Player"))
	//	{
	//		alerted = false;
	//	}
	//}
	
	private bool RaycastToPlayer(Collider other)
	{
		Vector3 dirToTarget = other.transform.position - transform.position;
		float angle = Vector3.Angle(dirToTarget.normalized, transform.forward);
		
		if (angle < fieldOfVision - 90)
		{
			RaycastHit raycastHit;
			//Shows a raycast in the editor.
			//Debug.DrawRay(transform.position, dirToTarget, Color.red, 1);
			//The 1 << 2 returns the value of the 3rd bit as a decimal (4). This is because the 'Ignore Raycast' layer is on layer 2 (which is actually the 3rd layer but layers start from 0).
			//So, we need the decimal value of the binary bit position (which is the position of the layer). The '~' inverts the bits, so now we check for everything beside the gameobjects with the 'Ignore Raycast' layer attachted.
			//~(1 << 2)
			if (Physics.Raycast(transform.position, dirToTarget, out raycastHit, Mathf.Infinity, layerMask))
			{
				Debug.DrawRay(transform.position, dirToTarget, Color.yellow, 1);
				//If the raycast hit the player.
				if (raycastHit.transform.gameObject.CompareTag("Player"))
				{
					Debug.DrawRay(transform.position, dirToTarget, Color.green, 1);
					//alerted = true;
					//Path p = ABPath.Construct(transform.position, raycastHit.transform.position);
					//richAI.SetPath(p);
					//currentAction = Action.Attack;
					return true;
				}
			}
		}
		return false;
	}
	
	public void OnPathComplete(Path p)
	{
		if (p.error)
		{
			Debug.Log("Error encountered while calculating path. Here is the error log: " + p.errorLog);
		} else
		{
			currentPath = p;
		}
	}
	
	private void FindNextPoint()
	{
		GoToNextPoint();
	}
	
	public void GoToNextPoint()
	{
		// Returns if no points have been set up
		if (points.Length == 0)
			return;

		//richAI.destination = points[destPoint].position;

		// Set the agent to go to the currently selected destination.
		seeker.StartPath(transform.position, points[currentPoint].position, OnPathComplete);

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		currentPoint = (currentPoint + 1) % points.Length;
	}
	
	private void findNewRandomPoint()
	{
		// Call a RandomPath call like this, assumes that a Seeker is attached to the GameObject

		// The path will be returned when the path is over a specified length (or more accurately when the traversal cost is greater than a specified value).
		// A score of 1000 is approximately equal to the cost of moving one world unit.
		int theGScoreToStopAt = (int) seekingDistance * 1000;

		// Create a path object
		RandomPath path = RandomPath.Construct(transform.position, theGScoreToStopAt);
		// Determines the variation in path length that is allowed
		path.spread = 15000;

		// Start the path and return the result to MyCompleteFunction (which is a function you have to define, the name can of course be changed)
		seeker.StartPath(path, OnPathComplete);
	}
	
	public float nfmod(float a, float b)
	{
		return a - b * Mathf.Floor(a / b);
	}
}
