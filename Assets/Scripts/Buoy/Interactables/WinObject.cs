using UnityEngine;
using System.Collections;

public class WinObject : Interactable {

	// Use this for initialization
	void Start () {
		base.Start();
		World.winning_object_created(this.gameObject);
	}
	
	protected override void DoInteraction(Rigidbody rb) {
		Buoy buoy = rb.gameObject.GetComponent<Buoy>();
		if (buoy){
			World.winning_object_hit(this.gameObject);
		}
	}
}
