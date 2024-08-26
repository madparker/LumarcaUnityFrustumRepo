using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LumarcaMeshRender : MonoBehaviour {

	public Material material;
	public bool fastLessAccurate = false;

	public Vector3[] transformedVerts;
	public Vector3[] transformedNormals;
	public Mesh mesh;

	public bool drawDots = true;

	bool init = false;

	//compute shader
	ComputeBuffer cbTris;
	ComputeBuffer cbVerts;
	ComputeBuffer cbNorms;
	ComputeBuffer cbResults;
	float[] resultsArray;
	List<float> results;

	public ComputeShader computeShader;
	
	public static float maxY = -99999999;

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
			
			
			//compute shader stuff
			int vector3Size = sizeof(float) * 3;
			int intSize = sizeof(int);
			int floatSize = sizeof(float);

			if (maxY < -99999998)
			{
				CameraFrustrumScript cfs = Camera.main.GetComponent<CameraFrustrumScript>();

				Vector3[] front = cfs.GetFrontPlane();
				maxY = front[2].y * 10;
			}

			resultsArray = new float[transformedVerts.Length/3]; 
			
			cbTris = new ComputeBuffer(mesh.triangles.Length, intSize);
			cbVerts = new ComputeBuffer(mesh.vertices.Length, vector3Size);
			cbNorms = new ComputeBuffer(transformedNormals.Length, vector3Size);
			cbResults = new ComputeBuffer(resultsArray.Length, floatSize);
			
			cbTris.SetData(mesh.triangles);
			cbVerts.SetData(mesh.vertices);
			cbNorms.SetData(mesh.normals);
			cbResults.SetData(resultsArray);
			
			//computeShader.SetFloat("numTris", resultsArray.Length/3);
			//computeShader.SetFloat("maxY", maxY);
    
			//computeShader.SetBuffer(0, "tris", cbTris);
			//computeShader.SetBuffer(0, "verts", cbVerts);
			//computeShader.SetBuffer(0, "norms", cbNorms);
			//computeShader.SetBuffer(0, "results", cbResults);
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
		
		if (resultsArray.Length > 0)
		{
			//computeShader data shader updating

			//computeShader execution
			//computeShader.Dispatch(0, resultsArray.Length / 32 + 1, 1, 1);

			//no need to Dispose, we want to keep this around and keep updating it

			//return results
			results = resultsArray.ToList();
		}
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

	public List<float> GetComputeShaderIntersects(Vector3 line)
	{	
		if (resultsArray.Length > 0)
		{
			//computeShader data shader updating

			//computeShader execution
			computeShader.Dispatch(0, resultsArray.Length / 32 + 1, 1, 1);

			//get results from computeShader
//			cbResults.GetData(resultsArray);

			//no need to Dispose, we want to keep this around and keep updating it

			//return results
			results = resultsArray.ToList();
		}

//		for (int i = 0; i < results.Count; i++)
//		{
//			print("results: " + i + " " + results[i]);
//		}

		return results;
	}
}
