using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhaleWaves : LumarcaLineRenderer {

	// Use this for initialization
	void Start () {
		SetMaterial();
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime/2f;
	}

	public override Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
	                                      float topX, float bottomX,
	                                      float topY, float bottomY,
	                                      float topZ, float bottomZ){
		List<Vector3> result = new List<Vector3>();
		
		Vector3 vec1 = UtilScript.CloneVec3(linePos);
		Vector3 vec2 = UtilScript.CloneVec3(linePos);

		vec1.y = bottomY + 0.725f + Mathf.PerlinNoise(counter + linePos.x * 2, counter + linePos.z * 2) * 0.1f;
		vec2.y = vec1.y - 0.025f;
		
		result.Add(vec1);
		result.Add(vec2);
		
		float col = UtilScript.Map(vec1.y, linePos.y - 1, linePos.y + 1, 0, 1);
		
		//		Debug.Log(col);
		mat.color = new Color(0.15f, 0.35f, 0.5f, 1); 
		
		return result.ToArray();
	}
}
