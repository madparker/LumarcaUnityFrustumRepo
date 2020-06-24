using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereWave : LumarcaLineRenderer {

	public float waxLengthMod = 30;
	public GameObject center;
	public float speedMod;
	public float offSet;

	// Use this for initialization
	void Start () {
		SetMaterial();
		mat.color = new Color(0, 0, 1);
	}

	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime * speedMod;
	}

	public override Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
		float topX, float bottomX,
		float topY, float bottomY,
		float topZ, float bottomZ){
		Vector3[] result = new Vector3[2];

		//		Vector3 vec1 = UtilScript.CloneVec3(linePos);
		//		Vector3 vec2 = UtilScript.CloneVec3(linePos);

//		float midX = ((topX + bottomX)/2);
//		float midZ = ((topZ + bottomZ)/2);

		float xPos = linePos.x;
		float zPos = linePos.z;
		float hypot = Mathf.Sqrt(Mathf.Pow(xPos - center.transform.position.x, 2) + Mathf.Pow(zPos - center.transform.position.z, 2));

		float hypotAdjusted = hypot * waxLengthMod - counter + offSet;


		//		Debug.Log("hypotAdjusted: " + hypotAdjusted);

		float waveHeight = Mathf.Sin(hypotAdjusted) * topY * -.75f;

		Mathf.Clamp(waveHeight, bottomY, topY);

		if(waveHeight < bottomY){
			waveHeight = bottomY;
		}

		result[0] = new Vector3(xPos, waveHeight, zPos);
		result[1] = new Vector3(xPos, waveHeight, zPos);

		return result;
	}

}
