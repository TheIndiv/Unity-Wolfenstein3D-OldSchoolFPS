using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class BaseState
{
	public BaseState(GameObject gameObject)
	{
		this.gameObject = gameObject;
		this.transform = gameObject.transform;
	}
	
	protected GameObject gameObject;
	protected Transform transform;
	public State previousState;
	protected AIDestinationSetter destinationSetter;
	
	public abstract State Tick();
}
