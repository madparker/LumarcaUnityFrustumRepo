using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraightPhysics : MonoBehaviour {

	public Vector3 move;
	public Vector3 stop;
	public Vector3 reset;
	public float resetTime = 3.3f;

	Rigidbody rb;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();

		InvokeRepeating("Reset", resetTime, resetTime);
	}

	// Update is called once per frame
	void Update () {

		//		if(transform.position.x > stop.x ||
		//		   transform.position.y > stop.y ||
		//		   transform.position.z > stop.z){
		//			Reset();
		//		}

		rb.MovePosition(rb.position += move * Time.deltaTime);
	}

	void Reset(){
		//		Debug.Log("reset: " + transform.GetChild(0).transform.rotation.eulerAngles);
		rb.MovePosition(reset);
		//		transform.position += (move * Time.deltaTime) * 16;
	}


}
