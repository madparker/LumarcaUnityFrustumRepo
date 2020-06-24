using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LumarcaLine {

	public const string JSON_TOP = "top";
	public const string JSON_BOTTOM = "bottom";
	public const string JSON_MATERIAL = "mat";
	public const string JSON_HAS_DOT = "dot";

	public Vector3 top;
	public Vector3 bottom;

	public string material;

	public bool hasDots;

	public LumarcaLine(Vector3 top, Vector3 bottom, string material, bool hasDots){
		this.top = top;
		this.bottom = bottom;

		if(top.y < bottom.y){
			Debug.Log("ERROR");
		}

		this.material = material;
		this.hasDots = hasDots;
	}

	public LumarcaLine(Vector3 top, Vector3 bottom, string material){
		this.top = top;
		this.bottom = bottom;

		if(top.y < bottom.y){
			Debug.Log("ERROR");
		}

		this.material = material;
		this.hasDots = true;
	}

	public static LumarcaLine LoadFromJSON(JObject jLine){

		LumarcaLine ll = new LumarcaLine(
			UtilScript.JsonToVector3(jLine[JSON_TOP]),
			UtilScript.JsonToVector3(jLine[JSON_BOTTOM]),
			jLine[JSON_MATERIAL].ToString(),
			(bool)jLine[JSON_HAS_DOT]);

		return ll;
	}

	public JObject GetJSONLine(){
		JObject jsonLine = new JObject();
		jsonLine[JSON_TOP] = UtilScript.Vector3ToJson(top);
		jsonLine[JSON_BOTTOM] = UtilScript.Vector3ToJson(bottom);
		jsonLine[JSON_MATERIAL] = material;
		jsonLine[JSON_HAS_DOT] = hasDots;

		return jsonLine;
	}
}
