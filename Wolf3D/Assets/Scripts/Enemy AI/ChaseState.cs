using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChaseState : BaseState
{
	private Enemy enemy;
	private bool CR_Running = false;
	private float cooldown = 1.5f;
	private Coroutine currentCoroutine;
	
	public ChaseState(Enemy enemy) : base(enemy.gameObject)
	{
		this.enemy = enemy;
		destinationSetter = enemy.GetComponent<AIDestinationSetter>();
	}
	
	public override State Tick()
	{
		if (enemy.isHit)
		{
			enemy.isHit = false;
			return State.Hit;
		}
		else
		{
			//Path p = ABPath.Construct(transform.position, enemy.player.transform.position, enemy.OnPathComplete);
			//enemy.richAI.SetPath(p);
			////enemy.seeker.StartPath(p);
			
			//Vector3 pos = new Vector3(enemy.player.transform.position.x, enemy.transform.position.y, enemy.player.transform.position.z);
			//enemy.transform.LookAt(pos);
			
			float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
			
			if (distance < 10)
			{
				Debug.DrawRay(enemy.transform.position, (enemy.player.transform.position - enemy.transform.position), Color.white, 4);
				float rand = Random.Range(0.0f, 1.0f);
				if (rand < 0.75f && !CR_Running)
				{
					enemy.richAI.maxSpeed = 0;
					currentCoroutine = StaticCoroutine.StartCoroutine(ShotCooldown_CR());
					enemy.isHit = false;
					return State.Attack;
				}
			}
			
			enemy.anim.SetTrigger(enemy.walkHash);
			return State.Chase;
		}
	}
    
	private IEnumerator ShotCooldown_CR()
	{
		CR_Running = true;
		cooldown = Random.Range(1.5f, 3.0f);
		yield return new WaitForSeconds(cooldown);
		CR_Running = false;
	}
}
