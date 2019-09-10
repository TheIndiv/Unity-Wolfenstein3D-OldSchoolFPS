using System.Collections;
using System.Collections.Generic;
using ch.sycoforge.Decal;
using UnityEngine;

[RequireComponent(typeof(EasyDecal))]
public class DecalPoolingBloodDrip : MonoBehaviour
{
	
	public static string PoolName = "BloodSplat 2";
	
	private Vector3 prefabScale;
	private EasyDecal decal;
	private Transform originalParent;
	public Vector3 originalPosition;
	
	public DecalAnimation decalAnim;
	
	private void Awake()
	{
		decal = GetComponent<EasyDecal>();
		
		prefabScale = gameObject.transform.localScale;
		originalParent = transform.parent;
		
		decal.OnFadedOut += decal_OnFadedOut;
	}
	
	void OnSpawned()
	{
		originalPosition = transform.position;
		decalAnim.Reset();
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
