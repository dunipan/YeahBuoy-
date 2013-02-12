using UnityEngine;
using System.Collections;

public class Buoy : Interactable {
	//MAX DISTANCE A CLICK CAN BE FROM THE BUOY'S CENTER
	const float RADIUS = 7.0f;
	
	//FORCE MULTIPLIER FOR OUR CLICKS
	const float POWER = 180.0f;
	
	bool Looking = true;
	Transform swimmer;
	
	void Start () {
		base.Start();
		gameObject.tag = "Buoy";
		swimmer = GameObject.FindGameObjectWithTag("Swimmer").transform;
	}
	
	void Update () { }
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
		transform.LookAt(worldPosVector3);
    }
	
	protected override void DoInteraction(Rigidbody rb) 
	{
		D.Log("Buoy hit: " + rb.gameObject.ToString());
	}
	
}