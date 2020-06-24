using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CalcLinePositions : LumarcaLineRenderer {

	List<Vector3> topList = new List<Vector3>();
	List<Vector3> botList = new List<Vector3>();

	// Use this for initialization
	void Start () {
		SetMaterial();
		mat.color = new Color(1, 1, 1);

		frontBottomY = -1000;
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space)){
			printToXML();
		}
	}

	float tolerance = 0.001f;
	float frontBottomY = -1000;
	float frontBottomZ = 1000;
	float maxDist = 0;

	public bool closeEnough(float y1, float y2){

		return Mathf.Abs(y1 - y2) < tolerance;
	}

	public override Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
		float topX, float bottomX,
		float topY, float bottomY,
		float topZ, float bottomZ){

		if(frontBottomY < bottomY){
			frontBottomY = bottomY;
		}
		if(frontBottomZ > bottomZ){
			frontBottomZ = bottomZ;// + 0.01f;
		}

		Vector3[] result = new Vector3[2];

		Vector3 vec1 = UtilScript.CloneVec3(linePos);
		Vector3 vec2 = UtilScript.CloneVec3(linePos);

		vec1.y = bottomY;
		vec2.y = bottomY;

		Vector3 pivot = new Vector3(vec1.x, frontBottomY, bottomZ);

		float dist = Vector3.Distance(vec1, pivot);

		if(maxDist < dist){
			maxDist = dist;
		}

		float angle = Mathf.PI * 2;

		vec1.y = frontBottomY;
		vec1.z = dist + frontBottomZ;

		if(lineNum == 500){
//			500: 0.4266512 ,-0.3809375, -0.1321059
//			500: 0.4266512 ,-0.3809375, -0.1321059
			print("500: " + vec1.x + " ,"  +  vec1.y  +  ", " + vec1.z);
		}

		if(Time.frameCount == 10){
			print("HERE");
			botList.Add(vec1);
			topList.Add(new Vector3(linePos.x, topY, linePos.z));
		}

		vec2 = new Vector3(vec1.x + 0.01f, vec1.y, vec1.z);

		result[0] = vec2;
		result[1] = vec1;

		return result;
	}

	public void printToXML(){
		LoadLumarca ll = Camera.main.GetComponent<LoadLumarca>();

		JObject result = new JObject();

		Vector3[] front = ll.front;
		Vector3[] back = ll.back;

		JObject top = new JObject();
		JObject bottom = new JObject();


		result["top"] = top;
		result["bottom"] = bottom;

//		ArgumentException: Can not add Newtonsoft.Json.Linq.JObject to Newtonsoft.Json.Linq.JObject.
//		Newtonsoft.Json.Linq.JObject.ValidateToken (Newtonsoft.Json.Linq.JToken o, Newtonsoft.Json.Linq.JToken existing) (at Assets/Scripts/JsonDotNet/Source/Linq/JObject.cs:185)
//		Newtonsoft.Json.Linq.JContainer.InsertItem (Int32 index, Newtonsoft.Json.Linq.JToken item) (at Assets/Scripts/JsonDotNet/Source/Linq/JContainer.cs:296)
//		Newtonsoft.Json.Linq.JObject.InsertItem (Int32 index, Newtonsoft.Json.Linq.JToken item) (at Assets/Scripts/JsonDotNet/Source/Linq/JObject.cs:177)
//		Newtonsoft.Json.Linq.JContainer.AddInternal (Int32 index, System.Object content) (at Assets/Scripts/JsonDotNet/Source/Linq/JContainer.cs:521)
//		Newtonsoft.Json.Linq.JContainer.Add (System.Object content) (at Assets/Scripts/JsonDotNet/Source/Linq/JContainer.cs:492)
//		CalcLinePositions.printToXML () (at Assets/Princeton/scripts/CalcLinePositions.cs:101)
//		CalcLinePositions.Update () (at Assets/Princeton/scripts/CalcLinePositions.cs:24)


		JArray topCorners = new JArray();
		top["topCorners"] = topCorners;

		topCorners.Add(UtilScript.Vector3ToJson(front[3]));
		topCorners.Add(UtilScript.Vector3ToJson(front[2]));
		topCorners.Add(UtilScript.Vector3ToJson(back[2]));
		topCorners.Add(UtilScript.Vector3ToJson(back[3]));

		JArray botCorners = new JArray();
		bottom["botCorners"] = botCorners;

		botCorners.Add(UtilScript.Vector3ToJson(front[0]));
		botCorners.Add(UtilScript.Vector3ToJson(front[1]));

		//Back Corners
		Vector3 pivot = new Vector3(back[1].x, frontBottomY, front[1].z);
		float dist = Vector3.Distance(pivot, back[1]);

		Vector3 backCorner1 = new Vector3(back[1].x, frontBottomY, dist + frontBottomZ);
		Vector3 backCorner2 = new Vector3(back[0].x, frontBottomY, dist + frontBottomZ);

		botCorners.Add(UtilScript.Vector3ToJson(backCorner1));
		botCorners.Add(UtilScript.Vector3ToJson(backCorner2));
//
//		for(int i = 0; i < back.Length; i++){
//			print("back[" + i + "]: " + UtilScript.ActualVector3(back[i]));
//			botCorners.Add(UtilScript.Vector3ToJson(back[i]));
//		}

		print("Count:" + topList.Count);

		JArray topLines = new JArray();
		top["topLines"] = topLines;
		JArray botLines = new JArray();
		bottom["botLines"] = botLines;

		for(int i = 0; i < topList.Count; i++){
			topLines.Add(UtilScript.Vector3ToJson(topList[i]));
			botLines.Add(UtilScript.Vector3ToJson(botList[i]));
//			print("->500: " + UtilScript.ActualVector3(top[i]) + "->" + UtilScript.ActualVector3(bottom[i]));
		}

		UtilScript.WritePrettyJSONToFile("LumarcaFrustum-LinePositions.json", result);
	}

}
