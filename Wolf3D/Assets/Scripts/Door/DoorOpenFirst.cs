using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenFirst
{
    public GameObject door;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;
    private Animator doorAnimator;

    private void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        doorOpenSound.Play();
        doorAnimator.Play("DoorFrontOpen");
	    door.GetComponent<BoxCollider>().enabled = false;
	    StaticCoroutine.StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(5);
        doorCloseSound.Play();
        doorAnimator.Play("DoorFrontClose");
        yield return new WaitForSeconds(doorAnimator.GetCurrentAnimatorClipInfo(0).Length);
	    door.GetComponent<BoxCollider>().enabled = true;
    }
}
