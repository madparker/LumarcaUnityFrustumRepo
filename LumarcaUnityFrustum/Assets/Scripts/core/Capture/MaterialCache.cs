using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCache : MonoBehaviour {

	const string PATH_TO_MATERIALS = "Materials/";

	static Dictionary<string, Material> matCache = new Dictionary<string, Material>();


	public static Material GetMaterial(string matStr){

		if(!matCache.ContainsKey(matStr)){
			Debug.Log(PATH_TO_MATERIALS + matStr);

			Material mat = Resources.Load(PATH_TO_MATERIALS + matStr, typeof(Material)) as Material;

			matCache[matStr] = mat;
		}

		if(matCache[matStr] == null){
			Debug.Log("MaterialCache- Could not find material: " + matStr);
		}
			
		return matCache[matStr];
	}

}
