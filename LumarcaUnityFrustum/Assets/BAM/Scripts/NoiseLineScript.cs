using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseLineScript : LumarcaLineRenderer {
	public float waxLengthMod = 30;
	public float speedMod;

	public Color[] colors;

	public bool ceilingMounted;

	// Use this for initialization
	void Start () {
//		SetMaterial();
//		mat.color = new Color(1, 0, 0);
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

		float xPos = linePos.x;
		float zPos = linePos.z;


		//		Debug.Log("hypotAdjusted: " + hypotAdjusted);

		float waveHeight = UtilScript.Map(Mathf.PerlinNoise(xPos/waxLengthMod + counter, zPos/waxLengthMod  + counter), 0, 1, bottomY, topY);

		result[0] = new Vector3(xPos, waveHeight, zPos);
		result[1] = new Vector3(xPos, topY, zPos);

		if(!ceilingMounted){
			result[1] = new Vector3(xPos, bottomY, zPos);
		}

		float range = topY - bottomY;

		float split = range/colors.Length;

		for(int i = 0; i < colors.Length - 1; i++){
			float currentTop = bottomY + i * split * range;

			if(currentTop < waveHeight){

				mat.SetColor("_Color1", colors[i]);
				mat.SetColor("_Color2", colors[i + 1]);
				mat.SetFloat("_LerpPer", (waveHeight - currentTop)/split);

//				mat = colors[colors.Length - i - 1];
			}
		}

//		if(waveHeight > topY * .1f){
//			mat = mat1;
//		} else if (waveHeight > bottomY * .1f){
//			mat = mat2;
//		} else {
//			mat = mat3;
//		}

		return result;
	}

}
