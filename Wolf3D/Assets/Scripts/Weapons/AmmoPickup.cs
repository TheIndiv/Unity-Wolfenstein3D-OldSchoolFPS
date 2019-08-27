using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : ItemManager
{
    public AudioSource ammoPickupSound;

    private void Start()
    {
	    ammoPickupSound = audioFX["Ammo"];
    }

    void OnTriggerEnter(Collider other)
    {
        if (PlayerStats.stats[4] < 99)
        {
            gameObject.SetActive(false);
            ammoPickupSound.Play();
            pickupFlash.flashScreen();

            PlayerStats.stats[4] += 8;
            if (PlayerStats.stats[4] > 99)
            {
                PlayerStats.stats[4] = 99;
            }
        }
    }
}
