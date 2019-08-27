using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	protected PickupFlash pickupFlash;
	private GameObject audioFXObject;
	protected Dictionary<string, AudioSource> audioFX = new Dictionary<string, AudioSource>();
	
	void Awake()
	{
		pickupFlash = GameObject.FindGameObjectWithTag("FlashingPanel").GetComponent<PickupFlash>();
		audioFXObject = GameObject.FindGameObjectWithTag("AudioFX");
		
		foreach (Transform childFX in audioFXObject.transform)
		{
			if (!childFX.tag.Equals("Untagged"))
			{
				AudioSource audioSource = childFX.GetComponent<AudioSource>();
				audioFX.Add(childFX.tag, audioSource);
			}
		}
	}
}
