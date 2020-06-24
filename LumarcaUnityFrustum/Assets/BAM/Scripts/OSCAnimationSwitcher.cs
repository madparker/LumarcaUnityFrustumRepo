using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OSCAnimationSwitcher : MonoBehaviour {

	public string OSCScene = "KinectScene"; 

	private static float timer;
	public float SwitchTime;

	public static OSCAnimationSwitcher instance;

	private bool turnOffAnimation = false;
	private bool recentOSC = true;

	public PositionScript avatar;

	// Use this for initialization
	void Start () {
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
			timer = SwitchTime;
//			AnimationManager.instance.HardSwitch();
		} else {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(timer >= 0){
			timer -= Time.deltaTime;
		}

		if(timer < 0 && recentOSC){
			recentOSC = false;
			AnimationManager.instance.enabled = true;
			AnimationManager.instance.HardSwitch();
		}

		if(turnOffAnimation){
			turnOffAnimation = false;
			AnimationManager.instance.enabled = false;

			if(!SceneManager.GetActiveScene().name.Equals(OSCScene)){
				SceneManager.LoadScene(OSCScene);
			}
		}
	}

	public void GotOSC(){
		Debug.Log("GOT OSC");

		if(!avatar.Invalid()){
			timer = SwitchTime;
			turnOffAnimation = true;
			recentOSC = true;
		}
	}
}
