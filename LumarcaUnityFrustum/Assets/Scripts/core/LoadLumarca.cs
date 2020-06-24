using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using SimpleJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vectrosity;

public class LoadLumarca : MonoBehaviour {

	public bool calibration;
	public static int currentLine = 0;
	bool drawAll = false;

	public float lineWidth;

	public Vector3[] pos;
	List<Vector3> posList = new List<Vector3>();
	public Material mat;

	public bool drawCube = true;

	public Vector3[] front;
	public Vector3[] back;
	
	public GameObject[] gameObjects;
	public GameObject[] lineRenderers;
	public LumarcaAnimation[] lumarcaAnimations;
	
	Vector3 down = Vector3.down;
	
	Vector3 temp = new Vector3();
	
	Vector3 vec1 = new Vector3();
	Vector3 vec2 = new Vector3();
	Vector3 vec3 = new Vector3();

	int[] tris;
	Vector3[] verts;
	Vector3[] normals;

	List<float> result;

	List<Vector3> pool;

	List<int> depthList;

	public bool debugMessages = false;

	public string jsonLineFile = "positions";

	Ray ray = new Ray();

	CameraFrustrumScript cfs;

	public bool recordName = false;

	public string recordFile;

	public LumarcaAnimation recordAnimation;
	public LumarcaFrame currentFrame;

	float frontHeight;
	float backHeight;

	bool init = false;

	public const string PP_KILL_STRING = "PP_KILL_STRING";

	int[] killStrings = new int[]{};

	public static List<int> killStringsList = new List<int>();

	public bool flipX = false;

	// Use this for initialization
	void Awake () {
		Setup();
	}

	// Use this for initialization
	void Setup() {
		if(!init){

			PlayerPrefs.SetString(PP_KILL_STRING, "");

			foreach(int i in killStrings){
				killStringsList.Add(i);
			}

			print("killStringsList:" + killStringsList.Count);

			string text = PlayerPrefs.GetString(PP_KILL_STRING, "");

			string[] splitKill = text.Split(',');

			print("text: " + text);

			for(int i = 0; i < splitKill.Length; i++){

				int num = 0;

				System.Int32.TryParse(splitKill[i], out num); //KILL STRING MIGHT ALWAYS INCLUDE 0, BECAUSE OF TryParse 

				if(!killStringsList.Contains(num)){
					killStringsList.Add(num);
				} else {
					print("Killer");
				}
			}

			for(int i = 0; i < killStringsList.Count; i++){
				int num = killStringsList[i];

				while(killStringsList.Contains(num)){
					killStringsList.Remove(num);
				}

				killStringsList.Add(num);
			}

			print("killStringsList count: " + killStringsList.Count);

			init = true;

			Cursor.visible = !Application.isEditor;

			SetMaterial();
			pool = new List<Vector3>();

			for(int i = 0; i < 10000; i++){
				pool.Add(new Vector3());
			}

			result = new List<float>();

			TextAsset asset = Resources.Load<TextAsset>(jsonLineFile);

			JObject j1 = JObject.Parse(asset.text);

			cfs = Camera.main.GetComponent<CameraFrustrumScript>();

			Vector3 projPos = new Vector3(
				(float)j1[LumarcaGenerator.PROP_PROJ_POS]["x"],
				(float)j1[LumarcaGenerator.PROP_PROJ_POS]["y"],
				(float)j1[LumarcaGenerator.PROP_PROJ_POS]["z"]);

			cfs.SetupCamera(
				(float)j1[LumarcaGenerator.PROP_PHYSICAL_WIDTH],
				projPos,
				(float)j1[LumarcaGenerator.PROP_THROW_RATIO],
				(bool)j1[LumarcaGenerator.PROP_CEILING_MOUNT]);
			
			front = cfs.GetFrontPlane();

			back = new Vector3[4];
			back[0] = LumarcaGenerator.GetFarClippingPlane(front[0]);
			back[1] = LumarcaGenerator.GetFarClippingPlane(front[1]);
			back[2] = LumarcaGenerator.GetFarClippingPlane(front[2]);
			back[3] = LumarcaGenerator.GetFarClippingPlane(front[3]);

			frontHeight = front[2].y - front[0].y;
			backHeight  = back[2].y  - back[0].y;

			JArray ja = j1["positions"] as JArray;

			for(int i = 0; i < ja.Count; i++){
				Vector3 vec = UtilScript.JsonToVector3(ja[i]);

				if(flipX){
					vec.x = -vec.x;
				}

				float lerpPer = (vec.z - front[0].z)/(back[0].z - front[0].z);

				float lineHeight = Mathf.Lerp(frontHeight, backHeight, lerpPer);

				if(cfs.ceilingMounted){
					vec.y = (front[2].y + (front[2].y - lineHeight))/2f;
				} else {
					vec.y = (front[0].y + (front[0].y + lineHeight))/2f;
				}

				posList.Add(vec);
			}

			pos = posList.ToArray();

			Debug.Log("Num Lines: " + pos.Length);

			foreach(LumarcaAnimation la in lumarcaAnimations){
				la.LoadFromJSON();
			}

			GetComponent<LumarcaAnimation>();

			if(recordName){
				recordAnimation = GetComponent<LumarcaAnimation>();
				if(recordAnimation == null){
					recordAnimation = gameObject.AddComponent<LumarcaAnimation>();
				}
			}

			VectorManager.useDraw3D = true;

	//		0.6424695

	//		VectorLine.SetRay3D (Color.green, transform.position, transform.forward * 5.0f);

	//		VectorLine line = VectorLine.SetLine(Color.green,  new Vector3[]{Vector3.left, Vector3.down});
	//		line.SetColor(Color.yellow);
	//		line.Draw3D();

	//		VectorLine.SetLine(Color.red, new Vector3[]{Vector3.left, Vector3.down});
	//
	//		vectorLinePool = new ObjectPool<VectorLine>(new ObjectGenerator<VectorLine>());
	//
	//		VectorLine myLine = vectorLinePool.GetObject();
	//
	//		myLine.SetColor(Color.blue);
	//		myLine.points3[0] = new Vector3(0,0,0);
	//		myLine.points3[1] = new Vector3(0,1,0);
	//
	//		List<int> myWidths = new List<int>(){1}; // C#
	//		myLine.SetWidths (myWidths);
		}
	}

