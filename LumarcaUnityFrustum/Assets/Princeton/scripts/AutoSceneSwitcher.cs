using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneSwitcher : MonoBehaviour {

	public int i;

	public float sceneTime = 10;

	public float counter = 0;

	public float fadeOut = 1;
	public float fadeIn = 1;

	static AutoSceneSwitcher instance;

	// Use this for initialization
	void Start () {

		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		 
		counter += Time.deltaTime;

		if(counter > sceneTime - fadeOut){
			FadeToBlackScript ftb = Camera.main.gameObject.GetComponent<FadeToBlackScript>();

			if(ftb == null){
				ftb = Camera.main.gameObject.AddComponent<FadeToBlackScript>();
			}

			ftb.enabled = true;
			ftb.FadeIn = false;
			ftb.fadeTime = fadeOut;
		}

		if(counter > sceneTime){
			counter = 0;

			i++;

			print(i);

			if(i >= SceneManager.sceneCountInBuildSettings){
				i = 0;
			}

			print(i);

			SceneManager.LoadScene(i);
//			FadeIn();
		}

	}

	void FadeOut(){
		FadeToBlackScript ftb = Camera.main.gameObject.GetComponent<FadeToBlackScript>();

		if(ftb == null){
			ftb = Camera.main.gameObject.AddComponent<FadeToBlackScript>();
		}

		ftb.enabled = true;
		ftb.FadeIn = false;
		ftb.fadeTime = 1;
	}
		
	void FadeIn(){
		FadeToBlackScript ftb = Camera.main.gameObject.GetComponent<FadeToBlackScript>();

		if(ftb == null){
			ftb = Camera.main.gameObject.AddComponent<FadeToBlackScript>();
		}

		ftb.enabled = true;
		ftb.FadeIn = true;
		ftb.fadeTime = fadeIn;
	}
}
