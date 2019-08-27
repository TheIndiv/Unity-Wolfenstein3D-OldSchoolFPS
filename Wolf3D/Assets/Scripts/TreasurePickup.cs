using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasurePickup : ItemManager
{
	private AudioSource extraLifeSource;
	private AudioSource treasurePickupSound;
	public int scoreAmount;
    
	void Start()
	{
		extraLifeSource = audioFX["ExtraLife"];
		treasurePickupSound = audioFX[transform.tag];
	}

    void OnTriggerEnter(Collider other)
    {
        if (PlayerStats.stats[1] < 999999)
        {
            gameObject.SetActive(false);
            treasurePickupSound.Play();
            pickupFlash.flashScreen();

            PlayerStats.stats[1] += scoreAmount;
            PlayerStats.newExtraLife -= scoreAmount;
            if (PlayerStats.newExtraLife <= 0)
            {
	            PlayerStats.giveExtraLife(extraLifeSource, pickupFlash);
                PlayerStats.newExtraLife = 40000;
            }
            if (PlayerStats.stats[1] > 999999)
            {
                PlayerStats.stats[1] = 999999;
            }
        }
    }
}
