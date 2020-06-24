using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using SimpleJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LumarcaGenerator : MonoBehaviour {

	Camera cam;
	CameraFrustrumScript cfs;

	public bool part1 = true;

	public int numDepths = 10;
	public float yarnWidth = 0.005f;
	public Vector3[] front;
	public Vector3[] back;

	public bool drawFrame;
	public GameObject dotHolder;

	public int lineConfiguration;
	public int gridDivide = 4;
	
	float totalWidth;
	float frontHeight;
	float backHeight;
	float totalDepth;
	
	const string SHADER_MODE = "_Mode";

	public enum LUMARCA_MODE {Dots, VLines};
	public LUMARCA_MODE lumarcaMode;
	
	public Material mat;
	
	public List<Vector3> lines;
	public List<Vector3> dots;
	public List<GameObject> lineObjects;
	public List<int> depthList;

	public static float tolerance = 0.2f;
	List<int> bag;
	
	Vector3 camPos;

	public string fileNamePre = "";

	public const string PROP_LUMARCA_SIZE = "lumarcaSize";
	public const string PROP_PROJ_POS = "ProjPos";
	public const string PROP_THROW_RATIO = "ThrowRatio";
	public const string PROP_PHYSICAL_WIDTH = "PhysicalWidth";
	public const string PROP_CEILING_MOUNT = "CeilingMount";
	public const string PROP_POSITIONS = "positions";

	private Vector3 mins = new Vector3(1000, 1000, 1000);
	private Vector3 maxs = new Vector3(-1000, -1000, -1000);


	// Use this for initialization
	void Start () {

//		float input = 237.5654665f;
//		
//		float part1 = GetColorPart1(input, 0, 1024);
//		float part2 = GetColorPart2(input, 0, 1024);
//
//		Debug.Log ("MAP1: " + part1);
//		Debug.Log ("MAP2: " + part2);
//		Debug.Log ("Convert: " + PartsToFloat(part1, part2, 0, 1024));

		cam = Camera.main;

		cfs = cam.GetComponent<CameraFrustrumScript>();
	
		front = cfs.GetFrontPlane();
			
		back = new Vector3[4];
		back[0] = GetFarClippingPlane(front[0]);
		back[1] = GetFarClippingPlane(front[1]);
		back[2] = GetFarClippingPlane(front[2]);
		back[3] = GetFarClippingPlane(front[3]);

		totalWidth = front[1].x - front[0].x;

		frontHeight = front[2].y - front[0].y;
		backHeight  = back[2].y  - back[0].y;

		totalDepth = back[0].z  - front[0].z;
		
		Debug.Log("totalWidth: " + totalWidth);
		Debug.Log("totalHeight: " + frontHeight);
		Debug.Log("totalDepth: " + totalDepth);

		camPos = cam.transform.position;

		gridDivide = (int)Mathf.Ceil(Mathf.Sqrt(numDepths));
//		gridDivide = lineConfiguration/numDepths;
	

		GenerateLines();
//		MultiPerDepthAndCutOutsideOfCube();

		Debug.Log("Lines: " + lines.Count);

		SetColorToPart();
	}

	public static Vector3 GetFarClippingPlane(Vector3 frontPos){

		Camera camera = Camera.main;

		Vector3 dir = frontPos - camera.transform.position;

		dir.Normalize();

		float zDepth = camera.transform.position.z + camera.farClipPlane;

		float mult = camera.farClipPlane/dir.z;

		return new Vector3(
			camera.transform.position.x + dir.x*mult, 
			camera.transform.position.y + dir.y*mult, zDepth);

	}

	List<int> MakeBag(){
		List<int> result = new List<int>();

		for(int i = 0; i < numDepths; i++){
			result.Add (i);
		}

		return result;
	}

	bool TooClose(Vector3 coord1, Vector3 coord2){
		
		if(Vector3.Distance(coord1, coord2) < tolerance){
			Debug.Log("TOO CLOSE");
			return true;
		}
		
		return false;
	}

	private bool ValidLine(Vector3 pos){
		bool result = true;
		
		for(int i = 0 ; i < lines.Count; i++){
			if(TooClose(lines[i], pos)){
				result = false;
				break;
			}
		}
		
		return result;
	}


	Vector3 GetNewLinePos(int depthPos, float x, Vector3 camPos, float depthIncrement){
//		int i = (int)(Random.Range(0, bag.Count));

		if(depthPos == -1000){
			return new Vector3();
		}

//		Debug.Log("depthPos: " + depthPos + " bag: " + bag.Count + " depthIncrement: " + depthIncrement);

		int depthLevel = bag[depthPos%bag.Count];
		
		float z = depthLevel * depthIncrement + front[0].z;
		
		Vector3 lineSpotAtFront = new Vector3(x, 0, front[0].z);
		
		Vector3 dir =  lineSpotAtFront - camPos;
		dir.Normalize();
		
		float distToSpot = (z - camPos.z)/dir.z;
		
		float zMult = distToSpot;

//		print("z: " + z);
		
		return new Vector3(dir.x * zMult, 0, z);
	}

	void GenerateLines(){
		float depthIncrement = totalDepth/(numDepths - 1);
		
		dotHolder = new GameObject("LineHolder");
		
		GameObject[] depthHolders = new GameObject[numDepths];
		
		for(int i = 1; i <= numDepths; i++){
			GameObject go = new GameObject("Depth " + i);
			go.transform.parent = dotHolder.transform;
			depthHolders[i-1] = go;
		}

		bag = MakeBag();
		
		lines = new List<Vector3>();
		lineObjects = new List<GameObject>();

		int index = 0;

		int maxAttempts = 10;

//		int temp = bag.Count - 1;

//		int[,] depths = new int[gridDivide, lineConfiguration/(gridDivide - 1)];
		List<List<int>> depths = new List<List<int>>();

		for(int gridPos = 0; gridPos < gridDivide; gridPos++){
			depths.Add(new List<int>());
		}

		for(int x = 0; x < lineConfiguration; x++){
//			Debug.Log("x: " + x + " depths.Count: " + depths.Count + " (x/gridDivide)%gridDivide: " + [(x/gridDivide)%gridDivide);
			depths[(x/gridDivide)%gridDivide].Add(x);
		}

		List<int> gridPickingOrder = new List<int>();

		for(int i = 0; i < gridDivide; i+=2){
			gridPickingOrder.Add(i);
		}

		for(int i = 1; i < gridDivide; i+=2){
			gridPickingOrder.Add(i);
		}

//		string orderStr = "";
//		for(int i = 0; i < gridDivide; i++){
//			orderStr += gridPickingOrder[i] + " ";
//		}
//
//		print("gridPickingOrder: " + orderStr);

		int lineSplit = (lineConfiguration/gridDivide);
		int interval = lineSplit/gridDivide;

//		Debug.Log("lineSplit: " + lineSplit);

		CameraFrustrumScript cfs = Camera.main.GetComponent<CameraFrustrumScript>();

		float lineSpacing = cfs.physicalWidth/lineConfiguration;

		int lineNum = 0;

		for(float x = front[0].x + lineSpacing/2; x < front[1].x; x += lineSpacing){
			int attempts = 0;

			int gridIndex = lineNum%gridPickingOrder.Count;

			int gridPos = gridPickingOrder[gridIndex];
			int segPos = (depths[gridPos].Count)/2;

			if((lineNum/gridDivide)%2 != 0){
				segPos = 0;
			}

//			Debug.Log("depths: " + depths.Count);

			while(depths[gridPos].Count == 0){

				lineNum++;

				gridIndex = lineNum%gridPickingOrder.Count;

				gridPos = gridPickingOrder[gridIndex];
				segPos = (depths[gridPos].Count - 1)/2;
			}

			int depthPos = depths[gridPos][segPos];
			
			Vector3 pos = GetNewLinePos(depthPos, x, camPos, depthIncrement);

			int trys = 0;


			depths[gridPos].RemoveAt(segPos);

			lines.Add(pos);

			index++;
				
			GameObject q = Instantiate(Resources.Load("Quad")) as GameObject;

			q.transform.parent = dotHolder.transform;

			float lerpPer = (pos.z - front[0].z)/(back[0].z - front[0].z);

			float lineHeight = Mathf.Lerp(frontHeight, backHeight, lerpPer);
            
            if(cfs.ceilingMounted) //if ceiling ceiling mounted
            { 
                lineHeight = -lineHeight; //flip the lineHeight down
            }

			pos.y = Camera.main.transform.position.y + lineHeight/2;

//			print("pos.y: " + pos.y);

			q.transform.localScale = new Vector3(yarnWidth, lineHeight, yarnWidth);
			q.transform.position = pos;

			lineObjects.Add(q);
			lineNum++;
		}
		
//		Debug.Log ("tolerance: " + tolerance);
//		Debug.Log ("Num Lines: " + index);
//		Debug.Log ("lineObjects[0]: " + lineObjects[0].transform.position);
//		Debug.Log ("lines[0]: " + lines[0]);
	}

	void CutOutSideOfCube(){
		
//		Debug.Log("objects Length: " + lineObjects.Count);
		
		List<Vector3> removeVecs = new List<Vector3>();
		List<GameObject> removeGameObjects = new List<GameObject>();

		for(int i = 0; i < lines.Count; i++){
			Vector3 pos = lines[i];

			if(!InsideCube(pos)){
				removeVecs.Add(pos);
			}
			
			if(!InsideCube(lineObjects[i].transform.position)){
				removeGameObjects.Add(lineObjects[i]);
			}
		}
		
//		Debug.Log("removeVecs Length: " + removeVecs.Count);
//		Debug.Log("GameObject Length: " + removeGameObjects.Count);

		for(int i = 0; i < removeVecs.Count; i++){
			Vector3 pos = removeVecs[i];
			lines.Remove(pos);
			
			GameObject o = removeGameObjects[i];
			lineObjects.Remove(o);
			Destroy(o);
		}
		
//		Debug.Log("Lines Length: " + lines.Count);
//		Debug.Log("GameObject Length: " + lineObjects.Count);
	}

	void MakeDiscreet(){
		makeDepthList();
		
		for(int i = 0; i < lines.Count; i++){
			
			int whichString = depthList[i];
			
			Vector3 line = lines[whichString];

//			Debug.Log("i: " + i);

			lines[whichString] = GetPosAtNewZForLines(line, Mathf.Lerp(front[0].z, 
			                                                   back[0].z, 
			                                                   i/(float)lines.Count));

			lineObjects[whichString].transform.position = lines[whichString];
		}
	}
		
	void MakeDiscreetAndCutOutsideOfCube(){
		makeDepthList();

		for(int i = 0; i < lines.Count; i++){

			int whichString = depthList[i];

			Vector3 line = lines[whichString];

			// Get which slice of depth you are currently in. For example, slicke 50 out of 100 depths
			// So you don't end up at the very from (ie slice 0) or the very back (ie slice 100)
			// where you are unlikely to be able to place a string because of the frame getting in the way
			// we add 2 to the number of depths add +1 to slice (so we'll always get atleast slice 1)
			// and we add +2 to the depths, so for 100 to slices, our values will actually range
			// from 1-100 (out of 101) instead of 0-99 (out of 99)
			int depthSlice = (int)Mathf.Floor((i/(float)lines.Count) * (numDepths)) + 1;

//			Debug.Log("depthSlice: "  + depthSlice);

			float depth = Mathf.Lerp(front[0].z, back[0].z, depthSlice/(float)(numDepths + 1));

			lines[whichString] = GetPosAtNewZForLines(line, depth);

			lineObjects[whichString].transform.position = lines[whichString];
			lineObjects[whichString].transform.parent = GameObject.Find("Depth " + depthSlice).transform;
		}
	}

	void MultiPerDepthAndCutOutsideOfCube(){

		int numLines = lines.Count;
		MakeDiscreetAndCutOutsideOfCube();

//		CutOutSideOfCube();
//
//		while(numLines !=  lines.Count){
//			Debug.Log("IN While1");
//			MakeDiscreetAndCutOutsideOfCube();
//			numLines = lines.Count;
//			CutOutSideOfCube();
//		}

		Analyze();
	}

	private void Analyze(){
		int[] sector = new int[9];

		for(int i = 1; i < lines.Count; i++){
			Vector3 line = lines[i];

			if(line.x > maxs.x){
				maxs.x = line.x;
			}
			if(line.y > maxs.y){
				maxs.y = line.y;
			}
			if(line.z > maxs.z){
				maxs.z = line.z;
			}


			if(line.x < mins.x){
				mins.x = mins.x;
			}
			if(line.y < mins.y){
				mins.y = mins.y;
			}
			if(line.z < maxs.z){
				mins.z = mins.z;
			}

			if(line.x <= -totalWidth * (0.33f/2f)){
				if(line.z <= -totalDepth * (0.33f/2f)){
					sector[0]++;
				} else if (line.z >= totalDepth * (0.33f/2f)){
					sector[2]++;
				} else {
					sector[1]++;
				}
			} else if (line.x > totalWidth * (0.33f/2f)){
				if(line.z <= -totalDepth* (0.33f/2f)){
					sector[6]++;
				} else if (line.z >= totalDepth * (0.33f/2f)){
					sector[8]++;
				} else {
					sector[7]++;
				}
			} else {
				if(line.z <= -totalDepth * (0.33f/2f)){
					sector[3]++;
				} else if (line.z >= totalDepth * (0.33f/2f)){
					sector[5]++;
				} else {
					sector[4]++;
				}
			}
		}

		print("mins: " + mins);
		print("maxs: " + maxs);

		
//		for(int i = 0; i < sector.Length; i++){
//			Debug.Log("Sector " + i + ": " + sector[i]);
//		}
	}

	private void makeDepthList(){
		depthList = new List<int>(); 
		depthList.Add(0);
		
		for(int i = 1; i < lines.Count; i++){
			
			Vector3 current = lines[i];
			bool inserted = false;
			
			for(int c = 0; c < i; c++){
				Vector3 prev = lines[depthList[c]];
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

	
	Vector3 GetPosAtNewZ(Vector3 pos, float newZ){
		
		Vector3 newPos = pos - camPos;
		
		newPos.Normalize();
		
		float f = ((newZ - camPos.z)/newPos.z);
		
		newPos = camPos + newPos * f;
		
		return newPos;
	}

	//NOTE: DO NOT USE FOR DOTS
	Vector3 GetPosAtNewZForLines(Vector3 pos, float newZ){

		Vector3 newPos = GetPosAtNewZ(pos, newZ);
	
		newPos.y = pos.y;

		return newPos;
	}

	void SetColorToPart(){
	
		Color c = new Color(0, 1, 0, 1);

		Debug.Log("DOT HOLDER: " + dotHolder.transform.childCount);
		
		for(int i = 0; i < dotHolder.transform.childCount; i++){
			GameObject depth = dotHolder.transform.GetChild(i).gameObject;

//			Debug.Log("Depth: " + i + " Num Dots: " + depth.transform.childCount);

			for(int j = 0; j < depth.transform.childCount; j++){
				GameObject dot = depth.transform.GetChild(j).gameObject;

				Vector3 pos = dot.transform.position;

				if(part1){
					c.r = GetColorPart1(pos.x, back[0].x,  back[1].x);
					c.g = GetColorPart1(pos.y, back[0].y,  back[2].y);
					c.b = GetColorPart1(pos.z, front[0].z, back[0].z);
				} else {
					c.r = GetColorPart2(pos.x, back[0].x,  back[1].x);
					c.g = GetColorPart2(pos.y, back[0].y,  back[2].y);
					c.b = GetColorPart2(pos.z, front[0].z, back[0].z);
				}
				
				if(part1){
					dot.GetComponent<MeshRenderer>().material.SetInt(SHADER_MODE, 0);
				} else {
					dot.GetComponent<MeshRenderer>().material.SetInt(SHADER_MODE, 1);
				}
				
	//			c.r = 0;
	//			c.g = 0;
				c.a = 1;
				
//				Debug.Log(c.r + "x" + c.g + "x" + c.b);
////				Debug.Log(c.r + "x" + c.g + "x" + c.b);
////				Debug.Log("return: " + PartsToFloat(GetColorPart1(pos.z), GetColorPart2(pos.z)));
//				Debug.Log("1: " +  GetColorPart1(Mathf.Abs(pos.x)));
//				Debug.Log("2: " +  GetColorPart2(Mathf.Abs(pos.x)));
//				Debug.Log("return: " + PartsToFloat(GetColorPart1(Mathf.Abs(pos.x)), GetColorPart2(Mathf.Abs(pos.x))));
//				
//				float cx1 = GetColorPart1(Mathf.Abs(pos.x));
//				float cx2 = GetColorPart2(Mathf.Abs(pos.x));
//				
//				Debug.Log("org: " + (PartsToFloat(cx1, cx2)));
//
				dot.GetComponent<MeshRenderer>().material.color = c;
			}
		}
	}

	float Map(float num, float oldMin, float oldMax, float newMin, float newMax){
		float oldRange = oldMax - oldMin;
		float newRange = newMax - newMin;

		float oldNum = num - oldMin;

		float percent = oldNum/oldRange;

		return (percent * newRange) + newMin;
	}

	float GetColorPart1(float f, float min, float max){
		float val = Map(f, min, max, 0, 255); 

		float d = (Mathf.Floor(val)/255f);

		return d;
	}

	float GetColorPart2(float f, float min, float max){
		float part1 = GetColorPart1(f, min, max);

		float val = Map(f, min, max, 0, 255)/255f; 

		val = val - part1;

		return val * 255;
	}

	float PartsToFloat(float i1, float i2, float min, float max){
		float part1 = Map(i1, 0, 1, min, max);
		float part2 = Map(i2/255f, 0, 1, min, max);

		return part1 + part2;
	} 


	bool InsideCube(Vector3 pos){
		if(pos.y < front[2].y &&
		   pos.x > front[0].x&&
		   pos.x < front[1].x){
			return true;
		}

		return false;
	}

	bool once = false;

	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Space)){
//		if(Input.mousePosition.x > 300 && !once){
			once = true;
			
			Debug.Log ("tolerance: " + tolerance);
			Debug.Log("End count:" + lines.Count);

			part1 = !part1;
			SetColorToPart();

			int part = 2;
			if(part1)
				part = 1;

			StartCoroutine(UploadPNG(fileNamePre + "Lumarca" + 
			                         Camera.main.pixelWidth + "x" + 
			                         Camera.main.pixelHeight + 
			                         "_DotSize" + yarnWidth + 
			                         "Depth" + numDepths +
			                         "Part" + part));

			BinaryWriter writer;
			StringReader reader;

			switch(lumarcaMode){
			case LUMARCA_MODE.VLines:

				JObject jObject = new JObject();
				jObject[PROP_LUMARCA_SIZE] = cfs.physicalWidth;
				jObject[PROP_PROJ_POS] = UtilScript.Vector3ToJson(cfs.transform.position);
				jObject[PROP_THROW_RATIO] = cfs.throwRatio;
				jObject[PROP_PHYSICAL_WIDTH] = cfs.physicalWidth;
				jObject[PROP_CEILING_MOUNT] = cfs.ceilingMounted;
				JArray jPositions = new JArray();
				jObject[PROP_POSITIONS] = jPositions;


				for(int i = 0; i < lines.Count; i++){
					jPositions.Add(UtilScript.Vector3ToJson(lines[i]));
				}

				string positionFile = System.IO.Path.Combine(Application.dataPath, fileNamePre + "-positions.json");

				writer = new BinaryWriter(File.Open(positionFile, FileMode.Create));

				string objStr = JsonConvert.SerializeObject(jObject, Formatting.Indented);
				writer.Write(objStr);
				writer.Close();

				string exportPath = Application.dataPath + "/" + fileNamePre + "-" + cfs.physicalWidth + "-export.json";

				UtilScript.SaveStringToFile(exportPath, objStr);

				JObject o1 = JObject.Parse(File.ReadAllText(exportPath));

				Debug.Log("Save File: " + positionFile);
				Debug.Log("exportPath File: " + exportPath);
				Debug.Log ("Vec3:  " + o1["ThrowRatio"]);
				break;
			case LUMARCA_MODE.Dots:
//				for(int i = 0; i < dots.Count; i++){
//					jPositions[i] = UtilScript.Vector3ToJson(dots[i]);
//				}
//				
//				string dotFile = System.IO.Path.Combine(Application.dataPath, "DotPositions.json");
//
//				writer = new BinaryWriter(File.Open(dotFile, FileMode.Create));
//				jPositions.Serialize(writer);
//				writer.Close();
//				
//				reader = new BinaryReader(File.Open(dotFile, FileMode.Open));
//				ja = SimpleJSON.JSONNode.Deserialize(reader);
//
//				reader.Close();
//
//				
//				for(int i = 0; i < ja.Count; i++){
//					Debug.Log ("jn:  " + i + " : " + JSON.Parse(ja[i])["x"]);
//				}
				break;
			}
		}
		
//		DrawBox();
	}

	
	void OnPostRender() {
		if(drawFrame)
			DrawBox();
	}

	void OnDrawGizmos(){
		if(drawFrame)
			DrawBox();
	}

	void DrawBox(){
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

		if(front.Length > 0){

			for(int i = 0; i < front.Length; i++){
				Vector3 v = front[i];

				print("front[" + i + "]: " + v.x + ", " + v.y + ", " + v.z);
	
			}

			for(int i = 0; i < back.Length; i++){
				Vector3 v = back[i];

				print("back[" + i + "]: " + v.x + ", " + v.y + ", " + v.z);
			}


			GL.PushMatrix();
			mat.SetPass(0);
			GL.Begin(GL.LINES);
			GL.Color(Color.red);
			GL.Vertex(front[0]);
			GL.Vertex(front[1]);
			GL.Vertex(front[1]);
			GL.Vertex(front[2]);
			GL.Vertex(front[2]);
			GL.Vertex(front[3]);
			GL.Vertex(front[3]);
			GL.Vertex(front[0]);

			GL.Vertex(back[0]);
			GL.Vertex(back[1]);
			GL.Vertex(back[1]);
			GL.Vertex(back[2]);
			GL.Vertex(back[2]);
			GL.Vertex(back[3]);
			GL.Vertex(back[3]);
			GL.Vertex(back[0]);

			
			GL.Vertex(front[0]);
			GL.Vertex(back[0]);

			GL.Vertex(front[1]);
			GL.Vertex(back[1]);

			GL.Vertex(front[2]);
			GL.Vertex(back[2]);
			
			GL.Vertex(front[3]);
			GL.Vertex(back[3]);

	//		Test for Speed hit with lots of lines
	//		for(int x = 0; x < 100; x++){
	//			for(int y = 0; y < 1000; y++){
	//				GL.Vertex(new Vector2(x * 2, y));
	//				GL.Vertex(new Vector2(x * 2+ 2, y));
	//			}
	//		}

			GL.End();
			GL.PopMatrix();
		}
	}

	public IEnumerator UploadPNG(string imageName) {
		// We should only read the screen buffer after rendering is complete
		yield return new WaitForEndOfFrame();
		
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy(tex);
		
		// For testing purposes, also write to a file in the project folder
		File.WriteAllBytes(Application.dataPath + "/../" + imageName + ".png", bytes);


		
		
		// Create a Web Form
		//		WWWForm form = new WWWForm();
		//		form.AddField("frameCount", Time.frameCount.ToString());
		//		form.AddBinaryData("fileUpload", bytes);
		//		
		//		// Upload to a cgi script
		//		WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
		//		yield return w;
		//		if (w.error != null)
		//			print(w.error);
		//		else
		//			print("Finished Uploading Screenshot");
	}

}
