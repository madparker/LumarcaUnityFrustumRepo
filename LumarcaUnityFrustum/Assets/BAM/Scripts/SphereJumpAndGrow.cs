using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereJumpAndGrow : MonoBehaviour {

	public Vector3 min;
	public Vector3 max;

	public float growSpeed;
	public float maxScale = 2;

	float scale = 0;

	const int MODE_GROW = 0;
	const int MODE_FADE = 1;

	int mode = MODE_GROW;

	public LoadLumarca ll;
	public GameObject otherSphere;

	GameObject growSphere;

	// Use this for initialization
	void Start () {
		growSphere = gameObject;
		scale = 0;
		mode = MODE_GROW;
		growSphere.transform.position = RandomPos();
	}
	
	// Update is called once per frame
	void Update () {
		if(scale < maxScale){
			scale += growSpeed * Time.deltaTime;
			growSphere.transform.localScale = new Vector3(scale, scale, scale);
		} else if(mode == MODE_GROW){
			growSphere = otherSphere;
			scale = 0;
			mode = MODE_FADE;
			growSphere.transform.position = transform.position;
		} else if(mode == MODE_FADE){
			scale = 0;
			growSphere.transform.localScale = new Vector3(scale, scale, scale);
			gameObject.transform.localScale = new Vector3(scale, scale, scale);
			growSphere = gameObject;
			mode = MODE_GROW;
			growSphere.transform.position = RandomPos();
		}
	}

//	public void Reset(){
//		mode = true;
//		transform.position = RandomPos();
//		scale = 0;
//		transform.localScale = new Vector3(scale, scale, scale);
//	}

	Vector3 RandomPos(){
		return new Vector3(
			Random.Range(min.x, max.x),
			Random.Range(min.y, max.y),
			Random.Range(min.z, max.z));
	}
}
