using UnityEngine;
using System.Collections;

public class TestBake : MonoBehaviour {

	
	void Start ()
	{
		SkinnedMeshRenderer skin = this.GetComponent<SkinnedMeshRenderer> ();
		
		Mesh baked = new Mesh();
		skin.BakeMesh(baked);
		
//		var normals = new Vector3[baked.normals.Length]; baked.normals.CopyTo(normals, 0);
//		var triangles = new int[baked.triangles.Length]; baked.triangles.CopyTo(triangles, 0);
//		var vertices = new Vector3[baked.vertices.Length]; baked.vertices.CopyTo(vertices, 0);
//		
//		int i = triangle * 3;
//		
//		Vector3[] verts = new Vector3[3];
//		verts[0] = vertices [triangles [i]];
//		verts[1] = vertices [triangles [i + 1]];
//		verts[2] = vertices [triangles [i + 2]];
//		
//		if (this.transform != null)
//			for (int v = 0; v <3; v++)
//				verts [v] = this.transform.TransformPoint (verts [v]);
//		
//		return vertices;
	}

	void Update(){
		SkinnedMeshRenderer skin = this.GetComponent<SkinnedMeshRenderer> ();
		
		Mesh baked = new Mesh();
		skin.BakeMesh(baked);
	}
}