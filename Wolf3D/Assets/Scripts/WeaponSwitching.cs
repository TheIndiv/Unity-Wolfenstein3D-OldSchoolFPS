using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
	public int selectedWeapon = 1;
	public int previousSelectedWeapon = 1;
	
    public bool isSwitching = false;
	public GameObject smg;
	public GameObject chaingun;
	private int stopShootHash = Animator.StringToHash("StopShoot");

    void Start()
	{
		SelectWeapon();
    }

    void Update()
    {
	    if (!isSwitching)
	    {
	        previousSelectedWeapon = selectedWeapon;
	
	        if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
	        {
	            selectedWeapon++;
		        selectedWeapon = (int)nfmod(selectedWeapon, transform.childCount);
	        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
	        {
	            selectedWeapon--;
		        selectedWeapon = (int) nfmod(selectedWeapon, transform.childCount);
	        } else if (Input.GetKeyDown(KeyCode.Alpha1)) 
	        {
	            selectedWeapon = 0;
	        } else if (Input.GetKeyDown(KeyCode.Alpha2))
	        {
	            selectedWeapon = 1;
	        } else if (Input.GetKeyDown(KeyCode.Alpha3) && smg.transform.parent == transform)
	        {
		        selectedWeapon = 2;
	        } else if (Input.GetKeyDown(KeyCode.Alpha4) && chaingun.transform.parent == transform)
	        {
		        selectedWeapon = transform.childCount - 1;
	        }
			
		    if (previousSelectedWeapon != selectedWeapon) evaluateSelection(previousSelectedWeapon);
	    }
    }
    
	public void evaluateSelection(int weapon)
	{
		isSwitching = true;
		Animator anim = transform.GetChild(weapon).GetComponent<Animator>();
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			anim.SetBool("SwitchWeapon", true);
		} else
		{
			StartCoroutine(switchWeapon(anim.GetCurrentAnimatorStateInfo(0), weapon));
		}
	}

	IEnumerator switchWeapon(AnimatorStateInfo isAnimationPlaying, int weapon)
    {
        isSwitching = true;

        //yield return new WaitForSeconds(0.1f + isAnimationPlaying.length - ((isAnimationPlaying.normalizedTime % 1f) * isAnimationPlaying.length));

        Animator animator = transform.GetChild(weapon).GetComponent<Animator>();
        ////Ensures its current state doesn't get stuck on the last frame.
        //animator.Play("New State", -1, 0f);
        ////Resets the animator.
	    //animator.Rebind();
	    animator.SetTrigger(stopShootHash);
	    animator.SetBool("Shooting", false);
	    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true);
	    
	    animator.SetBool("SwitchWeapon", true);
    }

    public void changeSelectedWeapon(int newIndex)
    {
        selectedWeapon = newIndex;
    }

    public void SelectWeapon()
	{
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
	            weapon.gameObject.SetActive(true);
                PlayerStats.stats[7] = selectedWeapon;
            } else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
        
		StartCoroutine(waitWeapon());
	}
    
	private IEnumerator waitWeapon()
	{
		Animator animator = transform.GetChild(selectedWeapon).GetComponent<Animator>();
		yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true);
		isSwitching = false;
	}

    float nfmod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
}
