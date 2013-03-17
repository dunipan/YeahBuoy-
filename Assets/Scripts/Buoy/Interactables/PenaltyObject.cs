using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenaltyObject : Interactable {
	
	public int penalty = 200;
	
	void Start () {
		base.Start();
	}
	
	protected override void DoInteraction(Rigidbody rb) { }
	
	
}
