using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour {

	public GameObject wrapper;

	public Vector3 dropPos = new Vector3(0, 0, 0);
	public Vector3 offsetRange = new Vector3(-0.3f, 0, 0);
	public GameObject[] prefabs;

	public float spawnTime;

	int currentIndex = 0;

	// Use this for initialization
	void Start () {
		InvokeRepeating("MakeObj", 0, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MakeObj(){
		GameObject go = Instantiate(prefabs[currentIndex++%prefabs.Length]) as GameObject;

		Rigidbody rb = go.GetComponent<Rigidbody>();

		rb.velocity = new Vector2(Random.Range(-2f, 2f), 0);

		Vector3 newPos = dropPos;

		newPos.z += Random.Range(-1f, 1f) * offsetRange.z;
		newPos.x += Random.Range(-1, 1) * offsetRange.x;

		go.transform.position = newPos;

		go.transform.parent = wrapper.transform;
	}
}
