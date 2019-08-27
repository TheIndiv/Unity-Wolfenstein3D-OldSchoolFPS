using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	private Dictionary<State, BaseState> _availableState;
	
	public State state;
	public BaseState CurrentState { get; private set; }
	public event Action<BaseState> OnStateChanged;
	
	public void SetStates(Dictionary<State, BaseState> states)
    {
	    _availableState = states;
	    //foreach (KeyValuePair<State, BaseState> kvp in _availableState)
		//    Debug.Log ("Key = {" + kvp.Key + "}, Value = {" + kvp.Value + "}");
    }

    // Update is called once per frame
	private void Update()
	{
	    if (CurrentState == null)
	    {
	    	CurrentState = _availableState[State.Idle];
	    	CurrentState.previousState = State.Idle;
	    }
	    
		var nextState = CurrentState.Tick();
	    
	    if (nextState != null && nextState.GetType() != CurrentState.GetType())
	    {
	    	SwitchToNewState(nextState);
	    }
    }
    
	private void SwitchToNewState(State nextState)
	{
		//CurrentState.previousState = CurrentState;
		CurrentState = _availableState[nextState];
		state = nextState;
		//OnStateChanged.Invoke(CurrentState);
	}
}
