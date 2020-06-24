using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrop : MonoBehaviour {

	public Vector3 dropPos = new Vector3(0, 0, 0);
	public Vector3 offsetRange = new Vector3(-0.3f, 0.6f, -0.5f);

	public float resetTime = 3;

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		InvokeRepeating("Reset", resetTime, resetTime);

		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Reset(){
		Vector3 newPos = dropPos;

		rb.velocity = new Vector2(Random.Range(-2f, 2f), 0);

//		newPos.x -= rb.velocity.x * offsetRange.x;
//		newPos.z += Random.Range(-1f, 1f) * offsetRange.z;
//		newPos.x += Random.Range(-1, 1) * offsetRange.x;

		transform.position = newPos;

		rb.angularVelocity = Vector3.zero;
	}
}
