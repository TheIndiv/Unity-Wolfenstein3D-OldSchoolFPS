using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
	private Enemy enemy;
	private bool deadAnimPlayed = false;
	
	public DeadState(Enemy enemy) : base(enemy.gameObject)
	{
		this.enemy = enemy;
	}
	
	public override State Tick()
	{
		if (!deadAnimPlayed)
		{
			deadAnimPlayed = true;
			enemy.anim.SetTrigger(enemy.deathHash);
		}
		return State.Dead;
	}
}
