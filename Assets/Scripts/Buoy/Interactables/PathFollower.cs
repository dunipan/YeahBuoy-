using UnityEngine;
using System.Collections;

public class PathFollower : Interactable {
	public string path_name = "Kayak Bottom Path";
	
	protected Vector3[] path;
	protected int current_index = 0;
	
	protected bool reverse = false;
	protected bool loop = false;	
	
	protected void FixedUpdate(){
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
		D.Log<string>(distance.ToString());
		DoTest( path[current_index] , 20.0f);
		
		GameObject ripple = GameObject.FindGameObjectWithTag("Ripple");
		if (ripple){
			Vector3 ripple_pos = path[current_index] + new Vector3(0,1,0);
			ripple_pos.y = transform.position.y;
			ripple.transform.position = ripple_pos;
		}
		
		if( lookAtEnabled ){
		    lookAtTarget.y = transform.position.y;
			Quaternion q = Quaternion.LookRotation(lookAtTarget - transform.position);
			float f = 5 * Time.deltaTime;
		    transform.rotation = Quaternion.Slerp(transform.rotation, q , f);
		}
	}
	
	protected Vector3 DoTest(Vector3 worldPosVector3, float force_multiplier) {
		if (debug){
			if (!debug_cube){
				debug_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Collider.Destroy(debug_cube.GetComponent<BoxCollider>());
				debug_cube.transform.parent = gameObject.transform;
			}
		}
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
		}
		return worldPosVector3;
    }
}
