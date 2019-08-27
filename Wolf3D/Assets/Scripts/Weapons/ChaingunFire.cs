using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ChaingunFire : MonoBehaviour
{
    public AudioSource chaingunFireSource;
    public AudioSource chaingunLowerSource;
    private int baseDamage = 10;
	public WeaponSwitching weaponSwitching;
	private ParticleDecalPool particleDecalPool;
	
	//public float shakeDuration;
	public float shakeAmount;
	//public float shakeDecreaseAmount;
	public float shakeRoughness;
	public float shakeFadeInTime;
	public float shakeFadeOutTime;

	private double startTime;
    private double shootDuration;

    private bool CR_running = false;
    
	private Animator anim;
	
	private int startShootHash = Animator.StringToHash("StartShoot");
	public int fullAutoHash = Animator.StringToHash("FullAuto");
	private int stopShootHash = Animator.StringToHash("StopShoot");
	private int switchWeaponHash = Animator.StringToHash("SwitchWeapon");
	private int idleTagHash = Animator.StringToHash("Idle");
	private int shootingTagHash = Animator.StringToHash("Shooting");

    private void Start()
    {
        particleDecalPool = GameObject.FindGameObjectWithTag("BloodParticles").GetComponent<ParticleDecalPool>();
	    shootDuration = (double)chaingunFireSource.clip.samples / chaingunFireSource.clip.frequency;
	    anim = GetComponent<Animator>();
    }

    void Update()
    {
	    if ((Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) && anim.GetCurrentAnimatorStateInfo(0).tagHash == idleTagHash)
	    {
	    	if (PlayerStats.stats[4] == 0)
	    	{
	    		anim.SetBool(switchWeaponHash, true);
	    	} else
	    	{
		    	anim.SetTrigger(startShootHash);
	    	}
	    } else if (Input.GetButtonUp("Fire1"))
	    {
	    	if (anim.GetCurrentAnimatorStateInfo(0).tagHash == shootingTagHash)
	    	{
		    	anim.SetBool(startShootHash, false);
		    	anim.SetTrigger(stopShootHash);
	    	} else
	    	{
	    		anim.SetBool(stopShootHash, false);
	    	}
	    }
	    
	    if (anim.GetBool(switchWeaponHash))
	    {
	    	if (anim.GetCurrentAnimatorStateInfo(0).IsName("Lower"))
	    	{
	    		anim.SetBool(switchWeaponHash, false);
		    }
	    }
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
    
	public void shoot()
	{
		if (PlayerStats.stats[4] <= 0)
		{
			anim.SetTrigger(stopShootHash);
			anim.SetBool(fullAutoHash, false);
			anim.SetBool(switchWeaponHash, true);
		}
		else
		{
			PlayerStats.stats[4] -= 1;
	
			int newDamage = 1 * Random.Range(1, 21);
	
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out raycastHit, Mathf.Infinity, ~(1 << 2)))
			{
				if (raycastHit.transform.gameObject.layer == 10)
				{
					raycastHit.transform.parent.GetComponent<Enemy>().Damage(newDamage);
					//IDamageable<float> enemy = raycastHit.transform.gameObject.GetComponent<IDamageable<float>>();
					//if (enemy != null) enemy.Damage(newDamage);
					particleDecalPool.spawnBloodParticles(newDamage);
				}
			}
		}
	}
    
	private void fire()
	{
		chaingunFireSource.loop = true;
		chaingunFireSource.Play();
		shoot();
	}
	
	private void playLowerSound()
	{
		chaingunFireSource.loop = false;
		startTime = AudioSettings.dspTime;
		
		double timeLeft = shootDuration - ((double)chaingunFireSource.timeSamples / chaingunFireSource.clip.frequency);
		chaingunFireSource.SetScheduledEndTime(startTime + timeLeft);
		chaingunLowerSource.PlayScheduled(startTime + timeLeft);
	}
}
