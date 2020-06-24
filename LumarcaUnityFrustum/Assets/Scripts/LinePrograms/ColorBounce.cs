using UnityEngine;
using System.Collections;

public class ColorBounce : LumarcaLineRenderer {

	private float waveRot = 0.0f;
	private float dest = 0.75f;
	private float tol = .1f;
	private float lerpVal = 0.002f;
	Vector3 mid = new Vector3();

	// Use this for initialization
	void Start () {
		SetMaterial();
	}
	
	// Update is called once per frame
	void Update () {
		waveRot = Mathf.Lerp(waveRot, dest, lerpVal);
		
		if(Mathf.Abs(waveRot) > Mathf.Abs(dest) - tol){
			dest *= -1;
		}
	}
	
	public override Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
	                                       float topX, float bottomX,
	                                       float topY, float bottomY,
	                                       float topZ, float bottomZ){

		mid = new Vector3((topX + bottomX)/2,
		                  (topY + bottomY)/2,
		                  (topZ + bottomZ)/2);

		Vector3[] result = new Vector3[2];

		mat.color = new Color(1- waveRot, 0, waveRot);
		
		result[0] = new Vector3();
		result[1] = new Vector3();

		result[0].Set(linePos.x, 
		              linePos.y - Vector3.Distance(linePos, mid) * waveRot,
		              linePos.z);
		result[1].Set(linePos.x, 
		              linePos.y + Vector3.Distance(linePos, mid) * waveRot,
		              linePos.z);

		return result;
	}
}
