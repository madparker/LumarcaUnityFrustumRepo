using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveTestAlign  : LumarcaLineRenderer {
	
	// Use this for initialization
	void Start () {
		SetMaterial();
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
	}
	
	public override Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
	                                       float topX, float bottomX,
	                                       float topY, float bottomY,
	                                       float topZ, float bottomZ){
		Vector3[] result = new Vector3[2];
		
		Vector3 vec1 = UtilScript.CloneVec3(linePos);
		Vector3 vec2 = UtilScript.CloneVec3(linePos);
		
		float midX = ((topX + bottomX)/2);
		float midZ = ((topZ + bottomZ)/2);
		
		float xPos = linePos.x;
		float zPos = linePos.z;
		float hypot = Mathf.Sqrt(Mathf.Pow(xPos - midX, 2) + Mathf.Pow(zPos - midZ, 2));
		float hypotAdjusted = hypot - counter * 130f;

		float mod = (Mathf.Sin(linePos.x/200f + linePos.z/200f + counter/3f) + Mathf.Sin(linePos.x/200f - linePos.z/200f - counter/3f))/2f;

		float waveHeight = linePos.y + mod * topY * .4f;
		
		result[0] = new Vector3(xPos, waveHeight, zPos);
		result[1] = new Vector3(xPos, waveHeight - 50f, zPos);
		
		return result;
	}
	}
