using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class WanderState : BaseState
{
	private Enemy enemy;
	private bool firstRun = true;
	private bool CR_Running = false;
	private Coroutine lastCoroutine;
	
	public WanderState(Enemy enemy) : base(enemy.gameObject)
    {
	    this.enemy = enemy;
	    destinationSetter = enemy.GetComponent<AIDestinationSetter>();
    }

	public override State Tick()
	{
		if (enemy.isHit) 
		{
			enemy.richAI.destination = enemy.transform.position;
			destinationSetter.enabled = true;
			enemy.isHit = false;
			return State.Hit;
		}
		
		if (firstRun)
		{
			enemy.GoToNextPoint();
			firstRun = false;
		}
		
		enemy.isPathDone = enemy.seeker.IsDone();
	    
		//If the enemy currently has a path, however is unable to reach the destination, search for a new path.
		if (enemy.currentPath != null && !PathUtilities.IsPathPossible(enemy.currentPath.path))
		{
			enemy.richAI.SearchPath();
			Debug.Log("Get back here!");
		}
		
		//When the enemy has no active path, calculate a new one.
		if (!enemy.richAI.pathPending && enemy.richAI.reachedEndOfPath && enemy.isPathDone && !CR_Running)
		{
			enemy.anim.SetTrigger(enemy.idleHash);
			if (enemy.points[(int) enemy.nfmod(enemy.currentPoint-1, enemy.points.Length)].CompareTag("Finish"))
			{
				return State.Chase;
			}
			else
			{
				lastCoroutine = StaticCoroutine.StartCoroutine(WanderWait_CR());
			}
		} else if (!CR_Running)
		{
			enemy.anim.SetTrigger(enemy.walkHash);
		} else
		{
			enemy.anim.SetTrigger(enemy.idleHash);
		}
		
		if (enemy.alerted) 
		{
			if (lastCoroutine != null)
			{
				StaticCoroutine.StopCoroutine(lastCoroutine);
				lastCoroutine = null;
			}
			
			destinationSetter.enabled = true;
			
			return State.Chase;
		}
		else return State.Wander;
	}
    
	private IEnumerator WanderWait_CR()
	{
		CR_Running = true;
		yield return new WaitForSeconds(enemy.seekingDelay);
		enemy.seeker.transform.LookAt(enemy.points[enemy.currentPoint].position);
		enemy.GoToNextPoint();

		enemy.anim.SetTrigger(enemy.walkHash);
		CR_Running = false;
	}
}
