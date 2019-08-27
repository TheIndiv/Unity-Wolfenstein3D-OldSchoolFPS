using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExit : StateMachineBehaviour
{
	private WeaponSwitching weaponSwitching;
	private int startShootHash = Animator.StringToHash("StartShoot");
	private int switchWeaponHash = Animator.StringToHash("SwitchWeapon");

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
    {
	    if (weaponSwitching == null) weaponSwitching = anim.transform.parent.GetComponentInParent<WeaponSwitching>();
	    
	    if (PlayerStats.stats[4] == 0 && weaponSwitching.previousSelectedWeapon == weaponSwitching.selectedWeapon)
	    {
		    //Automatically switch to the knife if out of ammo.
		    weaponSwitching.changeSelectedWeapon(0);
		    weaponSwitching.SelectWeapon();
	    } else
	    {
		    weaponSwitching.SelectWeapon();
	    }
	    
	    anim.SetBool(startShootHash, false);
	    anim.SetBool(switchWeaponHash, false);
    }
}
