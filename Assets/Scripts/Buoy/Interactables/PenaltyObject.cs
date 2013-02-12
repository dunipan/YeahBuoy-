using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenaltyObject : Interactable {
	
	void Start () {
		base.Start();
	}
	
	void Update () {
	
	}
	
	protected override void DoInteraction(Rigidbody rb) 
	{
		D.Log ("PO hit: " + rb.gameObject.ToString());
	}
	
	
}
