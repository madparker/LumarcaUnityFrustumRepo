using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

	public int i;

	public string calScene;
	public string waveScene;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)){
			SceneManager.LoadScene(calScene);
		}		
		if(Input.GetKeyDown(KeyCode.W)){
			SceneManager.LoadScene(waveScene);
		}
	}
}
