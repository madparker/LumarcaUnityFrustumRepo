using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereNoiseMovement : MonoBehaviour {

	public float frontX = 0.5f;
	public float backX = 1.5f;
	public float frontRanger = 0.5f;
	public float backRanger = 0.5f;
	public float speedMod = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float moveTime = Time.realtimeSinceStartup * speedMod;

		float perlin = Mathf.PerlinNoise(0, moveTime);

		float zPos = UtilScript.Map(perlin, 0, 1, -frontRanger, frontRanger);

		float lerpPer = (zPos + frontRanger)/(frontRanger * 2f);

		float xPos = Mathf.Lerp(frontX, backX, lerpPer);

		float yPos = perlin - 0.75f;

		transform.position = new Vector3(
			UtilScript.Map(Mathf.PerlinNoise(moveTime, 0), 0, 1, -xPos, xPos),
			yPos,
			zPos);
	}
}
