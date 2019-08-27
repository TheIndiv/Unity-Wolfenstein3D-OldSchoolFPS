using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Raycasting is how to tell how far something is from the player.
public class PlayerRayCasting : MonoBehaviour
{
    public static float distanceFromTarget;
    public float toTarget;

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycastHit))
        {
            toTarget = raycastHit.distance;
            distanceFromTarget = toTarget;
        }
    }
}
