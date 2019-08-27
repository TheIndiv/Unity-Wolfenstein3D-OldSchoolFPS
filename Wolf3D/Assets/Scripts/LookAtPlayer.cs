using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject target;
    private Transform cameraTransform;

    private void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player");
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        //Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        //transform.LookAt(targetPosition);

        //This is so the sprite will not be distorted when the camera is looking at it from an angle. This way, the sprite will ALWAYS be facing the camera head on.
        transform.forward = -cameraTransform.forward;
    }
}
