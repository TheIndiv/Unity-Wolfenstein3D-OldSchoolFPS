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
	}
}
