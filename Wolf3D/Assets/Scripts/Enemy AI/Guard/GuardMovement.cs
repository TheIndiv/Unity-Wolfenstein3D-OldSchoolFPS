using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class GuardMovement : MonoBehaviour, IDamageable<float>
{
    GameObject player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    Animator anim;
    SpriteRenderer guardSprite;

    private float timeBetweenAttacks;
    public int attackDamage;

    public float fieldOfVision = 150;

    public GameObject[] spheres;
    public List<Vector3> prevAreas;
    public GameObject attemptSphere;
    public float seekingRadius;
    public float seekingDistance;
    public float seekingDelay;
    public float searchDistance;
    public bool alerted = false;
    float timer;

    public Transform[] points;
    private int destPoint = 0;

    private bool isCoroutineRunning = false;

    private NavMeshTriangulation navMeshAreas;
    public int[] polygons;
    public Vector3[] verticies;

    Seeker seeker;
    RichAI richAI;
    AstarPath aStar;
    Path currentPath;
    float currentDestination;
    ABPath abPath;

	public LayerMask layerMask;
	
	private AnimatorStateInfo currentState;

    public enum Action
    {
        Idle, Attack, Dying, Seeking, Patrolling
    }
    public Action currentAction;

    void Awake()
    {
        richAI = GetComponent<RichAI>();
        seeker = GetComponent<Seeker>();
        aStar = GetComponent<AstarPath>();

        player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth = player.GetComponent<PlayerHealth>;
        //enemyHealth = GetComponenet<EnemyHealth>;
        anim = transform.GetComponentInChildren<Animator>();
        guardSprite = transform.GetComponentInChildren<SpriteRenderer>();

        richAI.funnelSimplification = true;

        //GotoNextPoint();

        //navMeshAreas = NavMesh.CalculateTriangulation();
        //polygons = navMeshAreas.indices;
        //verticies = navMeshAreas.vertices;

        //Mesh mesh = sphere.GetComponent<MeshFilter>().mesh;
        //mesh.Clear();
        //mesh.vertices = verticies;
        //mesh.triangles = polygons;
    }

    private void Start()
    {
        GotoNextPoint();
    }

    public bool reachedEndofPath;
    public bool isPathDone;

    void Update()
    {
        isPathDone = seeker.IsDone();

        Vector3 dirToTarget = player.transform.position - transform.position;
        float angle = Vector3.SignedAngle(dirToTarget, transform.forward, Vector3.up);

        //if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        //{

        if (currentAction == Action.Attack && isPathDone)
        {
            seeker.GetCurrentPath().Error();
            seeker.StartPath(transform.position, player.transform.position, OnPathComplete);
            playWalk(angle);
        }

        if (currentPath != null && !PathUtilities.IsPathPossible(currentPath.path))
        {
            richAI.SearchPath();
            Debug.Log("get back here!");
        }

        if (!richAI.pathPending && richAI.reachedEndOfPath && isPathDone)
        {
            playIdle(angle);
            if (!isCoroutineRunning) {
                if (currentAction == Action.Seeking)
                {
                    StartCoroutine(seekWait(angle));
                    isCoroutineRunning = true;
                } else if (currentAction == Action.Patrolling)
                {
                    if (points[(int) nfmod(destPoint-1, points.Length)].CompareTag("Finish"))
                    {
                        alerted = true;
                        currentAction = Action.Seeking;
                    }
                    else
                    {
                        StartCoroutine(patrollWait(angle));
                        isCoroutineRunning = true;
                    }
                }
            }
        } else
        {
            playWalk(angle);
        }

        //}
        //else 
        //{
        //    nav.enabled = false;
        //}
    }
    
	public void Damage(float damageTaken)
	{
		currentState = anim.GetCurrentAnimatorStateInfo(0);
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

    IEnumerator seekWait(float angle)
    {
        yield return new WaitForSeconds(seekingDelay);
        findNewRandomPoint();
        playWalk(angle);
        isCoroutineRunning = false;
    }

    IEnumerator patrollWait(float angle)
    {
        yield return new WaitForSeconds(seekingDelay);
        seeker.transform.LookAt(points[destPoint].position);
        GotoNextPoint();
        playWalk(angle);
        isCoroutineRunning = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    StopAllCoroutines();
    //    RaycastToPlayer(other);
    //}

    private void OnTriggerStay(Collider other)
    {
        RaycastToPlayer(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (alerted == true)
        {
            currentAction = Action.Seeking;
        }
    }

    private void RaycastToPlayer(Collider other)
    {
        Vector3 dirToTarget = other.transform.position - transform.position;
        float angle = Vector3.Angle(dirToTarget, transform.forward);

        if (angle < fieldOfVision)
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
                    alerted = true;
                    currentAction = Action.Attack;
                }
            }
        }
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        //richAI.destination = points[destPoint].position;

        // Set the agent to go to the currently selected destination.
        seeker.StartPath(transform.position, points[destPoint].position, OnPathComplete);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
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

    //private int i = 0;
    
    //private bool findNewRandomPointOLD()
    //{

    //    //Creates a random point in the sphere which will be checked if it intersects the navmesh in any position.
    //    //Vector3 newDirection = transform.position + (Random.insideUnitSphere * seekingDistance);

    //    Vector3 newDirection = transform.position + GetPointOnUnitCircleCircumference() * seekingDistance;

    //    NavMeshHit navHit;
    //    //attemptSphere.transform.position = newDirection;
    //    //attemptSphere.GetComponent<Renderer>().material.color = Color.yellow;
    //    //Debug.DrawLine(newDirection, newDirection + new Vector3(searchDistance, 0, searchDistance), Color.black, seekingDelay);

    //    if (NavMesh.SamplePosition(newDirection, out navHit, searchDistance, 1 << NavMesh.GetAreaFromName("Walkable")))
    //    {
    //        //attemptSphere.transform.position = newDirection;
    //        //attemptSphere.GetComponent<Renderer>().material.color = Color.white;

    //        bool pointIntersecting = false;
    //        foreach (Vector3 point in prevAreas)
    //        {
    //            if (PointInsideSphere(navHit.position, point, seekingRadius))
    //            {
    //                pointIntersecting = true;
    //            }
    //        }

    //        if (!pointIntersecting)
    //        {
    //            if (i < 2)
    //            {
    //                prevAreas.Add(navHit.position);
    //            } else
    //            {
    //                prevAreas[i % 2] = navHit.position;
    //            }
    //            nav.SetDestination(navHit.position);
    //            //spheres[i % 2].transform.position = navHit.position;
    //            i++;
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public Vector3 GetPointOnUnitCircleCircumference()
    //{
    //    float randomAngle = Random.Range(0f, Mathf.PI * 2f);
    //    return new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)).normalized;
    //}

    //private bool PointInsideSphere(Vector3 point, Vector3 center, float radius)
    //{
    //    return Vector3.Distance(point, center) < radius;
    //}

    //private Vector3 GetRandomLocation()
    //{
    //    NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

    //    //Subtract 3 becuase each triangle contains 3 verticies.
    //    int maxIndices = navMeshData.indices.Length - 3;
    //    // Pick the first indice of a random triangle in the nav mesh. Spawn on Verticies.
    //    Vector3 point = navMeshData.vertices[navMeshData.indices[Random.Range(0, maxIndices)]];
    //    Vector3 firstVertexPosition = point;
    //    Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[Random.Range(0, maxIndices)]];

    //    //Eliminate points that share a similar X or Z position to stop spawining in square grid line formations
    //    if ((int)firstVertexPosition.x == (int)secondVertexPosition.x || (int)firstVertexPosition.z == (int)secondVertexPosition.z)
    //    {
    //        point = GetRandomLocation(); //Re-Roll a position
    //    }
    //    else
    //    {
    //        // Select a random point on it. Not using Random.value as clumps form around Verticies 
    //        point = Vector3.Lerp(firstVertexPosition, secondVertexPosition, Random.Range(0.05f, 0.95f));
    //    }

    //    return point;
    //}

    private void playWalk(float angle)
    {
        if (angle >= -36 && angle <= 36)
        {
            anim.Play("WalkForward");
        }
        else if (angle > 36 && angle <= 72)
        {
            anim.Play("WalkLeftForward");
        }
        else if (angle < -36 && angle >= -72)
        {
            anim.Play("WalkRightForward");
        }
        else if (angle > 72 && angle <= 108)
        {
            anim.Play("WalkLeft");
        }
        else if (angle < -72 && angle >= -108)
        {
            anim.Play("WalkRight");
        }
        else if (angle > 108 && angle <= 144)
        {
            anim.Play("WalkLeftBack");
        }
        else if (angle < -108 && angle >= -144)
        {
            anim.Play("WalkRightBack");
        }
        else if ((angle > 144 && angle <= 180) || (angle < -144 && angle >= -180))
        {
            anim.Play("WalkBack");
        }
    }

    private void playIdle(float angle)
    {
        if (angle >= -36 && angle <= 36)
        {
            //Face forward.
            anim.Play("Idle", 0, 0.14f);
        }
        else if (angle > 36 && angle <= 72)
        {
            //Face left forwards.
            anim.Play("Idle", 0, 0.56f);
        }
        else if (angle < -36 && angle >= -72)
        {
            //Face right forwards.
            anim.Play("Idle", 0, 0.98f);
        }
        else if (angle > 72 && angle <= 108)
        {
            //Face left.
            anim.Play("Idle", 0, 0.28f);
        }
        else if (angle < -72 && angle >= -108)
        {
            //Face right.
            anim.Play("Idle", 0, 0.7f);
        }
        else if (angle > 108 && angle <= 144)
        {
            //Face left backwards.
            anim.Play("Idle", 0, 0.42f);
        }
        else if (angle < -108 && angle >= -144)
        {
            //Face right backwards.
            anim.Play("Idle", 0, 0.84f);
        }
        else if ((angle > 144 && angle <= 180) || (angle < -144 && angle >= -180))
        {
            //Face backward.
            anim.Play("Idle", 0, 0.0f);
        }
    }

    float nfmod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
}
