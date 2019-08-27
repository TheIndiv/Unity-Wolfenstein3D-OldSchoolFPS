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

    private void OnTriggerEnter(Collider other)
	{
		//If the animator is at the state "ClosedState".
	    if (anim.GetCurrentAnimatorStateInfo(0).tagHash == closedStateHash && (!useInteractKeyToOpen || !other.CompareTag("Player"))) {
	        doorOpenSound.Play();
	        anim.SetTrigger(openHash);
        }

        objectsInsideCollider++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && useInteractKeyToOpen)
        {
        	//If the animator is at the state "ClosedState".
	        if (anim.GetCurrentAnimatorStateInfo(0).tagHash == closedStateHash)
            {
                doorOpenSound.Play();
                anim.SetTrigger(openHash);
            } else if (anim.GetCurrentAnimatorStateInfo(0).tagHash == openedStateHash)	//If the animator is at the state "OpenState".
            {
                doorCloseSound.Play();
                anim.SetTrigger(closeHash);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInsideCollider--;

	    if (objectsInsideCollider <= 0 && !CR_Running)
        {
            StartCoroutine(CloseDoor());
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

	private IEnumerator CloseDoor()
    {
        CR_Running = true;
        yield return new WaitForSeconds(5);

        if (objectsInsideCollider <= 0) {
	        doorCloseSound.Play();
	        anim.SetTrigger(closeHash);

            objectsInsideCollider = 0;
        }
        CR_Running = false;
    }
}
