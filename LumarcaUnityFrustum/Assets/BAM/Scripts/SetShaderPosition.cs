using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderPosition : MonoBehaviour {

	public LumarcaMeshRender lm;
	public float modPos = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Postprocess the image
	void Update()
	{
		lm.material.SetVector("_GOPosition", transform.position * modPos);
	}
}
