using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumarcaShaderWrapper : MonoBehaviour {

	public ComputeShader shader;
	public int kernelHandle;
	public RenderTexture tex;
	public int numLines;

	// Use this for initialization
	void Start () {
		kernelHandle = shader.FindKernel("CSMain");
	}
	
	// Update is called once per frame
	void Update () {
		RunShader();
	}

	void RunShader()
	{
		tex = new RenderTexture(256,256,24);
		tex.enableRandomWrite = true;
		tex.Create();

		shader.SetTexture(kernelHandle, "Result", tex);
		shader.Dispatch(kernelHandle, numLines, 1, 1);
	}
}
