using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScript : MonoBehaviour {

	private Vector3 headPosition;
	public Vector3 HeadPosition{
		set{
			headPosition = value;
		}
	}

	private Vector3 topTorsoPosition;
	public Vector3 TopTorsoPosition{		
		set{
			topTorsoPosition = value;
		}
	}

	private Vector3 bottomTorsoPosition;
	public Vector3 BottomTorsoPosition{		
		set{
			bottomTorsoPosition = value;
		}
	}
		
	private Vector3 leftHandPosition;
	public Vector3 LeftHandPosition{
		set{
			leftHandPosition = value;
		}
	}

	private Vector3 rightHandPosition;
	public Vector3 RightHandPosition{
		set{
			rightHandPosition = value;
		}
	}


	private Vector3 leftFootPosition;
	public Vector3 LeftFootPosition{
		set{
			leftFootPosition = value;
		}
	}

	private Vector3 rightFootPosition;
	public Vector3 RightFootPosition{
		set{
			rightFootPosition = value;
		}
	}

	private Vector3 leftElbowPosition;
	public Vector3 LeftElbowPosition{
		set{
			leftElbowPosition = value;
		}
	}

	private Vector3 rightElbowPosition;
	public Vector3 RightElbowPosition{
		set{
			rightElbowPosition = value;
		}
	}

	private Vector3 leftKneePosition;
	public Vector3 LeftKneePosition{
		set{
			leftKneePosition = value;
		}
	}

	private Vector3 rightKneePosition;
	public Vector3 RightKneePosition{
		set{
			rightKneePosition = value;
		}
	}


	public GameObject head;
	public GameObject topTorso;
	public GameObject bottomTorso;
	public GameObject rightHand;
	public GameObject leftHand;
	public GameObject rightFoot;
	public GameObject leftFoot;
	public GameObject rightKnee;
	public GameObject leftKnee;
	public GameObject rightElbow;
	public GameObject leftElbow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		head.transform.localPosition = headPosition;
		topTorso.transform.localPosition = topTorsoPosition;
		bottomTorso.transform.localPosition = bottomTorsoPosition;
		rightHand.transform.localPosition = rightHandPosition;
		leftHand.transform.localPosition = leftHandPosition;
		rightFoot.transform.localPosition = rightFootPosition;
		leftFoot.transform.localPosition = leftFootPosition;
		rightKnee.transform.localPosition = rightKneePosition;
		leftKnee.transform.localPosition = leftKneePosition;
		rightElbow.transform.localPosition = rightElbowPosition;
		leftElbow.transform.localPosition = leftElbowPosition;
	}

	public bool Invalid(){

		float distTorso = Vector3.Distance(topTorsoPosition, 	Vector3.zero);
		float distHead 	= Vector3.Distance(headPosition, 		Vector3.zero);
		float distRHand = Vector3.Distance(rightHandPosition, 	Vector3.zero);
		float distLHand = Vector3.Distance(leftHandPosition, 	Vector3.zero);
		float distRFoot = Vector3.Distance(rightFootPosition, 	Vector3.zero);
		float distLFoot = Vector3.Distance(leftFootPosition, 	Vector3.zero);

		Debug.Log("dist: " + distTorso);

		return 
			(distTorso == 0) ||
			(distTorso  > 1) ||
			(distHead + distRHand + distLHand + distRFoot + distLFoot == 0);
	}
}
