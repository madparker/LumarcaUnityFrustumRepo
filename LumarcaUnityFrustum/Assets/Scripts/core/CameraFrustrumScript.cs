using UnityEngine;
using System.Collections;

public class CameraFrustrumScript : MonoBehaviour {

	public Vector3 projectorPos = new Vector3(0, 0, -10);
	public float pixelWidth = 1024;
	public float pixelHeight = 768;
	public float throwRatio = 2;

	public bool debug = true;
	
	public float physicalWidth;
	public float physicalHeight;
	
	public bool ceilingMounted = false;
	private bool prevCM;
	
	float throwDistance;
	Camera cam;

	void Awake(){

//		TextAsset asset = Resources.Load(jsonLineFile) as TextAsset;
//
//		JObject j1 = JObject.Parse(asset.text);
	}

	void Start () {
		prevCM = ceilingMounted;
		SetupCamera();
	}
	
	// Update is called once per frame
	void Update () {
		if(prevCM != ceilingMounted){
			SetupCamera();
		}
	}

	public void SetupCamera(float pWidth, Vector3 projPos, float tRatio, bool cMounted){
		this.physicalWidth = pWidth;
		this.projectorPos = projPos;
		this.throwRatio = tRatio;
		this.ceilingMounted = cMounted;

		SetupCamera();
	}

	public void SetupCamera(){

		prevCM = ceilingMounted;
		
		cam = GetComponent<Camera>();
		
		cam.aspect = pixelWidth/pixelHeight;
		physicalHeight = physicalWidth/cam.aspect;
		
		throwDistance = throwRatio * physicalWidth;

		Debug.Log("throwDistance: " + (throwDistance - 0));
		
		cam.fieldOfView = 2 * Mathf.Atan(physicalHeight * 0.5f / throwDistance) * Mathf.Rad2Deg;
		cam.nearClipPlane = throwDistance;
		cam.farClipPlane = cam.nearClipPlane + physicalWidth;
		
		//		Debug.Log("throwDistance: " + throwDistance);
		//		Debug.Log("physicalWidth: " + physicalWidth);
		//		Debug.Log("physicalHeight: " + physicalHeight);
		
		if(ceilingMounted){
			cam.transform.position = new Vector3(0, +physicalHeight/2f, -throwDistance - physicalWidth/2f);
			SetObliqueness(0, -1);
		} else {
			cam.transform.position = new Vector3(0, -physicalHeight/2f, -throwDistance - physicalWidth/2f);
			SetObliqueness(0, 1);
		}
		
		if(debug){
			showFrustum();
			showDebugMessages();
		}
	}

	void showFrustum(){

		LineRenderer line = gameObject.AddComponent<LineRenderer>();

		
		line.material.color = Color.white;

		line.positionCount = 10;
		line.startWidth =30;

		line.SetPosition(0, cam.ViewportToWorldPoint (new Vector3 (0,0, cam.nearClipPlane)));
		line.SetPosition(1, cam.ViewportToWorldPoint (new Vector3 (0,1, cam.nearClipPlane)));
		line.SetPosition(2, cam.ViewportToWorldPoint (new Vector3 (1,1, cam.nearClipPlane)));
		line.SetPosition(3, cam.ViewportToWorldPoint (new Vector3 (1,0, cam.nearClipPlane)));
		line.SetPosition(4, cam.ViewportToWorldPoint (new Vector3 (0,0, cam.nearClipPlane)));
		line.SetPosition(5, cam.ViewportToWorldPoint (new Vector3 (0,0, cam.farClipPlane)));
		line.SetPosition(6, cam.ViewportToWorldPoint (new Vector3 (0,1, cam.farClipPlane)));
		line.SetPosition(7, cam.ViewportToWorldPoint (new Vector3 (1,1, cam.farClipPlane)));
		line.SetPosition(8, cam.ViewportToWorldPoint (new Vector3 (1,0, cam.farClipPlane)));
		line.SetPosition(9, cam.ViewportToWorldPoint (new Vector3 (0,0, cam.farClipPlane)));
	}

	public virtual Vector3[] GetFrontPlane(){

		Vector3[] vecs = new Vector3[4];
		
		vecs[0] = cam.ViewportToWorldPoint (new Vector3 (0,0, cam.nearClipPlane));
		vecs[1] = cam.ViewportToWorldPoint (new Vector3 (1,0, cam.nearClipPlane));
		vecs[2] = cam.ViewportToWorldPoint (new Vector3 (1,1, cam.nearClipPlane));
		vecs[3] = cam.ViewportToWorldPoint (new Vector3 (0,1, cam.nearClipPlane));

		return vecs;
	}

	void SetObliqueness(float horizObl, float vertObl) {

		Matrix4x4 mat = cam.projectionMatrix;

		mat[0, 2] = horizObl;
		mat[1, 2] = vertObl;
		cam.projectionMatrix = mat;
	}

	void showDebugMessages(){
		var frustumHeight = 2.0f * 2048 * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
		var frustumWidth = frustumHeight * cam.aspect;
		
		Debug.Log("camera.aspect: " + cam.aspect);
		Debug.Log("frustumWidth: " + frustumWidth);
		Debug.Log("frustumHeight: " + frustumHeight);
		Debug.Log("camera.nearClipPlane: " + cam.nearClipPlane);
		
		
		Vector3 p = cam.ViewportToWorldPoint (new Vector3 (0,0, cam.nearClipPlane));
		Debug.Log("near0: " + p);
		p = cam.ViewportToWorldPoint (new Vector3 (1,1, cam.nearClipPlane));
		Debug.Log("near1: " + p);
		p = cam.ViewportToWorldPoint (new Vector3 (0,0, cam.farClipPlane));
		Debug.Log("far0: " + p);
		p = cam.ViewportToWorldPoint (new Vector3 (1,1, cam.farClipPlane));
		Debug.Log("far1: " + p);
	}
}
