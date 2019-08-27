using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaingunStateMachine : StateMachineBehaviour
{
	private ChaingunFire chaingunScript;
	private int spinningHash = Animator.StringToHash("ChaingunSpinning");
	private int idleHash = Animator.StringToHash("Idle");
	
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    if (chaingunScript == null) chaingunScript = anim.GetComponentInParent<ChaingunFire>();
	    
	    if (stateInfo.nameHash == spinningHash) anim.SetBool(chaingunScript.fullAutoHash, true);
	    else if (stateInfo.nameHash == idleHash) anim.SetBool(chaingunScript.fullAutoHash, false);
    }
}
