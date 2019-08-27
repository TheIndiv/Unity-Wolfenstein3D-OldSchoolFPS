using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
	private Enemy enemy;
	
	public IdleState(Enemy enemy) : base(enemy.gameObject)
    {
	    this.enemy = enemy;
    }

	public override State Tick()
	{
		if (enemy.isHit)
		{
			enemy.isHit = false;
			return State.Hit;
		}
		
		enemy.anim.SetTrigger(enemy.idleHash);
	    return State.Wander;
    }
}
