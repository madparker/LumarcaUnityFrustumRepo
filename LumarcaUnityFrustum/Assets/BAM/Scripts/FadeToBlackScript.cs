using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FadeToBlackScript : MonoBehaviour {

	public float fadeTime = 1;
	public bool fadeIn = true;
	public Shader shader;

	public bool FadeIn{
		get{
			return fadeIn;
		}

		set{
			if(value != fadeIn){
				fadeIn = value;
				Setup();
			}
		}
	}

	private Material material;

	private float start;
	private float end;

	private float timer;

	// Creates a private material used to the effect
	void Awake ()
	{
		if(material == null){
			if(shader == null){
				shader = Shader.Find("Hidden/FadeInOut");
			}

			material = new Material(shader);
		}
	}

	void Start(){
		Setup();
	}

	void Setup(){
		if(fadeIn){
			start = 1;
			end = 0;
		} else {
			start = 0;
			end = 1;
		}

		timer = 0;
	}

	//Update function for testing
//	void Update(){
//		if(Input.GetKeyDown(KeyCode.Space)){
//			FadeIn = !FadeIn;
//		}
//	}

	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if(material == null){
			material = new Material(shader);
		}

		timer += Time.deltaTime/fadeTime;

		timer = Mathf.Clamp(timer, 0, 1);

		float intensity = Mathf.Lerp(start, end, timer);

		if (intensity == 0)
		{
			Graphics.Blit (source, destination);
			return;
		}

		material.SetFloat("_FadeAmt", intensity);
		Graphics.Blit (source, destination, material);
	}
}