using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LumarcaSceneManager : MonoBehaviour {
	
	private static bool init = false;
	Dictionary<KeyCode, string> scenes;	

	// Use this for initialization
	void Start () {
		if(!init){
			init = true;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}

		scenes = new Dictionary<KeyCode, string>();
		scenes.Add(KeyCode.Alpha1, "LumarcaLines");
		scenes.Add(KeyCode.Alpha2, "Lines-RotatingSpheres");
		scenes.Add(KeyCode.Alpha3, "Lines-LumarcaText");
		scenes.Add(KeyCode.Alpha4, "Lines-Dino");
		scenes.Add(KeyCode.Alpha5, "Lines-Hand");
		scenes.Add(KeyCode.Alpha6, "Lines-Car");
		scenes.Add(KeyCode.Alpha7, "Lines-CenterWave");
		scenes.Add(KeyCode.Alpha8, "Lines-Whale");
		scenes.Add(KeyCode.Alpha9, "Lines-Horse2");
		scenes.Add(KeyCode.Alpha0, "Lines-Wave");
	}
	
	// Update is called once per frame
	void Update () {
		foreach(KeyCode key in scenes.Keys){
			if(Input.GetKeyDown(key)){
				Application.LoadLevel(scenes[key]);
			}
		}
	}
}
