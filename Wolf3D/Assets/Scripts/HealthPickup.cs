using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : ItemManager
{
    public AudioSource healthPickupSound;
    public int healthAmount;

    private void Start()
    {
	    healthPickupSound = audioFX[transform.tag];
    }

    void OnTriggerEnter(Collider other)
    {
        if (PlayerStats.stats[3] < 100)
        {
            gameObject.SetActive(false);
            healthPickupSound.Play();
            pickupFlash.flashScreen();

            PlayerStats.stats[3] += healthAmount;
            if (PlayerStats.stats[3] > 100)
            {
                PlayerStats.stats[3] = 100;
            }
        }
    }
}
