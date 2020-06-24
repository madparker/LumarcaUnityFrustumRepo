using UnityEngine;
using System.Collections;

public class MoveStraight : MonoBehaviour {
	
	public Vector3 move;
	public Vector3 stop;
	public Vector3 reset;
	public float resetTime = 3.3f;

	// Use this for initialization
	void Start () {
		
		InvokeRepeating("Reset", resetTime, resetTime);
	}
	
	// Update is called once per frame
	void Update () {

//		if(transform.position.x > stop.x ||
//		   transform.position.y > stop.y ||
//		   transform.position.z > stop.z){
//			Reset();
//		}
		
		transform.position += move * Time.deltaTime;
	}

	void Reset(){
//		Debug.Log("reset: " + transform.GetChild(0).transform.rotation.eulerAngles);
		transform.position = reset;
//		transform.position += (move * Time.deltaTime) * 16;
	}


}
