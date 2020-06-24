using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LumarcaFrame {

	public List<LumarcaLine> lines = new List<LumarcaLine>();

	public void AddLine(LumarcaLine ll){
		lines.Add(ll);
	}

	public static LumarcaFrame LoadFromJSON(JArray jLines){

		LumarcaFrame lf = new LumarcaFrame();

		foreach(JObject jLine in jLines){
			LumarcaLine ll = LumarcaLine.LoadFromJSON(jLine);
			lf.lines.Add(ll);
		}

		return lf;
	}

	public JArray GetJSONLine(){
		JArray jArray = new JArray();

		foreach(LumarcaLine line in lines){
			JObject jsonLine = line.GetJSONLine();

			jArray.Add(jsonLine);
		}

		return jArray;
	}
}
