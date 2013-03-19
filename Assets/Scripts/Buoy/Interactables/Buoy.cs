using UnityEngine;
using System.Collections;

public class Buoy : Interactable {
	
	public const string BUOY_HIT_PENALTY_OBJECT = "buoyHitPenaltyObject";
	public const string BUOY_HIT_WIN_OBJECT = "buoyHitWinObject";
	public const string BUOY_HIT_FINAL_WIN_OBJECT = "buoyHitFinalWinObject";
	
	//MAX DISTANCE A CLICK CAN BE FROM THE BUOY'S CENTER
	public const float RADIUS = 7.0f;
	
	//FORCE MULTIPLIER FOR OUR CLICKS
	public const float POWER = 180.0f;
	
	private Vector3 lookAtTarget = Vector3.zero;
	private bool lookAtEnabled = false;
		
	void Start () {
		base.Start();
		gameObject.tag = "Buoy";
	}
	
	void FixedUpdate (){ }
	
	float ForceMultiplier(Vector3 worldPosVector3){
		//FIND THE DISTANCE FROM THE BUOY TO THE CLICK POINT AND SUBTRACT THAT FROM THE RADIUS TO
		//GET AN INCREASING LARGER NUMBER AS WE GET CLOSER TO THE BUOY OR A NEGATIVE IF WE ARE OUTSIDE IT
		//MULTIPLE THAT BY OUR POWER VALUE WHICH WILL ALLOW US TO FINE TUNE THE DISTANCE TRAVELLED
		return Mathf.Max(0.0f, RADIUS - Vector3.Distance(rigidbody.transform.position, worldPosVector3)) * POWER;
	}
	
	void ApplyForce(Vector3 worldPosVector3) {
		//DON'T APPLY ANY FORCE VERTICALLY
		worldPosVector3.y = transform.position.y;
		
		GameObject ripple = GameObject.FindGameObjectWithTag("Ripple");
		if (ripple){
			ripple.transform.position = worldPosVector3 + new Vector3(0,1,0);
			Animation ripple_anim = ripple.GetComponent<Animation>();
			if (ripple_anim){
				ripple_anim.Play();
			}
		}
		
		//GET THE DISTANCE BETWEEN OUR TWO POINTS
		Vector3 direction = rigidbody.transform.position - worldPosVector3;
		
		//ADD OUR FORCE
		rigidbody.AddForce(direction.normalized * ForceMultiplier(worldPosVector3));
		
		//ROTATE TO LOOK IN THE DIRECTION WE ARE GOING
		lookAtTarget = worldPosVector3;
		lookAtEnabled = true;
    }
	
	protected void LateUpdate(){
		if( lookAtEnabled ){
		    lookAtTarget.y = transform.position.y;
			Quaternion q = Quaternion.LookRotation(lookAtTarget - transform.position);
			float f = 5 * Time.deltaTime;
			float dot = Quaternion.Dot(q, transform.rotation);
			if (dot > 0.99999f){
				lookAtEnabled = false;
			}
		    transform.rotation = Quaternion.Slerp(transform.rotation, q , f);
		}
	}
	
	protected override void DoInteraction(Rigidbody rb) 
	{
		PenaltyObject po = rb.gameObject.GetComponent<PenaltyObject>();
		if (po){
			Messenger<Rigidbody>.Broadcast(Buoy.BUOY_HIT_PENALTY_OBJECT, rb);
		}
	}
	
}