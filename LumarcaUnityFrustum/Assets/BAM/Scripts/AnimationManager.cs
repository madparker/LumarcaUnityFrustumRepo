using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationManager : MonoBehaviour {

	public static AnimationManager instance;

	public List<string> scenes = new List<string>();
	public List<float> durations = new List<float>();

	private float timer;
	private static int index = 0;

	public float fadeTime = 5;

	private bool fade = false;

	// Use this for initialization
	void Start () {
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}

		timer = durations[index];
		Camera.main.GetComponent<FadeToBlackScript>().fadeTime = fadeTime;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if(timer <= 0){
			if(!fade){
				timer = fadeTime;
				fade = true;

				if(Camera.main.GetComponent<FadeToBlackScript>() == null){
					Camera.main.gameObject.AddComponent<FadeToBlackScript>();
				}

				Camera.main.GetComponent<FadeToBlackScript>().FadeIn = false;
			} else {
				index++;

				if(index == durations.Count){
					index = 0;
				}

				timer = durations[index];
				SceneManager.LoadScene(scenes[index]);
				fade = false;

				if(Camera.main.GetComponent<FadeToBlackScript>() == null){
					Camera.main.gameObject.AddComponent<FadeToBlackScript>();
				}

				Camera.main.GetComponent<FadeToBlackScript>().fadeTime = fadeTime;
			}
		}
	}

	public void HardSwitch(){
		timer = 0;
		fade = true;
	}
}
