using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
			enemy.gameObject.GetComponent<Enemy>().enabled = false;
			enemy.gameObject.GetComponent<RichAI>().enabled = false;
			enemy.gameObject.GetComponent<AIDestinationSetter>().enabled = false;
			enemy.enemyCollider.isTrigger = true;
			enemy.enemyCollider.center = new Vector3 (enemy.enemyCollider.center.x, enemy.enemyCollider.center.y - 0.4f, enemy.enemyCollider.center.z);
		}
		return State.Dead;
	}
}
