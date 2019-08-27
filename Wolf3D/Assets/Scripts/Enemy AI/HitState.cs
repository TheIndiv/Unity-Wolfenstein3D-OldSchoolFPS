using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HitState : BaseState
{
	private Enemy enemy;
	private bool CR_Running = false;
	private bool hitFinished = false;
	public State prevState;
	private Coroutine currentCoroutine;
	
	public HitState(Enemy enemy) : base(enemy.gameObject)
	{
		this.enemy = enemy;
		destinationSetter = enemy.GetComponent<AIDestinationSetter>();
	}
	
	public override State Tick()
	{
		if (enemy.isHit) 
		{
			StaticCoroutine.StopCoroutine(currentCoroutine);
			CR_Running = false;
			hitFinished = false;
			enemy.isHit = false;
		}
		
		if (!CR_Running)
		{
			if (hitFinished)
			{
				hitFinished = false;
				return State.Chase;
			} else
			{
				currentCoroutine = StaticCoroutine.StartCoroutine(Hit_CR());
			}
		}
		
		return State.Hit;
	}
	
	private IEnumerator Hit_CR()
	{
		CR_Running = true;
		enemy.anim.SetTrigger(enemy.hitHash);
		enemy.anim.SetInteger(enemy.randomHitHash, Random.Range(1, 3));
		enemy.anim.SetBool(enemy.walkHash, false);
		enemy.richAI.maxSpeed = 0;
		yield return new WaitForSeconds(0.2f);
		enemy.richAI.maxSpeed = enemy.maxMoveSpeed;
		CR_Running = false;
		hitFinished = true;
	}
}
