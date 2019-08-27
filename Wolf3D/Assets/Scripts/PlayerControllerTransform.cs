using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTransform : MonoBehaviour
{
	public Rigidbody rb;
	public CharacterController character;
    private string moveInputAxis = "Vertical";
    private string strafeInputAxis = "Horizontal";
    private string turnInputAxis = "HorizontalArrows";
    private float mouseDirectionX;

    public float rotationRate = 360;

    private float moveSpeed;
    public float walkSpeed;
	public float sprintSpeed;

    private void Start()
	{
		character = GetComponent<CharacterController>();
		moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float moveAxis = Input.GetAxisRaw(moveInputAxis);
        float strafeAxis = Input.GetAxisRaw(strafeInputAxis);
        float turnAxis = Input.GetAxisRaw(turnInputAxis);

        if (Input.GetAxisRaw("Sprint") == 1) {
            moveSpeed = sprintSpeed;
        }

        if (Input.GetAxisRaw("Sprint") == 0) {
            moveSpeed = walkSpeed;
        }
        
	    Vector3 direction = new Vector3(strafeAxis * moveSpeed, 0f, moveAxis * moveSpeed);
	    direction *= Time.deltaTime;
	    character.Move(transform.TransformDirection(direction));
        
        //ApplyInput(moveAxis, strafeAxis, turnAxis);
    }

    //private void ApplyInput(float moveInput, float strafeInput, float turnInput)
    //{
    //    Move(moveInput, strafeInput);
    //    Turn(turnInput);
    //}

    //private void Move(float inputMove, float inputStrafe)
    //{
    //    Vector3 direction = new Vector3(inputStrafe, 0, inputMove);
    //    direction = direction.normalized * moveSpeed * Time.deltaTime;
    //    rb.velocity = transform.TransformDirection(direction) * moveSpeed;

    //}

    //private void Turn(float input)
    //{
    //    Vector3 rotationVelocity = new Vector3(0, rotationRate * input, 0);
    //    Quaternion deltaRotation = Quaternion.Euler(rotationVelocity * Time.deltaTime);
    //    rb.MoveRotation(rb.rotation * deltaRotation);
    //    //transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    //}
}
