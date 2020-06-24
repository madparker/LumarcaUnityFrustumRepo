using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUCalibrationScript : LumarcaLineRenderer {

	static int indexLeft;
	static int indexRight;
	static int indexCallOut;

	int lineLeftNum;
	int lineRightNum;

	List<int> lefts = new List<int>();
	List<int> rights = new List<int>();

	Dictionary<int, Vector3> allPos = new Dictionary<int, Vector3>();

	public TextMesh lNum;
	public TextMesh rNum;
	public TextMesh killDisplay;

	public Material[] mats;

	public bool calMode = false;

	public string callString = "";

	bool upCall = false;


	public override Vector3[] GenerateLine(int lineNum, Vector3 linePos, 
		float topX, float bottomX,
		float topY, float bottomY,
		float topZ, float bottomZ){

		List<Vector3> result = new List<Vector3>();

		if(!lefts.Contains(lineNum) && !rights.Contains(lineNum)){
			if(linePos.x <= 0){
				AddedSortToList(lefts, lineNum, linePos);
			} else {
				AddedSortToList(rights, lineNum, linePos);
			}
			allPos.Add(lineNum, linePos);
		} else {
			if(calMode){
				
				if(lefts.Contains(indexCallOut) && upCall){
					indexLeft = lefts.IndexOf(indexCallOut);
					upCall = false;
				} else if(rights.Contains(indexCallOut) && upCall){
					indexRight = rights.IndexOf(indexCallOut);
					upCall = false;
				} 

				mat = mats[4];

				if(lineNum == lineLeftNum){

					lNum.text = "line: " + lineNum;

					Vector3 vec1 = UtilScript.CloneVec3(linePos);
					Vector3 vec2 = UtilScript.CloneVec3(linePos);

					vec1.y = topY;
					vec2.y = bottomY;

					result.Add(vec1);
					result.Add(vec2);
				} else if(lineNum == lineRightNum){

					rNum.text = "line: " + lineNum;

					Vector3 vec1 = UtilScript.CloneVec3(linePos);
					Vector3 vec2 = UtilScript.CloneVec3(linePos);

					vec1.y = topY;
					vec2.y = bottomY;

					result.Add(vec1);
					result.Add(vec2);
				}
			} else {

				switch(lineNum){
//				case 616:
//					result = drawLine(linePos, mats[0], topY, bottomY);
//					break;
				case 25:
					result = drawLine(linePos, mats[1], topY, bottomY);
					break;
				case 75:
					result = drawLine(linePos, mats[2], topY, bottomY);
					break;
				case 150:
					print(linePos); 
					result = drawLine(linePos, mats[3], topY, bottomY);
					break;
				default:
					break;
				}
			}
				

//			if(lineNum == 616 || lineNum == 567 || lineNum == 62 || lineNum == 0){
//
//				rNum.text = "line: " + lineRightNum;
//
//				Vector3 vec1 = UtilScript.CloneVec3(linePos);
//				Vector3 vec2 = UtilScript.CloneVec3(linePos);
//
//				vec1.y = topY;
//				vec2.y = bottomY;
//
//				result.Add(vec1);
//				result.Add(vec2);
//			}
		}

		return result.ToArray();
	}

	List<Vector3> drawLine(Vector3 linePos, Material pasMat, float topY, float bottomY){

		List<Vector3> vecs= new List<Vector3>();

		mat = pasMat;

		Vector3 vec1 = UtilScript.CloneVec3(linePos);
		Vector3 vec2 = UtilScript.CloneVec3(linePos);

		vec1.y = topY;
		vec2.y = bottomY;

		vecs.Add(vec1);
		vecs.Add(vec2);

		return vecs;
	}

	void AddedSortToList(List<int> list, int num, Vector3 currentPos){
		if(list.Count == 0){
			list.Add(num);
		} else {
			bool wasInserted = false;

			for(int i = 0; i < list.Count; i++){
				Vector3 listPos = allPos[list[i]];

				if(listPos.z > currentPos.z){
					wasInserted = true;
					list.Insert(i, num);
					break;
				}
			}

			if(!wasInserted){
				list.Add(num);
			}
		}
	}

	void Update () {

//		print("lefts" + lefts.Count);
//		print("rights" + rights.Count);

		if(Input.GetKeyDown(KeyCode.K)){
			print("K: " + indexCallOut);

			if(LoadLumarca.killStringsList.Contains(indexCallOut)){
				print("has");
				LoadLumarca.killStringsList.Remove(indexCallOut);
			} else {
				print("hasnot");
				LoadLumarca.killStringsList.Add(indexCallOut);
			}

			string result = "";
			string resultDiplay = "";

			int count = 0;

			for(int i = 0; i < LoadLumarca.killStringsList.Count; i++){
				int num = LoadLumarca.killStringsList[i];

				while(LoadLumarca.killStringsList.Contains(num)){
					LoadLumarca.killStringsList.Remove(num);
				}

				LoadLumarca.killStringsList.Add(num);
			}

			for(int i = 0; i < LoadLumarca.killStringsList.Count; i++){
				result +=  LoadLumarca.killStringsList[i] + ",";
				resultDiplay +=  LoadLumarca.killStringsList[i] + ",";

				count++;

				if(count == 30){
					count = 0;

					resultDiplay += "\n";
				}
			}

			//550, 502, 454

			PlayerPrefs.SetString(LoadLumarca.PP_KILL_STRING, result);

			UtilScript.SaveStringToFile("KillStrings.txt", result);

			killDisplay.text = "Kill String: " + resultDiplay;

			print("Kill String: " + result);
		}

		if(Input.GetKeyDown(KeyCode.Return)){

			print(callString);

			if(!callString.Equals("")){

				int i = System.Int32.Parse(callString);

				if(i >= 0 || i < 640){
					indexCallOut = i;

					upCall = true;
				}

				callString = "";
			}
		} else {
			InputKey(KeyCode.Keypad0, 256);
			InputKey(KeyCode.Keypad1, 256);
			InputKey(KeyCode.Keypad2, 256);
			InputKey(KeyCode.Keypad3, 256);
			InputKey(KeyCode.Keypad4, 256);
			InputKey(KeyCode.Keypad5, 256);
			InputKey(KeyCode.Keypad6, 256);
			InputKey(KeyCode.Keypad7, 256);
			InputKey(KeyCode.Keypad8, 256);
			InputKey(KeyCode.Keypad9, 256);

			InputKey(KeyCode.Alpha0, 48);
			InputKey(KeyCode.Alpha1, 48);
			InputKey(KeyCode.Alpha2, 48);
			InputKey(KeyCode.Alpha3, 48);
			InputKey(KeyCode.Alpha4, 48);
			InputKey(KeyCode.Alpha5, 48);
			InputKey(KeyCode.Alpha6, 48);
			InputKey(KeyCode.Alpha7, 48);
			InputKey(KeyCode.Alpha8, 48);
			InputKey(KeyCode.Alpha9, 48);
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			calMode = !calMode;
		}

		if(lefts.Count > 0 && rights.Count > 0){
			if(Input.GetKeyDown(KeyCode.Q)){
				indexLeft++;
			} else if(Input.GetKeyDown(KeyCode.A)){
				indexLeft--;
			}

			if(indexLeft < 0){
				indexLeft = lefts.Count - 1;
			}


			if(Input.GetKeyDown(KeyCode.O)){
				indexRight++;
			} else if(Input.GetKeyDown(KeyCode.L)){
				indexRight--;
			}

			if(indexRight < 0){
				indexRight = rights.Count - 1;
//				indexRight = lefts.Count - 1;
			}

			lineLeftNum = lefts[indexLeft%lefts.Count];
			lineRightNum = rights[indexRight%rights.Count];
//			lineRightNum = lefts[indexRight%lefts.Count];
		}
	}

	void InputKey(KeyCode k, int mod){

		if(Input.GetKeyDown(k)){
			callString +=  "" + (k.GetHashCode() - mod);
		}
	}
}
