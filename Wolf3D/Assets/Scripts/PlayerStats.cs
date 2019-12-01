using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
                               //Floor, Score, Lives, Health, Ammo, Gold Key, Blue Key, Current Weapon Image/Current Selected Weapon
	public static int[] stats = {1,     0,     3,     100,     8,    0,        0,        1};
    public GameObject[] textDisplay = new GameObject[8];
    private int[] previousStats = {1, 0, 3, 100, 98, 0, 0, 1};
    public Texture[] weaponTexture = new Texture[4];
    public static int newExtraLife = 40000;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("previousStats = " + String.Join(" ",
        //     new List<int>(previousStats)
        //     .ConvertAll(i => i.ToString())
        //     .ToArray()));
        //Debug.Log("stats = " + String.Join(" ",
        //     new List<int>(stats)
        //     .ConvertAll(i => i.ToString())
        //     .ToArray()));

        if (previousStats != stats) {
            int index = 0;
            foreach (int stat in stats)
            {
                if (previousStats[index] != stat)
                {
                    if (index == 7)
                    {
                        textDisplay[index].GetComponent<RawImage>().texture = weaponTexture[stat];
                    }
                    else if (index != 5 && index != 6)
                    {
                        textDisplay[index].GetComponent<TextMeshProUGUI>().text = "" + stat;
                    }
                    previousStats[index] = stat;
                }
                index++;
            }
        }
    }

    public static void giveExtraLife(AudioSource extraLifePickupSound, Flash pickupFlash)
    {
        extraLifePickupSound.Play();
        pickupFlash.flashScreen();

        //Give player max health.
        stats[3] = 100;

        //Give player 25 bullets.
        stats[4] += 25;
        //Check and make sure that the ammo count does not exceed 99.
        if (stats[4] > 99)
        {
            stats[4] = 99;
        }

        if (stats[2] < 9)
        {
            stats[2]++;
            if (stats[2] > 9)
            {
                stats[2] = 9;
            }
        }
    }
    
	public void dealDamage(int damage) {
		stats[3] -= damage;
		//Check if player's health is =< 0. This is Game Over...
	}
}
