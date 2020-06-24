using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModAnimSpeed : MonoBehaviour {

	public Animation myAnim;
	public float speed;
	public string animationName = "Allosaurus_Walk";

	// Use this for initialization
	void Start () {
//
//		Animator animator = GetComponent<Animator>();
//
//		AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
//
//		int flapId = Animator.StringToHash("Flap1");
//
//		print(animStateInfo.IsName("Flap1"));
//
//		myAnim = GetComponentsInChildren<Animation>()[0];
//
//		print(myAnim);

		Time.timeScale = speed;
//
//		AnimationState aState = myAnim[animationName];
//
//		aState.speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
