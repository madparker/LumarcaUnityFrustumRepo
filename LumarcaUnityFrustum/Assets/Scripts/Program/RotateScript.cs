using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {

//	742
//	628
//	514
//	400
//	285
//	171
//	56

	public Vector3 center;
	public float speed = 1;
	public float offset = 0;

	bool pause = false;

	// Use this for initialization
	void Start () {
		transform.RotateAround(center, Vector3.up, offset);
	}

	void Update(){
//		Debug.Log("euler: " + transform.rotation.eulerAngles);
		if(Input.GetKeyDown(KeyCode.Space)){
			pause = !pause;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		if(!pause)
			transform.RotateAround(center, Vector3.up, Time.fixedDeltaTime * speed);

//		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);
//
//		if(transform.position.z < 0 || transform.position.z > 1024){
//			speed *= -1;
//		}
//
//
//		Material mat = GetComponent<MeshRenderer>().material;
//
//		mat.SetVector("Center", transform.position);
	}
}
