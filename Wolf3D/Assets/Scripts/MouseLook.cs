using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    private Vector2 mouseDirection;
	private Transform parentTransform;
    public float lookRange;
    public float sensitivityX = 15.0f;
    public float sensitivityY = 15.0f;

    private float rotX = 0;
    private float rotY = 0;

    private void Start()
    {
	    parentTransform = transform.parent.parent.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseXY = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDirection += mouseXY;
        //Ensures that the player cannot keep looking up and eventually have the camera be upside down. This will lock the amount the player can move in camera (in degrees) in the y-direction.
        mouseDirection.y = Mathf.Clamp(mouseDirection.y, -lookRange, lookRange);
	    transform.parent.localRotation = Quaternion.AngleAxis(-mouseDirection.y, Vector3.right);

	    //parentTransform.MoveRotation(Quaternion.AngleAxis(mouseDirection.x, Vector3.up));
	    parentTransform.rotation = Quaternion.AngleAxis(mouseDirection.x, Vector3.up);
    }
}
