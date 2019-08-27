using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : ItemManager
{
	public GameObject goldKey;
	public GameObject silverKey;
	private AudioSource keyPickupSound;
	
	void Start()
	{
		keyPickupSound = audioFX["Key"];
	}
    
    void OnTriggerEnter(Collider other)
	{
        gameObject.SetActive(false);

        if (transform.name.Equals("GoldKey")) {
            PlayerStats.stats[5] = 1;
	        goldKey.SetActive(true);
        } else
        {
            PlayerStats.stats[6] = 1;
	        silverKey.SetActive(true);
        }

	    keyPickupSound.Play();
        pickupFlash.flashScreen();
    }
}
