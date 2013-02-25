using UnityEngine;
using System.Collections;

public class WinObject : Interactable {

	// Use this for initialization
	void Start () {
	
	}
	
	protected override void DoInteraction(Rigidbody rb) {
		Buoy buoy = rb.gameObject.GetComponent<Buoy>();
		if (buoy){
			Messenger<Rigidbody>.Broadcast(Buoy.BUOY_HIT_WIN_OBJECT, rb);
		}
	}
}
