using UnityEngine;
using System.Collections;

public class BaldEagleLiteCharacterScript : MonoBehaviour {
	public Animator baldEagleAnimator;
	Rigidbody baldEagleRigid;
	public bool isFlying=false;
	public float upDown=0f;
	public float forwardAcceleration=0f;
	public float yawVelocity=0f;
	public float groundCheckDistance=5f;
	public bool soaring=false;
	public bool isGrounded=true;
	public float forwardSpeed=0f;
	public float maxForwardSpeed=3f;
	public float meanForwardSpeed=1.5f;
	public float speedDumpingTime=.1f;
	public float groundedCheckOffset=1f;

	void Start(){
		baldEagleAnimator = GetComponent<Animator> ();
		baldEagleRigid = GetComponent<Rigidbody> ();
	}	   

	void Update(){
		Move ();
		if (baldEagleAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "GlideForward" ) {
			if(soaring){
				soaring=false;
				baldEagleAnimator.SetBool ("IsSoaring", false);
				baldEagleAnimator.applyRootMotion = false;
			}
		}else if(baldEagleAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "HoverOnce" ){
			forwardSpeed=meanForwardSpeed*.5f;
			baldEagleAnimator.applyRootMotion = false;
			isFlying = true;
		}
		GroundedCheck ();
	}

	void GroundedCheck(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position+Vector3.up*groundedCheckOffset, Vector3.down, out hit, groundCheckDistance)) {
			if (!soaring && !isGrounded ) {
				Landing ();
				isGrounded = true;		
			}
		} else {
			isGrounded=false;
		}
	}

	public void Landing(){
		baldEagleAnimator.SetBool ("Landing",true);
		baldEagleAnimator.applyRootMotion = true;
		baldEagleRigid.useGravity = true;
		isFlying = false;
	}
	
	public void Soar(){
		if(isGrounded){
			baldEagleAnimator.SetBool ("Landing",false);
			baldEagleAnimator.SetBool ("IsSoaring", true);
			baldEagleRigid.useGravity = false;
			soaring = true;
			isGrounded = false;
		}
	}

	public void Attack(){
		baldEagleAnimator.SetTrigger ("Attack");
	}
	
	public void Hit(){
		baldEagleAnimator.SetTrigger ("Hit");
	}

	public void Move(){
		baldEagleAnimator.SetFloat ("Forward",forwardAcceleration);
		baldEagleAnimator.SetFloat ("Turn",yawVelocity);
		baldEagleAnimator.SetFloat ("UpDown",upDown);
		baldEagleAnimator.SetFloat ("UpVelocity",baldEagleRigid.velocity.y);

		if(isFlying ) {

			if(forwardAcceleration<0f){
				baldEagleRigid.velocity=transform.up*upDown+transform.forward*forwardSpeed;	
			}else{
				baldEagleRigid.velocity=transform.up*(upDown+( forwardSpeed-meanForwardSpeed))+transform.forward*forwardSpeed;
			}

			transform.RotateAround(transform.position,Vector3.up,Time.deltaTime*yawVelocity*100f);
			forwardSpeed=Mathf.Lerp(forwardSpeed,0f,Time.deltaTime*speedDumpingTime);
			forwardSpeed=Mathf.Clamp( forwardSpeed+forwardAcceleration*Time.deltaTime,0f,maxForwardSpeed);
			upDown=Mathf.Lerp(upDown,0,Time.deltaTime*3f);	
		}
	}
}
