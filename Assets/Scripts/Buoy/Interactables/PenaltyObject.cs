using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenaltyObject : PathFollower {
	
	public int penalty = 200;
	
	void Start () {
		base.Start();
		path = iTweenPath.GetPath( this.path_name );
		//debug = true;
		lookAtEnabled = true;
		loop = true;
	}
		
	
	protected override void DoInteraction(Rigidbody rb) { }
	
	
}
