using UnityEngine;
using System.Collections;

public class LineShaderHolder : LumarcaLineRenderer {

	const float bpm = 60f/125f;

	public Vector3 center;

	public Vector3[] pulseCenter;
	public Vector3[] pulseClosest;

	public float[] mod;
	
	public float beatPos = -1;
	public float symbolPos = -1;
	public float symbolFade = -1;

	public Vector3 circPos;
	public float circRad;
	
	const string PULSE_0_POS = "_Pulse0Pos";
	const string PULSE_1_POS = "_Pulse1Pos";
	const string PULSE_2_POS = "_Pulse2Pos";
	const string PULSE_3_POS = "_Pulse3Pos";
	const string PULSE_4_POS = "_Pulse4Pos";
	const string PULSE_0_MOD = "_Pulse0Mod";
	const string PULSE_1_MOD = "_Pulse1Mod";
	const string PULSE_2_MOD = "_Pulse2Mod";
	const string PULSE_3_MOD = "_Pulse3Mod";
	const string PULSE_4_MOD = "_Pulse4Mod";
	
	const string BEAT_POS = "_BeatPos";
	const string SYMBOL_POS = "_SymbolPos";
	const string SYMBOL_FADE = "_SymbolFade";
	
	const string CIRCLE_POS= "_CircPos";
	const string CIRCLE_RAD= "_CircRad";

	bool init = true;

	int pulseNum = 0;

	bool sphereOn = false;

	public AudioSource audio;

	public float offsetAudio;

	public float timer;

	// Use this for initialization
	void Start () {

		offsetAudio *= 64 * bpm;

		audio.time = offsetAudio;

		Debug.Log("bpm: " + bpm);

		Invoke("SetInitalized", 0.1f);
		InvokeRepeating("Pulse", 0 * bpm, 	16 * bpm);
		InvokeRepeating("Pulse", 4 * bpm, 	16 * bpm);
		InvokeRepeating("Pulse", 7 * bpm, 	16 * bpm);
		InvokeRepeating("Pulse", 8 * bpm,	16 * bpm);
		InvokeRepeating("Pulse", 12 * bpm,	16 * bpm);

		Invoke("Pulser", 32 * bpm - offsetAudio);
		
		InvokeRepeating("Symbol", 64.5f * bpm  - offsetAudio, bpm);
		Invoke("StopSymbol", 152f * bpm - offsetAudio);
		InvokeRepeating("Symbol2", 192.5f * bpm  - offsetAudio, bpm);

		InvokeRepeating("CircleRot", 160.5f   * bpm - offsetAudio,	8 * bpm);
		InvokeRepeating("CircleRot", 161f * bpm - offsetAudio, 	8 * bpm);
		InvokeRepeating("CircleRot", 161.5f   * bpm - offsetAudio, 	8 * bpm);
		
		InvokeRepeating("CircleRot", 163f * bpm - offsetAudio, 16 * bpm);
		InvokeRepeating("CircleRot", 163.5f   * bpm - offsetAudio, 16 * bpm);
		InvokeRepeating("CircleRot", 164f * bpm - offsetAudio, 16 * bpm);
		
		InvokeRepeating("CircleRot", 171.5f   * bpm - offsetAudio, 16 * bpm);
		InvokeRepeating("CircleRot", 173f * bpm - offsetAudio, 16 * bpm);
	}
	float a = 0;

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(Input.GetKeyDown(KeyCode.Space)){
			a++;
			Debug.Log(Time.timeSinceLevelLoad/bpm);
		}

		for(int i = 0; i  < mod.Length; i++){
			mod[i] -= 1 * Time.deltaTime;
		}
		
		beatPos -= 0.09f * Time.deltaTime;
		symbolPos -= 0.75f * Time.deltaTime;
		symbolFade -= 0.04f * Time.deltaTime;
		
		circPos.x = center.x + Mathf.Sin(Time.timeSinceLevelLoad * 4) * 0.25f;
		circPos.z = center.z + Mathf.Cos(Time.timeSinceLevelLoad * 4) * 0.25f;

//		if(sphereOn){
			circRad -= 0.05f * Time.deltaTime;
//		}

		updateShader();
	}

	void updateShader(){

		Debug.Log("pulseClosest[1]: " + pulseClosest[1]);

		mat.SetVector(PULSE_0_POS, pulseClosest[0]);
		mat.SetVector(PULSE_1_POS, pulseClosest[1]);
		mat.SetVector(PULSE_2_POS, pulseClosest[2]);
		mat.SetVector(PULSE_3_POS, pulseClosest[3]);
		mat.SetVector(PULSE_4_POS, pulseClosest[4]);
		mat.SetFloat(PULSE_0_MOD, mod[0]);
		mat.SetFloat(PULSE_1_MOD, mod[1]);
		mat.SetFloat(PULSE_2_MOD, mod[2]);
		mat.SetFloat(PULSE_3_MOD, mod[3]);
		mat.SetFloat(PULSE_4_MOD, mod[4]);
		
		mat.SetFloat(BEAT_POS, beatPos);
		mat.SetFloat(SYMBOL_POS, symbolPos);
		mat.SetFloat(SYMBOL_FADE, symbolFade);
		
		mat.SetVector(CIRCLE_POS, circPos);
		mat.SetFloat(CIRCLE_RAD, circRad);
	}

	void Beat(){
		beatPos = -0.35f;
	}
	
	void Pulser(){
		mod[0] = 1;
		mod[1] = 1;
		mod[2] = 1;
		mod[3] = 1;
		mod[4] = 1;

		InvokeRepeating("Beat", 0, 0.48f);
	}

	int circCounter = 0;

	void CircleRot(){

//		sphereOn = true;
		circRad = 1.025f;

		circCounter++;

		if(circCounter == 22){
			CancelInvoke("CircleRot");
		}
	}

	void StopSymbol(){	
		CancelInvoke("Symbol");
	}

	void Symbol(){
		symbolPos = -0.345f;
		symbolFade = 1;
	}
	
	void Symbol2(){
		Symbol();
	}

	void Pulse(){
		mod[pulseNum%mod.Length] = 1;
		pulseNum++;
	}

	public override Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
	                                       float topX, float bottomX,
	                                       float topY, float bottomY,
	                                       float topZ, float bottomZ){
		Vector3[] result = new Vector3[0];
//
//		if(init){
//			for(int i = 0; i < pulseCenter.Length; i++){
//				if(Vector3.Distance(pulseCenter[i], linePos) <= 
//				   Vector3.Distance(pulseCenter[i], pulseClosest[i])){
//					pulseClosest[i] = linePos;
//				}
//			}
//		}
//		
		result = new Vector3[2];
		
		result[0] = UtilScript.CloneVec3(linePos);
		result[1] = UtilScript.CloneVec3(linePos);
		
		result[0].y = topY;
		result[1].y = bottomY;
		
		return result;
	}

	void SetInitalized(){
		init = false;
	}
}