	ObjectPool<VectorLine> vectorLinePool;

	void Update(){	

		if(Input.GetKeyDown(KeyCode.C)){
			calibration = !calibration;
		}

		if(calibration){
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				currentLine++;
			} else if(Input.GetKeyDown(KeyCode.DownArrow)){
				currentLine--;
			}

			if(Input.GetKey(KeyCode.W)){
				currentLine++;
			} else if(Input.GetKey(KeyCode.S)){
				currentLine--;
			}

			if(currentLine < 0){
				currentLine = pos.Length - 1;
			} else if(currentLine >= pos.Length){
				currentLine = 0;
			}

			if(Input.GetKeyDown(KeyCode.Space)){
				drawAll = !drawAll;
			}
		}
	}
	
	void OnPostRender() {
		Setup();

		if(calibration){
			showCalibrationLines();
		} else
			drawScene();
	}
	
	void OnDrawGizmos(){

        if (calibration){
			showCalibrationLines();
		} else {
			drawScene();
		}
	}

	void showCalibrationLines(){
		if(drawCube)
			DrawBox();

		if(depthList == null){
			makeDepthList();
		}

		Vector3 line = pos[depthList[currentLine]];

		Vector3 top = UtilScript.CloneVec3(line);
		Vector3 bottom = UtilScript.CloneVec3(line);
		
		top.y = front[0].y;
		bottom.y = front[2].y;
		
		DrawLine(top, bottom, mat);

		if(drawAll){
			foreach(Vector3 vec in pos){
				top = UtilScript.CloneVec3(vec);
				bottom = UtilScript.CloneVec3(vec);
				
				top.y = front[0].y;
				bottom.y = front[2].y;
				
				DrawLine(top, bottom, mat);
			}
		}
	}
	
	void drawScene(){

		if(recordName){
			currentFrame = new LumarcaFrame();
		}

        //		GL.PushMatrix();

        if (mat != null)
        {
            mat.SetPass(0);
        }

        if(cfs == null){
            cfs = Camera.main.GetComponent<CameraFrustrumScript>();
        }

        if (drawCube)
			DrawBox();
		
		foreach(GameObject go in gameObjects){
			drawMesh(go);
		}
			
		foreach(LumarcaAnimation la in lumarcaAnimations){
			drawAnimation(la);
		}

		float topY = front[2].y;

		foreach(GameObject go in lineRenderers){
			LumarcaLineRenderer[] llr = go.GetComponentsInChildren<LumarcaLineRenderer>();
			foreach(LumarcaLineRenderer lineRender in llr){
				if(lineRender.enabled){
					for(int lineNum = 0; lineNum < pos.Length; lineNum++){

						if(!killStringsList.Contains(lineNum)){
							Vector3 vec =pos[lineNum];

							float lerpPer = (vec.z - front[0].z)/(back[0].z - front[0].z);

							float lineHeight = Mathf.Lerp(frontHeight, backHeight, lerpPer);

							float bottomY = front[2].y - lineHeight;

							if(!cfs.ceilingMounted){
								bottomY = front[0].y;
								topY = bottomY + lineHeight;
							}

							Vector3[] points = lineRender.GenerateLine(
								lineNum, vec, 
								front[1].x, front[0].x,
								topY, bottomY,
								back[0].z,  front[0].z);
							
							for(int i = 0; i+1 < points.Length; i+=2){

								points[i].y = Mathf.Clamp(points[i].y, bottomY, topY);

								if(lineRender.drawDots){
									DrawLine(points[i], points[i+1], lineRender.mat);
								} else {
									DrawLineWithoutDots(points[i], points[i+1], lineRender.mat);
								}
							}
						}
					}
				}
			}
		}

//		GL.PopMatrix();

		if(recordName){
			print(recordName);
			recordAnimation.AddFrame(currentFrame);
		}
	}

	void drawAnimation(LumarcaAnimation la){
		LumarcaFrame lf = la.GetCurrentFrame();

		foreach(LumarcaLine ll in lf.lines){
			if(ll.hasDots){
				DrawLine(ll.top, ll.bottom, MaterialCache.GetMaterial(ll.material));
			} else {
				DrawLineWithoutDots(ll.top, ll.bottom, MaterialCache.GetMaterial(ll.material));
			}
		}
	}


	void drawMesh(GameObject rObject){

		LumarcaMeshRender[] lumarcaMeshes = rObject.GetComponentsInChildren<LumarcaMeshRender>();

		Vector3 pt1 = new Vector3();
		Vector3 pt2 = new Vector3();

		int checkNum = 0;
				
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

			for(int c = 0; c < pos.Length; c++){

				if(!killStringsList.Contains(c)){
					Vector3 vec = pos[c];

					vec.y = front[2].y * 10;
					
					pt1.x = vec.x;
					pt1.z = vec.z;
					pt2.x = vec.x;
					pt2.z = vec.z;

					if(PositionInBox(boxCollider, gObject.transform.lossyScale, gObject.transform.position, vec)){
						
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
								pt1.y = intersections[i];
								pt2.y = intersections[i + 1];
							}
								
							float lerpPer = (pt1.z - front[0].z)/(back[0].z - front[0].z);

							float lineHeight = Mathf.Lerp(frontHeight, backHeight, lerpPer);

							if(cfs.ceilingMounted){
								if(pt1.y > front[2].y){
									pt1.y = front[2].y;
								} 

								if(pt2.y > front[2].y){
									pt2.y = front[2].y;
								} 

								if(pt1.y < front[2].y - lineHeight){
									pt1.y = front[2].y - lineHeight;
								} 
								
								if(pt2.y < front[2].y - lineHeight){
									pt2.y = front[2].y - lineHeight;
								}
							} else {

								if(pt1.y > front[1].y + lineHeight){
									pt1.y = front[1].y + lineHeight;
								} 

								if(pt2.y > front[1].y + lineHeight){
									pt2.y = front[1].y + lineHeight;
								} 

								if(pt1.y < front[1].y){
									pt1.y = front[1].y;
								} 

								if(pt2.y < front[1].y){
									pt2.y = front[1].y;
								}
							}

							if(pt1.y != pt2.y){

								if(lmh.drawDots){
									DrawLine(pt1, pt2, rMat);
								} else {
									DrawLineWithoutDots(pt1, pt2, rMat);
								}
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

	RaycastHit rHit;

	bool PositionInBox2(BoxCollider box, Vector3 scale, Vector3 center, Vector3 pos){
		ray.direction.Set(0, -1, 0);
		ray.origin.Set (pos.x, pos.y, pos.z);

//		Debug.Log(pos.y * 10);

//		if (!box.Raycast(ray, out rHit)
//			inside = true;
//		else
//			inside = false;

		return box.Raycast(ray, out rHit, 10);
//		return box.bounds.IntersectRay(ray);


	}

	bool PositionInBox(BoxCollider box, Vector3 scale, Vector3 center, Vector3 pos){
		Vector3 size = box.size;
		
//		Debug.Log("Box Front: " + (center.z - size.z/2 * scale.z));
//		Debug.Log("Box Back : " + (center.z + size.z/2 * scale.z));

//		return true;
		return (center.x - size.x * scale.x * 1.1f <= pos.x && 
		        center.x + size.x * scale.x * 1.1f >= pos.x &&
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
//			mat.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
//			mat.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//			// Turn off backface culling, depth writes, depth test.
//			mat.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
//			mat.SetInt ("_ZWrite", 0);
//			mat.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
		}
	}

	void OnApplicationQuit()
	{
		if(recordName){
			print(recordName);
			recordAnimation.SaveToJSON(recordFile);
		}
	}
//	front: 0.6090493, 0.3809375, -0.6094999
//	UnityEngine.MonoBehaviour:print(Object)
//	LoadLumarca:DrawBox() (at Assets/Scripts/core/LoadLumarca.cs:458)
//	LoadLumarca:drawScene() (at Assets/Scripts/core/LoadLumarca.cs:224)
//	LoadLumarca:OnPostRender() (at Assets/Scripts/core/LoadLumarca.cs:171)

//	back: -1.015833, -0.8888542, 0.6095002
//	UnityEngine.MonoBehaviour:print(Object)
//	LoadLumarca:DrawBox() (at Assets/Scripts/core/LoadLumarca.cs:464)
//	LoadLumarca:drawScene() (at Assets/Scripts/core/LoadLumarca.cs:224)
//	LoadLumarca:OnPostRender() (at Assets/Scripts/core/LoadLumarca.cs:171)



	void DrawBox(){

		if(front.Length > 0){

//			print("front: " + front);

			//front
//			for(int i = 0; i < front.Length; i++){
//
//				if(i < front.Length - 1){
//					print("Front Measurements " + i + ": "+ 
//						"front[" + i + "]: " + ActualVector3(front[i]) + 
//						"->front[" + (i + 1) + "]: " + ActualVector3(front[(i + 1)]) + " = " + 
//						Vector3.Distance(front[i], front[i + 1]));
//				} else {
//					print("Front Measurements " + i + ": "+ 
//						"front[" + i + "]: " + ActualVector3(front[i]) + 
//						"->front[" + 0 + "]: " + ActualVector3(front[0]) + " = " + 
//						Vector3.Distance(front[i], front[0]));
//				}
//			}
//
//			//back
//			for(int i = 0; i < back.Length; i++){
//
//				if(i < back.Length - 1){
//					print("back Measurements " + i + ": "+ 
//						"back[" + i + "]: " + ActualVector3(back[i]) + 
//						"->back[" + (i + 1) + "]: " + ActualVector3(back[(i + 1)]) + " = " + 
//						Vector3.Distance(back[i], back[i + 1]));
//				} else {
//					print("back Measurements " + i + ": "+ 
//						"back[" + i + "]: " + ActualVector3(back[i]) + 
//						"->back[" + 0 + "]: " + ActualVector3(back[0]) + " = " + 
//						Vector3.Distance(back[i], back[0]));
//				}
//			}
//
//			//bottom
//			print("Bottom Measurements 0: "+ 
//				"front[0]: " + ActualVector3(front[0]) + 
//				"->front[1]: " + ActualVector3(front[1]) + " = " + 
//				Vector3.Distance(front[0], front[1]));
//
//			print("Bottom Measurements 1: "+ 
//				"front[1]: " + ActualVector3(front[1]) + 
//				"->back[1]: " + ActualVector3(back[1]) + " = " + 
//				Vector3.Distance(front[1], back[1]));
//
//			print("Bottom Measurements 2: "+ 
//				"back[0]: " + ActualVector3(back[0]) + 
//				"->back[1]: " + ActualVector3(back[1]) + " = " + 
//				Vector3.Distance(back[1], back[0]));
//
//			print("Bottom Measurements 3: "+ 
//				"back[0]: " + ActualVector3(back[0]) + 
//				"->front[0]: " + ActualVector3(front[0]) + " = " + 
//				Vector3.Distance(back[0], front[0]));
//
//			//top
//			print("Top Measurements 0: "+ 
//				"front[3]: " + ActualVector3(front[3]) + 
//				"->front[2]: " + ActualVector3(front[2]) + " = " + 
//				Vector3.Distance(front[3], front[2]));
//
//			print("Top Measurements 1: "+ 
//				"front[2]: " + ActualVector3(front[2]) + 
//				"->back[2]: " + ActualVector3(back[2]) + " = " + 
//				Vector3.Distance(front[2], back[2]));
//
//			print("Top Measurements 2: "+ 
//				"back[2]: " + ActualVector3(back[2]) + 
//				"->back[3]: " + ActualVector3(back[3]) + " = " + 
//				Vector3.Distance(back[2], back[3]));
//
//			print("Top Measurements 3: "+ 
//				"back[3]: " + ActualVector3(back[3]) + 
//				"->front[3]: " + ActualVector3(front[3]) + " = " + 
//				Vector3.Distance(back[3], front[3]));

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

		if(top.y < bottom.y){
			Vector3 temp = top;
			top = bottom;
			bottom = temp;
		}

		if(recordName){
			Debug.Log(rMat.name);
			currentFrame.AddLine(new LumarcaLine(top, bottom, rMat.name));
		}

		//Calc Dots
		float dotSize = cfs.physicalHeight/75f;

		Vector3 newTop = UtilScript.CloneVec3(top);
		newTop.y -= dotSize;
		Vector3 newBot = UtilScript.CloneVec3(bottom);
		newBot.y += dotSize;


		//Draw material lines
		rMat.SetPass(0);


		GL.Begin(GL.LINES);

		if(calibration){
			GL.Color(Color.red);
		}

		GL.Vertex(newTop);
		GL.Vertex(newBot);

		GL.End();

		//Add Dots
	
		mat.SetPass(0);

		GL.Begin(GL.LINES);
		GL.Color(Color.white);

		GL.Vertex(newTop);
		GL.Vertex(top);
		GL.Vertex(newBot);
		GL.Vertex(bottom);

		GL.End();
}
	
	void DrawLineWithoutDots(Vector3 top, Vector3 bottom){
		DrawLineWithoutDots(top, bottom, mat);
	}

	void DrawLineWithoutDots(Vector3 top, Vector3 bottom, Material mat){

		if(top.y < bottom.y){
			Vector3 temp = top;
			top = bottom;
			bottom = temp;
		}

		if(recordName){
			currentFrame.AddLine(new LumarcaLine(top, bottom, mat.name, false));
		}

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
			
			if(pointInTriangle(vec1.x, vec1.z, 
			                   vec2.x, vec2.z,
			                   vec3.x, vec3.z,
			                   vec.x, vec.z)){
				result.Add((vec1.y + vec2.y + vec3.y)/3f);
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

//		norm = getNormal(pt1, pt2, pt3);

		if(norm.y < 0){

			Vector3 temp = pt2;
			pt2 = pt1;
			pt1 = temp;
			
			norm *= -1;//getNormal(pt1, pt2, pt3);
		} 
				
		// dot product of normal and line's vector if zero line is parallel to
			// triangle
		float dotprod = norm.x * vect.x + norm.y * vect.y + norm.z * vect.z;
//		dotprod = Vector3.Dot(norm, vect);
		
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

	private static Vector3 getNormal(Vector3 pt1, Vector3 pt2, Vector3 pt3){
		
		float V1x, V1y, V1z;
		float V2x, V2y, V2z;
		Vector3 norm = new Vector3();
		
		// vector form triangle pt1 to pt2
		V1x = pt2.x - pt1.x;
		V1y = pt2.y - pt1.y;
		V1z = pt2.z - pt1.z;
		
		// vector form triangle pt2 to pt3
		V2x = pt3.x - pt2.x;
		V2y = pt3.y - pt2.y;
		V2z = pt3.z - pt2.z;
		
		// vector normal of triangle
		norm.x = (V1y * V2z - V1z * V2y);
		norm.y = (V1z * V2x - V1x * V2z);
		norm.z = (V1x * V2y - V1y * V2x);
		
		return norm;
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
		depthList = new List<int>(); 
		depthList.Add(0);
		
		for(int i = 1; i < pos.Length; i++){
			
			Vector3 current = pos[i];
			bool inserted = false;
			
			for(int c = 0; c < i; c++){
				Vector3 prev = pos[depthList[c]];
				if(current.z > prev.z){
					depthList.Insert(c, i);
					inserted = true;
					break;
				}
			}
			
			if(!inserted){
				depthList.Add(i);
			}
		}
		
		depthList.Reverse();
	}
}
