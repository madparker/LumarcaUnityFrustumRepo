using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureScript : MonoBehaviour {

	private const string NAME_END1 = "End1";
	private const string NAME_END2 = "End2";
	private const string NAME_LINE = "Line";
	private const string NAME_TEXT = "Text";

	private GameObject end1;
	private GameObject end2;
	private GameObject line;
	private TextMesh textMesh;

	public Vector3 point1 = new Vector3(1, -1, 0);
	public Vector3 Point1{
		set{
			point1 = value;
		}

		get{
			return point1;
		}
	}

	public Vector3 point2 = new Vector3(1, 1, 0);
	public Vector3 Point2{
		set{
			point1 = value;
		}

		get{
			return point1;
		}
	}

	// Use this for initialization
	void Start () {
		for(int i = 0; i < transform.childCount; i++){
			GameObject g = transform.GetChild(i).gameObject;

			switch(g.name){
			case NAME_END1:
				end1 = g;
				break;
			case NAME_END2:
				end2 = g;
				break;
			case NAME_LINE:
				line = g;
				break;
			case NAME_TEXT:
				textMesh = g.GetComponent<TextMesh>();
				break;
			}
		}
	
		SetSize(point1, point2);
	}
	
	// Update is called once per frame
	void Update () {
//		print("Z: " + (Camera.main.nearClipPlane + Camera.main.transform.position.z));

		textMesh.gameObject.transform.LookAt(textMesh.transform.position - Camera.main.transform.position);
	}

	public void SetSize(Vector3 pos1, Vector3 pos2){

//		Quaternion.FromToRotation(Vector3.up, transform.forward);
//
		Vector3 relativePos = pos1 - pos2;

		relativePos = Quaternion.AngleAxis(90, Vector3.back) * relativePos;
	
//		Quaternion rotation = Quaternion.LookRotation(relativePos);

		print(relativePos.normalized);

		Quaternion q = Quaternion.LookRotation(relativePos.normalized);

//		Debug.Log(relativePos.normalized);

		transform.rotation = q;

		Vector3 center = (pos1 + pos2);

		float size = Vector3.Distance(pos1, pos2);

		center.Scale(new Vector3(0.5f, 0.5f, 0.5f));

		transform.position = center;

		line.transform.localScale = new Vector3(
			size,
			line.transform.localScale.y,
			line.transform.localScale.z);

		textMesh.text = size + "m";

		end1.transform.localPosition = new Vector3(
			size/-2f,
			0,
			0);
		end2.transform.localPosition = new Vector3(
			size/2f,
			0,
			0);
	}
}
