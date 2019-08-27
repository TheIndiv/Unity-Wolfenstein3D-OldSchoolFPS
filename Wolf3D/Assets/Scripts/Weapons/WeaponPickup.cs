using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : ItemManager
{
    private GameObject fakeWeapon;
    private AudioSource pickupSound;
    private GameObject weaponHolder;
	private WeaponSwitching switchingScript;
	private int pickupAmmo = 6;

    private void Start()
    {
        fakeWeapon = gameObject;
	    pickupSound = audioFX[transform.tag];
        weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder");
        switchingScript = weaponHolder.GetComponent<WeaponSwitching>();
    }

	void OnTriggerStay(Collider other)
	{
		if (!switchingScript.isSwitching)
		{
			int previousSelectedWeapon = switchingScript.selectedWeapon;
			
			if (fakeWeapon.tag.Equals("SMG") && switchingScript.smg.transform.parent != weaponHolder.transform)
			{
				switchingScript.smg.transform.parent = weaponHolder.transform;
				switchingScript.smg.transform.SetSiblingIndex(2);
				
				switchingScript.selectedWeapon = 2;
				if (switchingScript.chaingun.transform.parent == weaponHolder.transform) previousSelectedWeapon = 3;
				
				switchingScript.evaluateSelection(previousSelectedWeapon);
			} else if (fakeWeapon.tag.Equals("Chaingun") && switchingScript.chaingun.transform.parent != weaponHolder.transform)
			{
				//if (weaponSwitchingScript.smg == null) {
				//    realWeapon = (GameObject)Instantiate(Resources.Load("SMG"));
				//    createWeapon(realWeapon);
				//}
				
				switchingScript.chaingun.transform.parent = weaponHolder.transform;
				switchingScript.chaingun.transform.SetSiblingIndex(3);
	 
				switchingScript.selectedWeapon = weaponHolder.transform.childCount - 1;
				
				switchingScript.evaluateSelection(previousSelectedWeapon);
			}
			
			PlayerStats.stats[4] += pickupAmmo;
			if (PlayerStats.stats[4] > 99)
			{
				PlayerStats.stats[4] = 99;
			}
			fakeWeapon.SetActive(false);
			pickupSound.Play();
			pickupFlash.flashScreen();
		}
    }
}
