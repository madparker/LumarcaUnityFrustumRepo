using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshChecker : MonoBehaviour {
	
	Mesh mesh;
	
	Vector3[] saveVerts;
	Vector3[] saveNorms;
	float[] vertMods1;
	float[] vertMods2;
	float[] vertCurrent;
	float[] holder;
	
	public float splashDepth = 1;
	public float sludgeFactor = 0.75f;
	public float rippleFlow = 0.75f;
	public float stepSpeed = 0.1f;

	float stepLerp = 0;

	List<List<int>> connectedVerts = new List<List<int>>();

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		
		Vector3[] verts = mesh.vertices;
		Vector3[] norms = mesh.normals;
		
		vertMods1 = new float[verts.Length];
		vertMods2 = new float[verts.Length];
		vertCurrent = new float[verts.Length];
		
		saveVerts = new Vector3[verts.Length];
		saveNorms = new Vector3[norms.Length];

		for(int i = 0; i < verts.Length; i++){
			
			vertMods1[i] = 0;
			vertMods2[i] = 0;
			vertCurrent[i] = 0;

			saveVerts[i] = new Vector3(verts[i].x, verts[i].y, verts[i].z);
			saveNorms[i] = new Vector3(norms[i].x, norms[i].y, norms[i].z);

			connectedVerts.Add(new List<int>());
		}

		for(int i = 0; i < mesh.triangles.Length; i+=3){
			
			int vert1 = mesh.triangles[i];
			int vert2 = mesh.triangles[i + 1];
			int vert3 = mesh.triangles[i + 2];
			
			insertVerts(vert1, vert2, vert3);
			insertVerts(vert2, vert1, vert3);
			insertVerts(vert3, vert1, vert2);
		}

		for(int node = 0 ; node < connectedVerts.Count; node++){
			for(int connection = 0 ; connection < connectedVerts[node].Count; connection++){
//				Debug.Log("Node: " + node + " connection: " + connectedVerts[node][connection]);
			}
		}

		mesh.vertices = verts;

		Debug.Log(verts.Length);

		InvokeRepeating("splashTime", 2, 10);

		stepLerp = stepSpeed;

	}

	public void splashTime(){
		splash();
	}

	public void insertVerts(int pos, int vert1, int vert2){
		if(!connectedVerts[pos].Contains(vert1)){
			connectedVerts[pos].Add(vert1);
		}
		if(!connectedVerts[pos].Contains(vert2)){
			connectedVerts[pos].Add(vert2);
		}
	}

	// Update is called once per frame
	void Update () {

//		if(Time.frameCount%16 == 0){
			calcSplash();
//		}

//		mesh = GetComponent<MeshFilter>().mesh;
//		
//		Vector3[] verts = mesh.vertices;
//		
//		for(int i = 0; i < verts.Length; i++){
//			verts[i] = saveVerts[i] + saveNorms[i] * Mathf.Sin(verts[i].z + Time.realtimeSinceStartup) * 10;
//		}
//		
//		mesh.vertices = verts;
	}


	public void calcSplash(){
		
		Vector3[] verts = mesh.vertices;

		if(stepLerp == stepSpeed){

			float maxMod = 0;
			
			for (int i = 0; i < verts.Length; i++) {

				List<int> surroundingVerts = connectedVerts[i];

				float suroundTotal = 0; 

				float highPt = 0;

				for(int c = 0; c < surroundingVerts.Count; c++){
					suroundTotal += vertMods2[surroundingVerts[c]];

					if(highPt <  vertMods2[surroundingVerts[c]]){
						highPt = vertMods2[surroundingVerts[c]];
					}
				}

				float mod = highPt * sludgeFactor - vertMods1[i];

				if(mod > maxMod){
					maxMod = mod;
				}

				vertMods1[i] = vertMods2[i] * (1 - rippleFlow) + mod * rippleFlow;

//				verts[i] = saveVerts[i] + (saveNorms[i] * vertMods1[i]);

			}

			Debug.Log(maxMod);

//			mesh.vertices = verts;
//			mesh.RecalculateNormals();
			
			holder = vertMods2;
			vertMods2 = vertMods1;
			vertMods1 = holder;

			stepLerp = 0;
		}

		stepLerp += Time.deltaTime;

		if(stepLerp >= stepSpeed){
			stepLerp = stepSpeed;
			Debug.Log("HERE");
		}

		Debug.Log("stepLerp/stepSpeed: " + stepLerp/stepSpeed);

		for (int i = 0; i < verts.Length; i++) {
			verts[i] = saveVerts[i] + 
				(saveNorms[i] * Mathf.Lerp(vertMods2[i], vertMods1[i], 1 - stepLerp/stepSpeed));
		}

		
		mesh.vertices = verts;
		mesh.RecalculateNormals();
	}
	
	private void splash() {
		vertMods2[vertMods2.Length/2] = splashDepth;
//		vertMods2[vertMods2.Length/2 + 1] = splashDepth;
//		vertMods2[vertMods2.Length/2 - 1] = splashDepth;
//		for (int dx = -splashWidth; dx < splashWidth; dx++) {
//			for (int dy = -splashWidth; dy < splashWidth; dy++){
//				int index = x + dx + (y + dy)  * width;
//				if(index > 0 && index < pt2.length)
//					pt2[index] += splashDepth;
//			}
//		}
	}
}
