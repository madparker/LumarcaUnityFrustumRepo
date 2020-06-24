using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayObject : MonoBehaviour {

	public GameObject delayObject;
	public float delayTime;

	// Use this for initialization
	void Start () {
		Invoke("ActivateObj", delayTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ActivateObj(){
		delayObject.SetActive(true);
	}
}
