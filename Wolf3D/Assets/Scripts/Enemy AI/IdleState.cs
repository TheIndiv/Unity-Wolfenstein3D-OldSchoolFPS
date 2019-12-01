using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IdleState : BaseState
{
	private Enemy enemy;
	
	public IdleState(Enemy enemy) : base(enemy.gameObject)
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
		
		Debug.Log(enemy.alerted);
		if (enemy.alerted) 
		{
			destinationSetter.enabled = true;
			return State.Chase;
		}
		else {
			enemy.anim.SetTrigger(enemy.idleHash);
			
			if (enemy.points.Length > 0) return State.Wander;
			else return State.Idle;
		}
    }
}
