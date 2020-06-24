using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongFormTranistions : MonoBehaviour
{

	enum State
	{
		none,
		speedUp,
		slowDown,
		spin,
		shrunk,
		grow,
	};

	State currentState = State.none;

	float maxAnimationSpeed = 2f;
	float minAnimationSpeed = .75f;
	float animAcel = 0.15f;

	int spinCount = 0;
	int resetCount = 3;

	public Animator anim;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Reset(){
		spinCount = 0;
		anim.SetBool("MoveToSpin", false);
		anim.SetBool("MoveToShrink", false);
		anim.SetBool("Reset", true);
		resetCount = 3;
		currentState = State.none;
	}

	public void UpdateAnim ()
	{
		print("UpdateAnim: " + anim.speed + ": " + currentState + " " + gameObject.name);

		switch (currentState) {
		case State.none:
			currentState = State.speedUp;
			break;
		case State.speedUp:
			if (anim.speed < maxAnimationSpeed) {
				anim.speed += animAcel;
			} else {
				currentState = State.slowDown;
			}
			break;
		case State.slowDown:
			anim.SetBool("Reset", false);

			if (anim.speed > minAnimationSpeed)
				anim.speed -= animAcel * 2.5f;
			else
				currentState = State.spin;
			break;
		case State.spin:
			anim.SetBool("MoveToSpin", true);

			spinCount++;
			if(spinCount == 5){
				anim.SetBool("MoveToShrink", true);
				currentState = State.shrunk;
			}
			break;
		case State.shrunk:
			currentState = State.grow;
			break;
		case State.grow:
			resetCount--;

			if(resetCount == 0){
				Reset();
			}

			break;
		default:
			break;
		}
			
	}
}
