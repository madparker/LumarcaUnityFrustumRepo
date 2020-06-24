using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using SimpleJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LoadLumarcaDots : MonoBehaviour {

	public Material lumarcaMaterial;

	public bool calibration;
	public static int currentLine = 0;
	bool drawAll = false;

	List<Vector3> posList = new List<Vector3>();
	public Material mat;

	public bool drawCube = true;

	public Vector3[] front;
	public Vector3[] back;
	
	public GameObject[] gameObjects;
	public GameObject[] lineRenderers;
	
	Vector3 down = Vector3.down;
	
	Vector3 temp = new Vector3();
	
	Vector3 vec1 = new Vector3();
	Vector3 vec2 = new Vector3();
	Vector3 vec3 = new Vector3();

	int[] tris;
	Vector3[] verts;
	Vector3[] normals;

	List<float> result = new List<float>();

	List<Vector3> pool;

	List<int> depthList;

	public bool debugMessages = false;
	
	Dictionary<int, Dictionary<int, List<Vector3>>> dotsByZY;
	Dictionary<int, Dictionary<int, List<GameObject>>> quadsByZY;

	public Material transparentMaterial;
	public int dotSize = 4;

	Ray ray;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		ray = new Ray();

		BinaryReader reader = new BinaryReader(File.Open("DotPositions.json", FileMode.Open));
//		JSONNode ja = SimpleJSON.JSONNode.Deserialize(reader);
		string jsonStr = reader.ReadString();
		JObject ja = JsonConvert.DeserializeObject(jsonStr) as JObject;

		for(int i = 0; i < ja.Count; i++){
			Vector3 vec = UtilScript.JsonToVector3(ja[i]);

			posList.Add(vec);
		}

		Dictionary<int, List<Vector3>> sameDepth = new Dictionary<int, List<Vector3>>();
		dotsByZY = new Dictionary<int, Dictionary<int, List<Vector3>>>();
		quadsByZY = new Dictionary<int, Dictionary<int, List<GameObject>>>();

		//Sort all the dots at the same depth into a temporary Dictionary
		foreach(Vector3 dPos in posList){
			if(!sameDepth.ContainsKey((int)dPos.z)){
				sameDepth.Add((int)dPos.z, new List<Vector3>());
			}

			sameDepth[(int)dPos.z].Add(dPos);	          
		}

		//Sort all the dots into a Dictionary of Dictionarys, sorted by Z and Y
		//This makes rows of dots, or lines, which we can then use to check intersections
		foreach(int z in sameDepth.Keys){
			dotsByZY.Add(z, new  Dictionary<int, List<Vector3>>());

			List<Vector3> list = sameDepth[z];

			foreach(Vector3 zPos in list){
				if(!dotsByZY[z].ContainsKey((int)zPos.y)){
					dotsByZY[z].Add((int)zPos.y, new List<Vector3>());
				}
				dotsByZY[z][(int)zPos.y].Add(zPos);
			}
		}

		int totalLines = 0;

		//Count up all the lines, create a Quad for each position
		foreach(int zVal in dotsByZY.Keys){
			Dictionary<int, List<Vector3>> dict = dotsByZY[zVal];
			
			if(debugMessages){
				Debug.Log("->" + zVal + ": " + dict.Count);
			}

			quadsByZY.Add(zVal, new Dictionary<int, List<GameObject>>());

			foreach(int yVal in dict.Keys){
				List<Vector3> line = dict[yVal];
				
				if(debugMessages){
					Debug.Log("---->" + yVal + ": " + line.Count);
				}

				quadsByZY[zVal].Add(yVal, new List<GameObject>());

				foreach(Vector3 vec in line){
					GameObject q = Instantiate(Resources.Load("Quad")) as GameObject;
					q.transform.localScale = new Vector3(dotSize/2, dotSize/2, dotSize/2);
					q.transform.position = vec;

					q.SetActive(false);

					quadsByZY[zVal][yVal].Add (q);
				}

				totalLines++;
			}
		}

		if(debugMessages){
			Debug.Log("totalLines: " + totalLines);
		}

		int posCount = 0;

		CameraFrustrumScript cfs = Camera.main.GetComponent<CameraFrustrumScript>();
		
		front = cfs.GetFrontPlane();
		back = new Vector3[4];
		
		for(int i = 0; i < front.Length; i++){
			back[i] = UtilScript.CloneVec3(front[i]);
			back[i].z = Camera.main.farClipPlane - Camera.main.nearClipPlane;
		}
	}

	void Update(){	
//
//		if(Input.GetKeyDown(KeyCode.C)){
//			calibration = !calibration;
//		}
//
//		if(calibration){
//			if(Input.GetKeyDown(KeyCode.UpArrow)){
//				currentLine++;
//			} else if(Input.GetKeyDown(KeyCode.DownArrow)){
//				currentLine--;
//			}
//
//			if(Input.GetKey(KeyCode.W)){
//				currentLine++;
//			} else if(Input.GetKey(KeyCode.S)){
//				currentLine--;
//			}
//
//			if(currentLine < 0){
//				currentLine = pos.Length - 1;
//			} else if(currentLine >= pos.Length){
//				currentLine = 0;
//			}
//
//			if(Input.GetKey(KeyCode.Space)){
//				drawAll = true;
//			} else {
//				drawAll = false;
//			}
//		}
	}
	
	void OnPostRender() {
//		if(calibration){
//			showCalibrationLines();
//		} else
			drawScene();
	}
	
	void OnDrawGizmos(){
//		if(calibration){
//			showCalibrationLines();
//		} else
			drawScene();
	}

	void showCalibrationLines(){
//		if(drawCube)
//			DrawBox();
//
//		if(depthList == null){
//			makeDepthList();
//		}
//
//		Vector3 line = pos[depthList[currentLine]];
//
//		Vector3 top = UtilScript.CloneVec3(line);
//		Vector3 bottom = UtilScript.CloneVec3(line);
//		
//		top.y = front[0].y;
//		bottom.y = front[2].y;
//		
//		DrawLine(top, bottom, mat);
//
//		if(drawAll){
//			foreach(Vector3 vec in pos){
//				top = UtilScript.CloneVec3(vec);
//				bottom = UtilScript.CloneVec3(vec);
//				
//				top.y = front[0].y;
//				bottom.y = front[2].y;
//				
//				DrawLine(top, bottom, mat);
//			}
//		}
	}
	
	void drawScene(){
		
		GL.PushMatrix();
		
		mat.SetPass(0);
		
		if(drawCube)
			DrawBox();
		
		foreach(GameObject go in gameObjects){
			drawMesh(go);
		}

//		foreach(GameObject go in lineRenderers){
//			LumarcaLineRenderer[] llr = go.GetComponentsInChildren<LumarcaLineRenderer>();
//			foreach(LumarcaLineRenderer lineRender in llr){
//				if(lineRender.enabled){
//					for(int lineNum = 0; lineNum < pos.Length; lineNum++){
//						Vector3 vec =pos[lineNum];
//						Vector3[] points = lineRender.GenerateLine(lineNum, vec, 
//						                                           front[1].x, front[0].x,
//						                                           front[2].y, front[0].y,
//						                                           back[0].z,  front[0].z);
//						for(int i = 0; i+1 < points.Length; i+=2){
//							if(lineRender.drawDots){
//								DrawLine(points[i], points[i+1], lineRender.mat);
//							} else {
//								DrawLineWithoutDots(points[i], points[i+1], lineRender.mat);
//							}
//						}
//					}
//				}
//			}
//		}

		GL.PopMatrix();
	}


	void drawMesh(GameObject rObject){

//		Debug.Log("Draw Mesh");

		LumarcaMeshRender[] lumarcaMeshes = rObject.GetComponentsInChildren<LumarcaMeshRender>();

		int checkNum = 0;

		Vector3 vec = new Vector3(front[0].x * 10, 0, 0);
				
		for(int g = 0; g < lumarcaMeshes.Length; g++){
			
			LumarcaMeshRender lmh = lumarcaMeshes[g];
//			Mesh mesh = filter.sharedMesh;

			GameObject gObject = lmh.gameObject;

			BoxCollider boxCollider = lmh.GetComponent<BoxCollider>();

			verts = lmh.GetComponent<LumarcaMeshRender>().transformedVerts;
			normals = lmh.GetComponent<LumarcaMeshRender>().transformedNormals;
			
			bool fast = lmh.GetComponent<LumarcaMeshRender>().fastLessAccurate;
			
			tris = lmh.mesh.triangles;

			Material rMat = mat;
			
			if(lmh != null){
				rMat = lmh.material;
			}

			foreach(int zVal in dotsByZY.Keys){

				
//				Debug.Log("zVal: " + zVal);

				Dictionary<int, List<Vector3>> dict = dotsByZY[zVal];
				foreach(int yVal in dict.Keys){

					
					List<Vector3> lineDots = dotsByZY[zVal][yVal];
					for(int ldNum = 0; ldNum < lineDots.Count; ldNum++){
						quadsByZY[zVal][yVal][ldNum].SetActive(false);
					}
					
//					Debug.Log("yVal: " + yVal);

					vec.y = yVal;
					vec.z = zVal;

					float x1, x2 = 0;

					if(PositionInBox2(boxCollider, gObject.transform.lossyScale, gObject.transform.position, vec)){

//						Debug.Log("in box");

						List<float> intersections;
						if(!fast){
							intersections = GetIntersectList(vec);
						} else {
							intersections = GetIntersectListFast(vec);
						}

						checkNum++;

						intersections.Sort();

						for(int i = 0; i < intersections.Count; i+=2){

							if(i+1 >= intersections.Count){
								if(debugMessages){
									Debug.Log("UNEVEN!!! " + intersections.Count);
								}
								break;
							} else {
								x1 = intersections[i];
								x2 = intersections[i + 1];
							}
							
							if(x1 > front[1].x){
								x1 = front[1].x;
							} 

							if(x2 > front[1].x){
								x2= front[1].x;
							} 
							
							if(x1 < front[0].x){
								x1 = front[0].x;
							} 
							
							if(x2 < front[0].x){
								x2 = front[0].x;
							} 

							if(x1 != x2){

								for(int ldNum = 0; ldNum < lineDots.Count; ldNum++){
									Vector3 lineDot = lineDots[ldNum];

									if(lineDot.x >= x1 && lineDot.x <= x2){
										quadsByZY[zVal][yVal][ldNum].SetActive(true);
										quadsByZY[zVal][yVal][ldNum].GetComponent<MeshRenderer>().material = rMat;
									} 
									else if(lineDot.x >= x1 - 50 && lineDot.x <= x2 + 50){
										quadsByZY[zVal][yVal][ldNum].SetActive(true);
										quadsByZY[zVal][yVal][ldNum].GetComponent<MeshRenderer>().material = mat;
									}
									else {
//										quadsByZY[zVal][yVal][ldNum].SetActive(false);
									}
								}

//								if(lmh.drawDots){
//									DrawLine(pt1, pt2, rMat);
//								} else {
//									DrawLineWithoutDots(pt1, pt2, rMat);
//								}
							}
						}
					}
				}
			}
		}
		
//		Debug.Log("checkNum: " + checkNum * mesh.vertexCount/3);
//		Debug.Log("normals: " + mesh.vertices.Length);
//		Debug.Log("tris: " + mesh.normals.Length);
	}

	bool PositionInBox2(BoxCollider box, Vector3 scale, Vector3 center, Vector3 pos){
		ray.direction.Set(1, 0, 0);
		ray.origin.Set (front[0].x * 10, pos.y, pos.z);

		return box.bounds.IntersectRay(ray);

//
//		Vector3 size = box.size;
//		return (
//			center.y + size.y * scale.y * 1.1f >= pos.y &&
//			center.z - size.z * scale.z * 1.1f <= pos.z && 
//			center.z + size.z * scale.z * 1.1f >= pos.z);
	}

	bool PositionInBox(BoxCollider box, Vector3 scale, Vector3 center, Vector3 pos){
		Vector3 size = box.size;
		
//		Debug.Log("Box Front: " + (center.z - size.z/2 * scale.z));
//		Debug.Log("Box Back : " + (center.z + size.z/2 * scale.z));

//		return true;
		return (
//				center.y - size.y * scale.y * 1.1f <= pos.y && 
		        center.y + size.y * scale.y * 1.1f >= pos.y &&
		        center.z - size.z * scale.z * 1.1f <= pos.z && 
		        center.z + size.z * scale.z * 1.1f >= pos.z);
	}

	void SetMaterial(){
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

	void DrawBox(){

		if(front.Length > 0){
			DrawLine(front[0], front[1]);
			DrawLine(front[1], front[2]);
			DrawLine(front[2], front[3]);
			DrawLine(front[0], front[3]);

			DrawLine(back[0], back[1]);
			DrawLine(back[1], back[2]);
			DrawLine(back[2], back[3]);
			DrawLine(back[0], back[3]);
			
			DrawLine(back[0], front[0]);
			DrawLine(back[1], front[1]);
			DrawLine(back[2], front[2]);
			DrawLine(back[3], front[3]);
		}
	}
	void DrawLine(Vector3 top, Vector3 bottom){
		DrawLine(top, bottom, mat);
	}

	void DrawLine(Vector3 top, Vector3 bottom, Material rMat){

		rMat.SetPass(0);
		
		GL.Begin(GL.LINES);

		if(calibration){
			GL.Color(Color.red);
		}
		GL.Vertex(top);
		GL.Vertex(bottom);
		
		GL.End();

		mat.SetPass(0);
		
		Vector3 top2 = UtilScript.CloneVec3(top);
		top2.y += 10;
		Vector3 bottom2 = UtilScript.CloneVec3(bottom);
		bottom2.y -= 10;
		
		GL.Begin(GL.LINES);
		GL.Color(Color.white);
		
		GL.Vertex(top);
		GL.Vertex(top2);
		
		GL.End();
		
		GL.Begin(GL.LINES);
		GL.Color(Color.white);
		
		GL.Vertex(bottom);
		GL.Vertex(bottom2);
		
		GL.End();
	}
	
	void DrawLineWithoutDots(Vector3 top, Vector3 bottom){
		DrawLine(top, bottom, mat);
	}

	void DrawLineWithoutDots(Vector3 top, Vector3 bottom, Material mat){

		mat.SetPass(0);

		GL.Begin(GL.LINES);

		GL.Vertex(top);
		GL.Vertex(bottom);
		
		GL.End();
	}

	//TODO calc vert positions once, save performance

	List<float> GetIntersectList(Vector3 vec){
		result.Clear();

		for(int i = 0; i < tris.Length; i+=3){
			
			int vert1 = tris[i];
			int vert2 = tris[i + 1];
			int vert3 = tris[i + 2];
						
			vec1 = verts[vert1];
			vec2 = verts[vert2];
			vec3 = verts[vert3];
			
			Vector3 inter = UtilScript.CloneVec3(vec);
			inter.y = front[2].y * 10;

			Vector3 normal = (normals[vert1] + normals[vert2] + normals[vert3])/3;

			inter = checkIntersectTri(vec1, vec2, vec3, inter, down, temp, normal);
			
			if(inter.y > -1001){
				result.Add(inter.y);
			}
		}
		
		return result;
	}

	List<float> GetIntersectListFast(Vector3 vec){
		result.Clear();

		for(int i = 0; i < tris.Length; i+=3){
			
			int vert1 = tris[i];
			int vert2 = tris[i + 1];
			int vert3 = tris[i + 2];

			vec1 = verts[vert1];
			vec2 = verts[vert2];
			vec3 = verts[vert3];
			
			if(pointInTriangle(vec1.y, vec1.z, 
			                   vec2.y, vec2.z,
			                   vec3.y, vec3.z,
			                   vec.y, vec.z)){
				result.Add((vec1.x + vec2.x + vec3.x)/3f);
			}
		}

		return result;
	}

	bool pointInTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float x, float y)
	{
		float denominator = ((y2 - y3)*(x1 - x3) + (x3 - x2)*(y1 - y3));
		float a = ((y2 - y3)*(x - x3) + (x3 - x2)*(y - y3)) / denominator;
		float b = ((y3 - y1)*(x - x3) + (x1 - x3)*(y - y3)) / denominator;
		float c = 1 - a - b;
		
		return 0 <= a && a <= 1 && 0 <= b && b <= 1 && 0 <= c && c <= 1;
	}
	
	Vector3 checkIntersectTri(Vector3 pt1, Vector3 pt2, Vector3 pt3,
	                          Vector3 linept, Vector3 vect, Vector3 result, Vector3 norm) {
	
		result.y = -10000;

		if(norm.y < 0){

			Vector3 temp = pt2;
			pt2 = pt1;
			pt1 = temp;
			
			norm *= -1;
		} 
				
		// dot product of normal and line's vector if zero line is parallel to
			// triangle
		float dotprod = norm.x * vect.x + norm.y * vect.y + norm.z * vect.z;
		
		if (dotprod < 0) {

			// Find point of intersect to triangle plane.
			// find t to intersect point
			float t =
				-(norm.x * (linept.x - pt1.x) + norm.y * (linept.y - pt1.y) + norm.z
			      * (linept.z - pt1.z))
				/ (norm.x * vect.x + norm.y * vect.y + norm.z * vect.z);
			
			// if ds is neg line started past triangle so can't hit triangle.
			if (t < 0){
				return result;
			}
			
			result.x = linept.x + vect.x * t;
			result.y = linept.y + vect.y * t;
			result.z = linept.z + vect.z * t;

			if (checkSameClockDir(pt1, pt2, result, norm) &&
			    checkSameClockDir(pt2, pt3, result, norm) &&
			    checkSameClockDir(pt3, pt1, result, norm)) {
						// answer in pt_int is inside triangle
						return result; // ANSWER
			}
		}

		result.Set(0, -10000, 0);
		return result;
	}


	private  bool checkSameClockDir(Vector3 pt1, Vector3 pt2,
	                                      Vector3 pt3, Vector3 norm) {

		// normal of trinagle
		float testi = (((pt2.y - pt1.y) * (pt3.z - pt1.z)) - ((pt3.y - pt1.y) * (pt2.z - pt1.z)));
		float testj = (((pt2.z - pt1.z) * (pt3.x - pt1.x)) - ((pt3.z - pt1.z) * (pt2.x - pt1.x)));
		float testk = (((pt2.x - pt1.x) * (pt3.y - pt1.y)) - ((pt3.x - pt1.x) * (pt2.y - pt1.y)));
		
		// Dot product with triangle normal
		float dotprod = testi * norm.x + testj * norm.y + testk * norm.z;
		
		// answer
		if (dotprod < 0)
			return false;
		else
			return true;
	}

	private void makeDepthList(){
//		depthList = new List<int>(); 
//		depthList.Add(0);
//		
//		for(int i = 1; i < pos.Length; i++){
//			
//			Vector3 current = pos[i];
//			bool inserted = false;
//			
//			for(int c = 0; c < i; c++){
//				Vector3 prev = pos[depthList[c]];
//				if(current.z > prev.z){
//					depthList.Insert(c, i);
//					inserted = true;
//					break;
//				}
//			}
//			
//			if(!inserted){
//				depthList.Add(i);
//			}
//		}
//		
//		depthList.Reverse();
	}
}
