using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGStateMachine : StateMachineBehaviour
{
	private SMGFire smgScript;
	private int fireHash = Animator.StringToHash("Fire");
	private int idleHash = Animator.StringToHash("Idle");
	
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    if (smgScript == null) smgScript = anim.GetComponentInParent<SMGFire>();
	    
	    //anim.SetBool(smgScript.stopShootHash, false);
	    //if (stateInfo.nameHash == fireHash) anim.SetBool(smgScript.fullAutoHash, true);
	    //else if (stateInfo.nameHash == idleHash) anim.SetBool(smgScript.fullAutoHash, false);
    }
}
