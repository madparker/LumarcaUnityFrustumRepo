using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	float speed = 60.0f;
	float jumpSpeed = 80.0f;
	float gravity = 0.0f;

	private Vector3 moveDirection = Vector3.zero;
	private bool grounded = false;

	void FixedUpdate() 
	{
		// Calculate the move direction
		moveDirection = Camera.main.transform.TransformDirection(moveDirection);
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		// Move the controller
		CharacterController controller = GetComponent<CharacterController>();    
		controller.Move(moveDirection * Time.deltaTime);
	}
}
