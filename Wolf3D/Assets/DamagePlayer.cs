using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
	public PlayerStats playerStats;
	public Flash damageFlash;
	
    // Start is called before the first frame update
    void Start()
	{
    	
    }

	public void dealDamage(int damage) {
		playerStats.dealDamage(damage);
		damageFlash.flashScreen();
	}
}
