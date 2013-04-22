using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFollower : Interactable {
	public string path_name = "Kayak Bottom Path";
	
	public float travel_speed = 40.0f;
	public float animation_cooldown_on_interaction = 5.0f;
	
	public Vector3 LookAdjustment = new Vector3(0, 0, 0);
	
	public Vector3[] path;
	protected int current_index = 0;
	protected int prev_index = 0;
	
	protected bool reverse = false;
	protected bool loop = false;
	protected bool curve = false;
	protected bool running = true;
		
	protected void Start(){
		base.Start();
		path = iTweenPath.GetPath( path_name );
		if(path != null){
			if (path.Length > 4 && curve){
				int loops = 0;
				prev_index = current_index;
				List<Vector3> bezier_path = new List<Vector3>();
				
				
				Curve b;
				float t = 0.0f;
				do {
					b = GetNextCurve();
					t = 0.0f;
					while ( t < 1.0f){
						bezier_path.Add( b.GetPointAtTime( t ));
						t += 0.1f;
					}
					
					prev_index = current_index;
					loops++;
					if (loops > 256){
						Debug.LogError("TOO MANY LOOPS IN PATH_FOLLOWER");
						break;
					}
				}while(b.EndPoint != path[0]);
				path = bezier_path.ToArray();
			}
		}
	}
	
	private Curve GetNextCurve(){
		if (current_index >= path.Length)
		{
			current_index = 0;
		}
		int index1 = current_index;
		int index2 = current_index+1;
		int index3 = current_index+2;
		if (current_index == path.Length - 2){
			index3 = 0;
			current_index = 0;
		}else if (current_index == path.Length - 1){
			index2 = 0;
			index3 = 1;
			current_index = 1;
		}else{
			current_index = current_index + 2;
		}
		return new Curve( path[index1],  path[index2], path[index3] );
	}
	/*
	private Bezier GetNextBezier(){
		if (current_index >= path.Length)
		{
			current_index = 0;
		}
		int index1 = current_index;
		int index2 = current_index+1;
		int index3 = current_index+2;
		int index4 = current_index+3;
		if (current_index == path.Length - 3){
			index4 = 0;
			current_index = 0;
		}else if (current_index == path.Length - 2){
			index3 = 0;
			index4 = 1;
			current_index = 1;
		}else if (current_index == path.Length - 1){
			index2 = 0;
			index3 = 1;
			index4 = 2;
			current_index = 2;
		}else{
			current_index = current_index + 4;
		}
		//D.Log<string>( prev_index.ToString() + " -> " + current_index.ToString() );
		return new Bezier( path[index1],  path[index2], path[index3], path[index4] );
	}
	*/
	protected void FixedUpdate(){
		if (running){
			Vector3 target = path[current_index];
			target.y = transform.position.y;
			float distance = Vector3.Distance( transform.position, target);
			if (distance < 1.0f){
				if (loop){
					current_index++;
				}else{
					if (reverse){
						current_index--;
					}else{
						current_index++;
					}
				}
				if ( current_index < 0){
					reverse = false;
					current_index = 0;
				}else if(current_index >= path.Length ){
					if(loop){
						current_index = 0;
					}else{
						reverse = true;
						current_index = path.Length - 1;
					}
				}
				lookAtEnabled = true;
			}
			ApplyForceToRigidbody( path[current_index] , travel_speed );
			if( lookAtEnabled ){
			    lookAtTarget.y = transform.position.y;
				Quaternion q = Quaternion.LookRotation(lookAtTarget - transform.position);
				q.eulerAngles = q.eulerAngles + LookAdjustment;
				float f = 5 * Time.deltaTime;
				q = Quaternion.Slerp(transform.rotation, q , f);
				
			    transform.rotation = q;
			}
		}
	}
	
	protected Vector3 ApplyForceToRigidbody(Vector3 worldPosVector3, float force_multiplier) {
		if (_rb){
			//DON'T APPLY ANY FORCE VERTICALLY
			worldPosVector3.y = transform.position.y;
			
			//GET THE DISTANCE BETWEEN OUR TWO POINTS
			Vector3 direction = _rb.transform.position - worldPosVector3;
			if (debug_cube){
				debug_cube.transform.localPosition = direction.normalized;
			}
			direction = direction.normalized;
			direction.x *= -1;
			direction.z *= -1;
			//ADD OUR FORCE
			_rb.AddForce(direction.normalized * force_multiplier);
			
			//ROTATE TO LOOK IN THE DIRECTION WE ARE GOING
			lookAtTarget = worldPosVector3;
			lookAtEnabled = true;
		}else{
			D.Warn<string>("NO RB ON PATH FOLLOWER");
		}
		return worldPosVector3;
    }
	
	protected override void DoInteraction(Rigidbody rb) {
		if (running){
			if (rb.gameObject.GetComponent<Buoy>() != null){
				StartCoroutine("Cooldown");
			}
		}
	}
	
	IEnumerator Cooldown () {
		while(running) {
			running = false;
			yield return new WaitForSeconds(animation_cooldown_on_interaction);
	    }
		running = true;
		yield return null;
    }
}
