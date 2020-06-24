using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCSingletonScript : MonoBehaviour {

	public static OSCSingletonScript instance;

	// Use this for initialization
	void Start () {
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
			Camera.main.GetComponent<LoadLumarca>().gameObjects = new GameObject[]{instance.gameObject};
		} else {
			Camera.main.GetComponent<LoadLumarca>().gameObjects = new GameObject[]{instance.gameObject};
			print(instance.gameObject.name);
			Destroy(gameObject);
		}
	}
}
