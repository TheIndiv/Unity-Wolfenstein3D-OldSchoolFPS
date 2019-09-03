using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Guard : Enemy
{
	public Transform Target { get; private set; }
	public float _enemyHealth;
	public float _baseDamage;
	public float _maxMoveSpeed;
	public int _ammo;
	
	public Transform[] _points;
	
	private StateMachine StateMachine;
	
	private void Awake()
	{
		StateMachine = GetComponent<StateMachine>();
		InitializeStateMachine();
	}
	
	private void InitializeStateMachine()
	{
		var states = new Dictionary<State, BaseState>()
		{
			{ State.Idle, new IdleState(this) },
			{ State.Wander, new WanderState(this) },
			{ State.Chase, new ChaseState(this) },
			{ State.Hit, new HitState(this) },
			{ State.Attack, new AttackState(this) },
			{ State.Dead, new DeadState(this) }
		};
		
		GetComponent<StateMachine>().SetStates(states);
	}
	
	public override void Damage(float damageTaken)
	{
		//Debug.Log("Guard Hit! Damage Taken: " + damageTaken);
		_enemyHealth -= damageTaken;
		alerted = true;
		isHit = true;
	}
	
	public void SetTarget(Transform target)
	{
		Target = target;
	}
	
	public override float enemyHealth
	{
		get { return _enemyHealth; }
		set { _enemyHealth = value; }
	}
	
	public override float baseDamage
	{
		get { return _baseDamage; }
		set { _baseDamage = value; }
	}
	
	public override float maxMoveSpeed
	{
		get { return _maxMoveSpeed; }
		set { _maxMoveSpeed = value; }
	}
	
	public override int ammo
	{
		get { return _ammo; }
		set { _ammo = value; }
	}
	
	public override Transform[] points
	{
		get { return _points; }
	}
	
	public override void changeSprite(int num)
	{
		Vector3 dirToTarget = player.transform.position - transform.position;
		float angle = Vector3.SignedAngle(dirToTarget, transform.forward, Vector3.up);
		
		if (num == 0) playIdle(angle);
		else playWalk(angle);
	}
	
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
}