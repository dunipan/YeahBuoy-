using UnityEngine;
using System.Collections;

public class Buoy : Interactable {
	
	public const string BUOY_HIT_PENALTY_OBJECT = "buoyHitPenaltyObject";
	public const string BUOY_HIT_WIN_OBJECT = "buoyHitWinObject";
	public const string BUOY_HIT_FINAL_WIN_OBJECT = "buoyHitFinalWinObject";
	public const string BUOY_NEW_FORCE = "buoyNewForce";
	
	//MAX DISTANCE A CLICK CAN BE FROM THE BUOY'S CENTER
	public const float RADIUS = 7.0f;
	
	//FORCE MULTIPLIER FOR OUR CLICKS
	public const float POWER = 180.0f;
	
	
	private GameObject _debug_cube;
	
	void Start () {
		base.Start();
		gameObject.tag = "Buoy";
		
		_debug_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_debug_cube.renderer.material = World.current_world.ruler_mesh1;
		Collider.Destroy(_debug_cube.collider);
	}
	
	void FixedUpdate (){
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
		_debug_cube.transform.position = transform.position + rigidbody.velocity;
	}
	
	float ForceMultiplier(Vector3 worldPosVector3){
		//FIND THE DISTANCE FROM THE BUOY TO THE CLICK POINT AND SUBTRACT THAT FROM THE RADIUS TO
		//GET AN INCREASING LARGER NUMBER AS WE GET CLOSER TO THE BUOY OR A NEGATIVE IF WE ARE OUTSIDE IT
		//MULTIPLE THAT BY OUR POWER VALUE WHICH WILL ALLOW US TO FINE TUNE THE DISTANCE TRAVELLED
		return Mathf.Max(0.0f, RADIUS - Vector3.Distance(rigidbody.transform.position, worldPosVector3)) * POWER;
	}
	
	void ApplyForce(Vector3 worldPosVector3) {
		GameObject ripple = GameObject.FindGameObjectWithTag("Ripple");
		if (ripple){
			Vector3 ripple_pos = worldPosVector3 + new Vector3(0,1,0);
			ripple_pos.y = GetTargetY();
			ripple.transform.position = ripple_pos;
			Animation ripple_anim = ripple.GetComponent<Animation>();
			if (ripple_anim){
				ripple_anim.Play();
			}
		}
		
		//ADD OUR FORCE
		ApplyForceToRigidBody(worldPosVector3, ForceMultiplier(worldPosVector3));
    }
	
	protected override void DoInteraction(Rigidbody rb) 
	{
		PenaltyObject po = rb.gameObject.GetComponent<PenaltyObject>();
		if (po){
			Messenger<Rigidbody>.Broadcast(Buoy.BUOY_HIT_PENALTY_OBJECT, rb);
		}
	}
	
}