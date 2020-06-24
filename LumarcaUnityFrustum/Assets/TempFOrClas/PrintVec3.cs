using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintVec3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 vec = new Vector3(2.67768348342f, 23.23432432432f, Mathf.PI);

		print(vec);
		print(UtilScript.ActualVector3(vec));

		float f = 2.67768348342f;

		print(f);
		Debug.Log(f + "");

		double d = f;

		print("d: " + d);
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
