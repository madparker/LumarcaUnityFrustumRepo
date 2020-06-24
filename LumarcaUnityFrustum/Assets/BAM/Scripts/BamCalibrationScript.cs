using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BamCalibrationScript : LumarcaLineRenderer {

	static int indexLeft;
	static int indexRight;

	int lineLeftNum;
	int lineRightNum;

	List<int> lefts = new List<int>();
	List<int> rights = new List<int>();

	Dictionary<int, Vector3> allPos = new Dictionary<int, Vector3>();

	public TextMesh lNum;
	public TextMesh rNum;

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
//			if(lineNum == lineLeftNum){
//
//				lNum.text = "line: " + lineLeftNum;
//
//				Vector3 vec1 = UtilScript.CloneVec3(linePos);
//				Vector3 vec2 = UtilScript.CloneVec3(linePos);
//
//				vec1.y = topY;
//				vec2.y = bottomY;
//
//				result.Add(vec1);
//				result.Add(vec2);
//			} else if(lineNum == lineRightNum){
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

			if(lineNum == 616 || lineNum == 567 || lineNum == 62 || lineNum == 0){

				rNum.text = "line: " + lineRightNum;

				Vector3 vec1 = UtilScript.CloneVec3(linePos);
				Vector3 vec2 = UtilScript.CloneVec3(linePos);

				vec1.y = topY;
				vec2.y = bottomY;

				result.Add(vec1);
				result.Add(vec2);
			}
		}

		return result.ToArray();
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
		print("lefts" + lefts.Count);
		print("rights" + rights.Count);

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
}
