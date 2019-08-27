using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifePickup : ItemManager
{
    public AudioSource extraLifePickupSound;

    private void Start()
    {
	    extraLifePickupSound = audioFX["ExtraLife"];
    }

    void OnTriggerEnter(Collider other)
    {
        if (PlayerStats.stats[2] < 9)
        {
            gameObject.SetActive(false);
            extraLifePickupSound.Play();
            pickupFlash.flashScreen();

            PlayerStats.stats[2]++;
            PlayerStats.stats[3] = 100;

            PlayerStats.stats[4] += 25;

            //Check and make sure the player's ammo does not exceed 99.
            if (PlayerStats.stats[4] > 99)
            {
                PlayerStats.stats[4] = 99;
            }

            if (PlayerStats.stats[2] > 9)
            {
                PlayerStats.stats[2] = 9;
            }
        } else
        {
            gameObject.SetActive(false);
            extraLifePickupSound.Play();
            pickupFlash.flashScreen();

            PlayerStats.stats[3] = 100;
            PlayerStats.stats[4] += 25;

            //Check and make sure the player's ammo does not exceed 99.
            if (PlayerStats.stats[4] > 99)
            {
                PlayerStats.stats[4] = 99;
            }
        }
    }
}
