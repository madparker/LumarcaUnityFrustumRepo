using UnityEngine;
using System.Collections;

public class FPSDIsplay : MonoBehaviour {

	float avg = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		avg += (int)(1.0f / Time.deltaTime);
	}

	void OnGUI()
	{
		GUI.Label(new Rect(100, 0, 100, 100), "" + (int)(1.0f / Time.deltaTime));   
		GUI.Label(new Rect(100, 20, 100, 100), "" + avg/Time.frameCount); 
		GUI.Label(new Rect(100, 40, 100, 100), "Line: " + LoadLumarca.currentLine);
	}
}
