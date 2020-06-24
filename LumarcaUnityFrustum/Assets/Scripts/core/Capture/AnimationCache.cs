using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCache : MonoBehaviour {

	const string PATH_TO_ANIMATIONS = "Animations/";

	static Dictionary<string, List<LumarcaFrame>> animCache = new Dictionary<string, List<LumarcaFrame>>();


	public static List<LumarcaFrame> GetFrames(string animKeyStr){
//
//		if(!animCache.ContainsKey(animKeyStr)){
//			Debug.Log(PATH_TO_MATERIALS + animKeyStr);
//
//			LumarcaAnimation la = Resources.Load(PATH_TO_ANIMATIONS + animKeyStr, typeof(Material)) as Material;
//
//			animCache[animKeyStr] = la;
//		}

		if(animCache.ContainsKey(animKeyStr)){
			return animCache[animKeyStr];
		}

		return null;
	}
		
	public static void SetFrames(string animKeyStr, List<LumarcaFrame> frames){
		animCache[animKeyStr] = frames;
	}

}
