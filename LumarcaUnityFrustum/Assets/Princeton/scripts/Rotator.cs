using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public Vector3 rot;

	Vector3 currentRot = new Vector3();
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		currentRot = transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		currentRot += rot * Time.deltaTime;

		rb.MoveRotation(Quaternion.Euler(currentRot));
	}
}
