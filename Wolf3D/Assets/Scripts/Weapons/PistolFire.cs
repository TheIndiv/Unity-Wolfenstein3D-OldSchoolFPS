using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PistolFire : MonoBehaviour
{
	public AudioSource fireSound;
    private int baseDamage = 10;
	public WeaponSwitching weaponSwitching;
	private ParticleDecalPool particleDecalPool;
	
	//public float shakeDuration;
	public float shakeAmount;
	//public float shakeDecreaseAmount;
	public float shakeRoughness;
	public float shakeFadeInTime;
	public float shakeFadeOutTime;
    
	private Animator anim;
	
	private int startShootHash = Animator.StringToHash("StartShoot");
	private int switchWeaponHash = Animator.StringToHash("SwitchWeapon");
	private int randomFireHash = Animator.StringToHash("RandomFire");
	private int LowerHash = Animator.StringToHash("Lower");
	
	//public CameraShake cameraShake;
	public float rateOfFire;
	private float timer = 0;
	
	public float pitchMin, pitchMax, volumeMin, volumeMax;

    private void Start()
    {
        particleDecalPool = GameObject.FindGameObjectWithTag("BloodParticles").GetComponent<ParticleDecalPool>();
	    anim = transform.GetComponentInParent<Animator>();
    }
    
	public void setShakeAmount(float value)
	{
		shakeAmount = value;
	}
	
	public void setShakeRoughness(float value)
	{
		shakeRoughness = value;
	}
	
	public void setShakeFadeInTime(float value)
	{
		shakeFadeInTime = value;
	}
	
	public void setShakeFadeOutTime(float value)
	{
		shakeFadeOutTime = value;
	}

    void Update()
	{
		timer += Time.deltaTime;
	    if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
	    {
		    if (timer >= rateOfFire)
		    {
			    if (PlayerStats.stats[4] <= 0)
		        {
			        anim.SetBool(switchWeaponHash, true);
		        } else
		        {
			        anim.SetTrigger(startShootHash);
			        anim.SetInteger(randomFireHash, Random.Range(1, 5));
		        }
			    timer = 0;
		    }
        } else if (Input.GetButtonUp("Fire1"))
        {
        	anim.SetBool(startShootHash, false);
        }
        
	    if (anim.GetBool(switchWeaponHash))
	    {
	    	if (anim.GetCurrentAnimatorStateInfo(0).tagHash == LowerHash)
	    	{
	    		anim.SetBool(switchWeaponHash, false);
		    }
	    }
    }
    
	private void shoot()
	{
		//StartCoroutine(cameraShake.Shake(shakeDuration, shakeAmount, shakeDecreaseAmount));
		CameraShaker.Instance.ShakeOnce(shakeAmount, shakeRoughness, shakeFadeInTime, shakeFadeOutTime);
		fireSound.Play();
		//fireSound.pitch = Random.Range(pitchMin, pitchMax);
		//fireSound.volume = Random.Range(volumeMin, volumeMax);
		//fireSound.PlayOneShot(fireClip);
		
		PlayerStats.stats[4] -= 1;

		int newDamage = 1 * Random.Range(20, 30);

		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out raycastHit, Mathf.Infinity, ~(1 << 2)))
		{
			if (raycastHit.transform.gameObject.layer == 10)
			{
				Enemy enemy = raycastHit.transform.parent.GetComponent<Enemy>();
				enemy.Damage(newDamage);
				//Debug.Log(enemy);
				//if (enemy != null) enemy.Damage(newDamage);
				if (enemy.enemyHealth <= 0) particleDecalPool.spawnBloodParticles(newDamage, raycastHit.transform.position - new Vector3(0, 0.4f, 0));
				else particleDecalPool.spawnBloodParticles(newDamage, raycastHit.transform.position);
			}
		}
	}
}
