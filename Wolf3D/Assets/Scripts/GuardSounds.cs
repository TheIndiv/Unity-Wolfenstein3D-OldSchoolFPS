using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSounds : MonoBehaviour
{
	[SerializeField]
	private AudioSource shootSound;
	
	private void playShootSound()
	{
		shootSound.Play();
		
		int newDamage = 1 * Random.Range(1, 21);
		
		Debug.DrawRay(transform.position, (Camera.main.transform.position - transform.position) * 5, Color.black, 5);
		RaycastHit raycastHit;
		if (Physics.Raycast(transform.position, (Camera.main.transform.position - transform.position), out raycastHit, Mathf.Infinity, ~(1 << 2)))
		{
			if (raycastHit.transform.gameObject.layer == 11)
			{
				raycastHit.transform.GetComponent<DamagePlayer>().dealDamage(newDamage);
			}
		}
	}
}
