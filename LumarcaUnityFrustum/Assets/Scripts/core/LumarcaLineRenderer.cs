using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LumarcaLineRenderer : MonoBehaviour {

	public Material mat;
	protected float counter;
	public bool drawDots = true;

	// Use this for initialization
	void Start () {
		counter = 0;
		SetMaterial();
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
	}

	public virtual Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
	                                      float topX, float bottomX,
	                                      float topY, float bottomY,
	                                      float topZ, float bottomZ){
		List<Vector3> result = new List<Vector3>();
		
		Vector3 vec1 = UtilScript.CloneVec3(linePos);
		Vector3 vec2 = UtilScript.CloneVec3(linePos);
		
		vec1.y = topY;
		vec2.y = bottomY;
		
		result.Add(vec1);
		result.Add(vec2);

//		mat.color = new Color(1, 0, 0); 

		return result.ToArray();
	}

	public void SetMaterial(){
		if (!mat)
		{
			// Unity has a built-in shader that is useful for drawing
			// simple colored things. In this case, we just want to use
			// a blend mode that inverts destination colors.			
			var shader = Shader.Find ("Hidden/Internal-Colored");
			mat = new Material (shader);
			mat.hideFlags = HideFlags.HideAndDontSave;
			// Set blend mode to invert destination colors.
			mat.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
			mat.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			// Turn off backface culling, depth writes, depth test.
			mat.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			mat.SetInt ("_ZWrite", 0);
			mat.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
		}
	}
}
