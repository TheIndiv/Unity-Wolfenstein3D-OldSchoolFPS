using System.Collections;
using System.Collections.Generic;
using ch.sycoforge.Decal;
using UnityEngine;

[RequireComponent(typeof(EasyDecal))]
public class DecalPooling : MonoBehaviour
{
	
	public static string PoolName = "BloodSplat 1";
	
	private Vector3 prefabScale;
	private EasyDecal decal;
	private Transform originalParent;
	public Vector3 originalPosition;
	private bool firstTime = true;
	
	public Material[] materials;
	
	private void Awake()
	{
		decal = GetComponent<EasyDecal>();
		//float rand = Random.Range(0.0f, 1.0f);
		//if (rand < 0.98f)
		//{
		//	int newRand = Random.Range(0, materials.Length-1);
		//	decal.DecalMaterial = materials[newRand];
		//} else
		//{
		//	decal.DecalMaterial = materials[1];
		//}
		
		prefabScale = gameObject.transform.localScale;
		originalParent = transform.parent;
		
		decal.OnFadedOut += decal_OnFadedOut;
	}
	
	void OnSpawned()
	{
		originalPosition = transform.position;
		if (!firstTime)
		{
			decal.Reset();
		}
		firstTime = false;
	}

	void OnDespawned()
	{
		transform.parent = originalParent;
		transform.localScale = prefabScale;
	}
    
	private void decal_OnFadedOut(EasyDecal obj)
	{
		EZ_Pooling.EZ_PoolManager.GetPool(PoolName).Despawn(obj.transform);
	}
}
