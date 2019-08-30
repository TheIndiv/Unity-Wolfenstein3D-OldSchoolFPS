using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;
	public Animator anim;

    private int objectsInsideCollider = 0;
    private bool CR_Running = false;  //Check if CloseDoor() Coroutine is already running.
	public bool useInteractKeyToOpen = false;
    
	private int closedStateHash = Animator.StringToHash("Close");
	private int openedStateHash = Animator.StringToHash("Open");
	private int openHash = Animator.StringToHash("Open");
	private int closeHash = Animator.StringToHash("Close");
	
	public bool playerInArea = false;
	
	void Awake()
	{
		anim.SetFloat("speedMultiplier", -1f);
		anim.speed = 0;
	}
	
	void Update()
	{
		if (playerInArea)
		{
			if (Input.GetButtonDown("Interact") && useInteractKeyToOpen)
			{
				Debug.Log("E pressed!");
				//If the animator is at the state "ClosedState".
				//if (anim.GetCurrentAnimatorStateInfo(0).tagHash == closedStateHash)
				//{
				//	doorOpenSound.Play();
				//	anim.SetTrigger(openHash);
				//} else if (anim.GetCurrentAnimatorStateInfo(0).tagHash == openedStateHash)	//If the animator is at the state "OpenState".
				//{
				//	doorCloseSound.Play();
				//	anim.SetTrigger(closeHash);
				//}
				
				anim.SetFloat("speedMultiplier", -anim.GetFloat("speedMultiplier"));
				anim.speed = 1;
				
				if (anim.GetFloat("speedMultiplier") == 1f)
				{
					if (doorCloseSound.isPlaying) doorCloseSound.Stop();
					doorOpenSound.Play();
				}
				else
				{
					if (doorOpenSound.isPlaying) doorOpenSound.Stop();
					doorCloseSound.Play();
				}
			}
		}
		
		stopAnim();
	}
	

    private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player")) playerInArea = true;
		
		//If the animator is at the state "ClosedState".
	    //if (anim.GetCurrentAnimatorStateInfo(0).tagHash == closedStateHash && (!useInteractKeyToOpen || !other.CompareTag("Player"))) {
	    //    doorOpenSound.Play();
	    //    anim.SetTrigger(openHash);
        //}

        objectsInsideCollider++;
    }

    //private void OnTriggerStay(Collider other)
	//{
	//	Debug.Log("Player in the zone!");
    //    if (Input.GetButtonDown("Interact") && useInteractKeyToOpen)
    //    {
    //    	Debug.Log("E pressed!");
    //    	//If the animator is at the state "ClosedState".
	//        if (anim.GetCurrentAnimatorStateInfo(0).tagHash == closedStateHash)
    //        {
    //            doorOpenSound.Play();
    //            anim.SetTrigger(openHash);
    //        } else if (anim.GetCurrentAnimatorStateInfo(0).tagHash == openedStateHash)	//If the animator is at the state "OpenState".
    //        {
    //            doorCloseSound.Play();
    //            anim.SetTrigger(closeHash);
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player")) playerInArea = false;
    	
        objectsInsideCollider--;
		
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f)
		{
			if (objectsInsideCollider <= 0 && !CR_Running)
	        {
	            //StartCoroutine(CloseDoor());
	        }
		}
		else
		{
			
		}
	}
    
	private void stopAnim()
	{
		if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0f && anim.GetFloat("speedMultiplier") == -1) || (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && anim.GetFloat("speedMultiplier") == 1))
		{
			anim.speed = 0;
		}
	}

    //private void OpenDoor()
    //{
    //    if (!doorOpen)
    //    {
    //        doorOpenSound.Play();
    //        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime == 0) {
    //            anim.Play("DoorOpen");
    //        } else
    //        {
    //            anim.Play("DoorOpen", -1, 1 - (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f));
    //        }
    //        doorOpen = true;
    //    }
    //}

	//OLD
	//private IEnumerator CloseDoor()
    //{
    //    CR_Running = true;
    //    yield return new WaitForSeconds(5);

    //    if (objectsInsideCollider <= 0) {
	//        doorCloseSound.Play();
	//        anim.SetTrigger(closeHash);

    //        objectsInsideCollider = 0;
    //    }
    //    CR_Running = false;
    //}
    
	//NEW
	private IEnumerator CloseDoor()
	{
		CR_Running = true;
		yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
		yield return new WaitForSeconds(5);

		if (objectsInsideCollider <= 0) {
			doorCloseSound.Play();
			anim.SetFloat("speedMultiplier", -anim.GetFloat("speedMultiplier"));
			anim.speed = 1;

			objectsInsideCollider = 0;
		}
		CR_Running = false;
	}
}
