using UnityEngine;
using System.Collections;

public class BaldEagleLiteUserControllerScript : MonoBehaviour {
	public BaldEagleLiteCharacterScript baldEagleLiteCharacter;
	public float upDownInputSpeed=3f;

	void Start () {
		baldEagleLiteCharacter = GetComponent<BaldEagleLiteCharacterScript> ();	
	}

	void Update(){
		if (Input.GetButtonDown ("Jump")) {
			baldEagleLiteCharacter.Soar ();
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			baldEagleLiteCharacter.Hit ();
		}
		if (Input.GetButtonDown ("Fire1")) {
			baldEagleLiteCharacter.Attack ();
		}
		if (Input.GetKey (KeyCode.N)) {
			baldEagleLiteCharacter.upDown=Mathf.Clamp(baldEagleLiteCharacter.upDown-Time.deltaTime*upDownInputSpeed,-1f,1f);
		}
		if (Input.GetKey (KeyCode.U)) {
			baldEagleLiteCharacter.upDown=Mathf.Clamp(baldEagleLiteCharacter.upDown+Time.deltaTime*upDownInputSpeed,-1f,1f);
		}
	}
	
	void FixedUpdate(){
		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");	

		baldEagleLiteCharacter.forwardAcceleration = v;
		baldEagleLiteCharacter.yawVelocity = h;
	}
}
