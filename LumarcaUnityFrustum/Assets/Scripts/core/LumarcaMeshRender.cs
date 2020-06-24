using UnityEngine;
using System.Collections;

public class LumarcaMeshRender : MonoBehaviour {

	public Material material;
	public bool fastLessAccurate = false;

	public Vector3[] transformedVerts;
	public Vector3[] transformedNormals;
	public Mesh mesh;

	public bool drawDots = true;

	bool init = false;

	protected void InitMeshes(){
		if(!init){
			init = true;
			MeshFilter mFilter = GetComponent<MeshFilter>();
			
			if(mFilter != null){
				mesh = mFilter.sharedMesh;
			} else {
				mesh = new Mesh();
				mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
			}
			
			transformedVerts = new Vector3[mesh.vertices.Length];
			transformedNormals = new Vector3[mesh.normals.Length];
			
			for(int i = 0; i < transformedVerts.Length; i++){
				transformedVerts[i] = new Vector3();
				transformedNormals[i] = new Vector3();
			}
		}
	}

	void OnDrawGizmos(){
		InitMeshes();
		UpdateMesh();
	}

	// Use this for initialization
	void Start () {
		InitMeshes();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMesh();
	}

	protected void UpdateMesh(){
		bool baked = false;
		
		MeshFilter mFilter = GetComponent<MeshFilter>();
		
		if(mFilter != null){
			mesh = mFilter.sharedMesh;
		} else {
			baked = true;
			
			SkinnedMeshRenderer skin = this.GetComponent<SkinnedMeshRenderer> ();
			Mesh bakedMesh = new Mesh();
			skin.BakeMesh(bakedMesh);
			mesh = bakedMesh;
			
		}

//		mesh.RecalculateBounds();
//		mesh.RecalculateNormals();
		
		Vector3[] verts = mesh.vertices;
		Vector3[] normals = mesh.normals;
		
		Vector3 position = transform.position;
		Quaternion rot = transform.rotation;
		
		for(int i = 0; i < transformedVerts.Length; i++){
			
			transformedNormals[i] = rot * normals[i];
			if(!baked){
				transformedVerts[i] = transform.TransformPoint(verts[i]);
			} else {
				transformedVerts[i] = rot *  verts[i];
				transformedVerts[i] += position;
			}
		}
	}
}
