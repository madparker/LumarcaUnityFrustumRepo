using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumberSceneSwitcher : MonoBehaviour {

	public int i;
//
//	public string calScene;
//	public string waveScene;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.F)){
			i++;

			i = i%SceneManager.sceneCountInBuildSettings;

			SceneManager.LoadScene(i);
		}

//		NumSceneSwitch(KeyCode.Alpha0);
//		NumSceneSwitch(KeyCode.Alpha1);
//		NumSceneSwitch(KeyCode.Alpha2);
//		NumSceneSwitch(KeyCode.Alpha3);
//		NumSceneSwitch(KeyCode.Alpha4);
//		NumSceneSwitch(KeyCode.Alpha5);
//		NumSceneSwitch(KeyCode.Alpha6);
//		NumSceneSwitch(KeyCode.Alpha7);
//		NumSceneSwitch(KeyCode.Alpha8);
//		NumSceneSwitch(KeyCode.Alpha9);
	}

	void NumSceneSwitch(KeyCode k){
		if(Input.GetKeyDown(k)){
			print(k.GetHashCode() - 48);

			SceneManager.LoadScene(k.GetHashCode() - 48);
		}
	}
}
