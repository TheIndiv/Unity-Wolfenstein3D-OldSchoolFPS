using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AttackState : BaseState
{
	private Enemy enemy;
	private bool CR_Running = false;
	private bool shootingFinished = false;
	private Coroutine currentCoroutine;
	
	public AttackState(Enemy enemy) : base(enemy.gameObject)
	{
		this.enemy = enemy;
		destinationSetter = enemy.GetComponent<AIDestinationSetter>();
	}
	
	public override State Tick()
	{
		if (enemy.isHit)
		{
			StaticCoroutine.StopCoroutine(currentCoroutine);
			if (CR_Running)
			{
				CR_Running = false;
				shootingFinished = false;
				enemy.richAI.maxSpeed = enemy.maxMoveSpeed;
			}
			enemy.isHit = false;
			return State.Hit;
		}
		//Vector3 pos = new Vector3(enemy.player.transform.position.x, enemy.transform.position.y, enemy.player.transform.position.z);
		//enemy.transform.LookAt(pos);
		
		if (!CR_Running)
		{
			if (shootingFinished) 
			{
				shootingFinished = false;
				enemy.richAI.maxSpeed = enemy.maxMoveSpeed;
				return State.Chase;
			} else
			{
				enemy.anim.SetTrigger(enemy.attackHash);
				enemy.anim.SetBool(enemy.walkHash, false);
				currentCoroutine = StaticCoroutine.StartCoroutine(Shoot_CR());
			}
		}
		
		return State.Attack;
	}
	
	private IEnumerator Shoot_CR()
	{
		CR_Running = true;
		yield return new WaitForSeconds(0.6f);
		CR_Running = false;
		shootingFinished = true;
	}
}
