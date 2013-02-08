var speed : float; //Speed of movement
var rotationDamping : float; //Damping on the rotation of the player, when facing towards goal
var graphicsRenderer : Renderer; //Reference to the renderer of player graphics
var WaveForce : float; //Wave force power
var BuoySpeed : float; //Buoy speed towards goal
var BuoySize : float; //Buoy speed towards goal
var MaxVelocityX : float;
var MaxVelocityZ : float;

private var moving : boolean = false; //Whether the player is moving or has stopped
private var goalPos : Vector3; //Position of the goal
private var goalDirection : Vector3; //The direction in which the goal is from the starting position
private var rotationVel : float; //Velocity of the player rotation, when facing towards goal
private var speedModifier : float;
private var rb : Rigidbody; //Reference to the rigidbody component
private var t : Transform; //Reference to the transform component
private var moveStartPos : Vector3; // Movestart position

var LevelCompleteBackground: Texture2D;

function Awake() {
	rb = rigidbody; //Cache rigidbody component
	t = transform; //Cache transform component
	goalPos.y = t.position.y; //Goal y position should be equal to transform y-pos
}

function Update() {
	//if(moving) {
		//Slowly face towards direction of movement
		var lookAt = t.position + goalDirection;
		var rot = t.rotation.eulerAngles; //Save current rotation
		t.LookAt(lookAt); //Set transform to look at the desired lookAt position
		var newYRot = t.rotation.eulerAngles.y; //Save the new y rotation
		var smoothYRot = Mathf.SmoothDampAngle(rot.y, newYRot, rotationVel, rotationDamping); //Smoothly damp to the new y rotation from the current y rotation
		rot.y = smoothYRot;
		t.rotation = Quaternion.Euler(rot); //Assign new rotation
	//}

}

function FixedUpdate() {
	
	//Bottom Left Quadrent, X>Z
	if (((goalPos.x-moveStartPos.x)>=-WaveForce) && ((goalPos.x-moveStartPos.x)<=-BuoySize) && ((goalPos.z-moveStartPos.z)>=-WaveForce) 
	&& ((goalPos.z-moveStartPos.z)<=-BuoySize) && ((goalPos.x-moveStartPos.x)<=(goalPos.z-moveStartPos.z)))
	{
	
		if(moving)
		{

			rb.AddForce(((goalPos.x-moveStartPos.x)+WaveForce)*BuoySpeed,rb.position.y,(((goalPos.x-moveStartPos.x)+WaveForce)*((moveStartPos.z-goalPos.z)/(moveStartPos.x-goalPos.x)))*BuoySpeed);
			//Debug.Log(BuoySize);
			//Debug.Log(goalPos.z-moveStartPos.z);
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);	
			moving = false;

			if (rb.velocity.x > MaxVelocityX) 
			{
				rb.AddForce(-((rb.velocity.x-MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z > MaxVelocityZ)
			{
				rb.AddForce(0,0,-((rb.velocity.z-MaxVelocityZ)/Time.deltaTime));
	

	
				}
			}
		}
	}
	
	//Bottom Left Quadrent Z>X
	else if (((goalPos.x-moveStartPos.x)>=-WaveForce) && ((goalPos.x-moveStartPos.x)<=-BuoySize) && ((goalPos.z-moveStartPos.z)>=-WaveForce) 
	&& ((goalPos.z-moveStartPos.z)<=-BuoySize) && ((goalPos.x-moveStartPos.x)>(goalPos.z-moveStartPos.z)))
		{
	
		if(moving)
		{
			rb.AddForce((((goalPos.z-moveStartPos.z)+WaveForce)*((moveStartPos.x-goalPos.x)/(moveStartPos.z-goalPos.z)))*BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)+WaveForce)*BuoySpeed);
			//rb.AddForce((moveStartPos.x-goalPos.x)*BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)+WaveForce)*BuoySpeed);
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);				
			moving = false;

			if (rb.velocity.x > MaxVelocityX) 
			{
				rb.AddForce(-((rb.velocity.x-MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z > MaxVelocityZ)
			{
				rb.AddForce(0,0,-((rb.velocity.z-MaxVelocityZ)/Time.deltaTime));
	
			//Debug.Log(rb.velocity.z);
			//Debug.Log(goalPos.z);
			//Debug.Log(moveStartPos.z);
	
				}
			}
		}
	}
	
	//Bottom Right Quadrent X>Z
	else if (((goalPos.x-moveStartPos.x)<=WaveForce) && ((goalPos.x-moveStartPos.x)>=BuoySize) && ((goalPos.z-moveStartPos.z)>=-WaveForce) 
	&& ((goalPos.z-moveStartPos.z)<=-BuoySize) && ((goalPos.x-moveStartPos.x)>=(moveStartPos.z-goalPos.z))) 
		{
	
		if(moving)
		{

			//rb.AddForce(((goalPos.x-moveStartPos.x)-WaveForce)*BuoySpeed,rb.position.y,(moveStartPos.z-goalPos.z)*BuoySpeed);
			rb.AddForce(((goalPos.x-moveStartPos.x)-WaveForce)*BuoySpeed,rb.position.y,(((goalPos.x-moveStartPos.x)-WaveForce)*((moveStartPos.z-goalPos.z)/(moveStartPos.x-goalPos.x)))*BuoySpeed);
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);				
			moving = false;
			
			if (rb.velocity.x < -MaxVelocityX) 
			{
				rb.AddForce((((-1*rb.velocity.x)+MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z > MaxVelocityZ)
			{
				rb.AddForce(0,0,-1*((rb.velocity.z-MaxVelocityZ)/Time.deltaTime));
	
			Debug.Log(rb.velocity.x);
			//Debug.Log(goalPos.z);
			//Debug.Log(moveStartPos.z);
	
				}
			}
		}
	}
	
	//Bottom Right Quadrent Z>X
	else if (((goalPos.x-moveStartPos.x)<=WaveForce) && ((goalPos.x-moveStartPos.x)>=BuoySize) && ((goalPos.z-moveStartPos.z)>=-WaveForce) 
	&& ((goalPos.z-moveStartPos.z)<=-BuoySize) && ((goalPos.x-moveStartPos.x)<(moveStartPos.z-goalPos.z))) // check this
		{
	
		if(moving)
		{

			rb.AddForce((((goalPos.z-moveStartPos.z)+WaveForce)*((goalPos.x-moveStartPos.x)/(moveStartPos.z-goalPos.z)))*-BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)+WaveForce)*BuoySpeed);
			//rb.AddForce((moveStartPos.x-goalPos.x)*BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)+WaveForce)*BuoySpeed);
											
			moving = false;
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);	
			if (rb.velocity.x < -MaxVelocityX) 
			{
				rb.AddForce(((-rb.velocity.x+MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z > MaxVelocityZ)
			{
				rb.AddForce(0,0,-((rb.velocity.z-MaxVelocityZ)/Time.deltaTime));
	
			Debug.Log(rb.velocity.x);
			//Debug.Log(goalPos.z);
			//Debug.Log(moveStartPos.z);
	
				}
			}
		}
	}
	
	//Top Right Quadrent Z>X
	else if (((goalPos.x-moveStartPos.x)<=WaveForce) && ((goalPos.x-moveStartPos.x)>=BuoySize) && ((goalPos.z-moveStartPos.z)<=WaveForce) 
	&& ((goalPos.z-moveStartPos.z)>=BuoySize) && ((goalPos.x-moveStartPos.x)<=(goalPos.z-moveStartPos.z)))
		{
	
		if(moving)
		{

			rb.AddForce((((goalPos.z-moveStartPos.z)-WaveForce)*((goalPos.x-moveStartPos.x)/(goalPos.z-moveStartPos.z)))*BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)-WaveForce)*BuoySpeed);

			//rb.AddForce((moveStartPos.x-goalPos.x)*BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)-WaveForce)*BuoySpeed);
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);									
			moving = false;
			
			if (rb.velocity.x < -MaxVelocityX) 
			{
				rb.AddForce(((-rb.velocity.x+MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z < -MaxVelocityZ)
			{
				rb.AddForce(0,0,((-rb.velocity.z+MaxVelocityZ)/Time.deltaTime));
	
			Debug.Log(rb.velocity.x);
			//Debug.Log(goalPos.z);
			//Debug.Log(moveStartPos.z);
	
				}
			}
		}
	}
	
	//Top Right Quadrent X>Z
	else if (((goalPos.x-moveStartPos.x)<=WaveForce) && ((goalPos.x-moveStartPos.x)>=BuoySize) && ((goalPos.z-moveStartPos.z)<=WaveForce) 
	&& ((goalPos.z-moveStartPos.z)>=BuoySize) && ((goalPos.x-moveStartPos.x)>(goalPos.z-moveStartPos.z)))
		{
	
		if(moving)
		{

			//rb.AddForce(((goalPos.x-moveStartPos.x)-WaveForce)*BuoySpeed,rb.position.y,(moveStartPos.z-goalPos.z)*BuoySpeed);
			rb.AddForce(((goalPos.x-moveStartPos.x)-WaveForce)*BuoySpeed,rb.position.y,(((goalPos.x-moveStartPos.x)-WaveForce)*((goalPos.z-moveStartPos.z)/(goalPos.x-moveStartPos.x)))*BuoySpeed);
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);																				
			moving = false;
			
			if (rb.velocity.x < -MaxVelocityX) 
			{
				rb.AddForce(((-rb.velocity.x+MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z < -MaxVelocityZ)
			{
				rb.AddForce(0,0,((-rb.velocity.z+MaxVelocityZ)/Time.deltaTime));
	
			Debug.Log(rb.velocity.x);
			//Debug.Log(goalPos.z);
			//Debug.Log(moveStartPos.z);
	
				}
			}
		}
	}
	
	//Top Left Quadrent X>Z
	else if (((goalPos.x-moveStartPos.x)>=-WaveForce) && ((goalPos.x-moveStartPos.x)<=-BuoySize) && ((goalPos.z-moveStartPos.z)<=WaveForce) 
	&& ((goalPos.z-moveStartPos.z)>=BuoySize) && ((moveStartPos.x-goalPos.x)>=(goalPos.z-moveStartPos.z)))
		{
	
		if(moving)
		{

			rb.AddForce(((goalPos.x-moveStartPos.x)+WaveForce)*BuoySpeed,rb.position.y,(((goalPos.x-moveStartPos.x)+WaveForce)*((goalPos.z-moveStartPos.z)/(moveStartPos.x-goalPos.x)))*-BuoySpeed);
			//rb.AddForce(((goalPos.x-moveStartPos.x)+WaveForce)*BuoySpeed,rb.position.y,(moveStartPos.z-goalPos.z)*BuoySpeed);
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);																				
			moving = false;
			
			if (rb.velocity.x > MaxVelocityX) 
			{
				rb.AddForce(-((rb.velocity.x-MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z < -MaxVelocityZ)
			{
				rb.AddForce(0,0,((-rb.velocity.z+MaxVelocityZ)/Time.deltaTime));
	
			Debug.Log(rb.velocity.x);
			//Debug.Log(goalPos.z);
			//Debug.Log(moveStartPos.z);
	
				}
			}
		}
	}
	
	//Top Left Quadrent Z>X
	else if (((goalPos.x-moveStartPos.x)>=-WaveForce) && ((goalPos.x-moveStartPos.x)<=-BuoySize) && ((goalPos.z-moveStartPos.z)<=WaveForce) 
	&& ((goalPos.z-moveStartPos.z)>=BuoySize) && ((moveStartPos.x-goalPos.x)<(goalPos.z-moveStartPos.z)))
		{
	
		if(moving)
		{
			rb.AddForce((((goalPos.z-moveStartPos.z)-WaveForce)*((moveStartPos.x-goalPos.x)/(goalPos.z-moveStartPos.z)))*-BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)-WaveForce)*BuoySpeed);
			//rb.AddForce((moveStartPos.x-goalPos.x)*BuoySpeed,rb.position.y,((goalPos.z-moveStartPos.z)-WaveForce)*BuoySpeed);
			Debug.Log("Starting Position" + moveStartPos);	
			Debug.Log("Ending Position" + goalPos);																																									
			moving = false;
			
			if (rb.velocity.x > MaxVelocityX) 
			{
				rb.AddForce(-((rb.velocity.x-MaxVelocityX)/Time.deltaTime),0,0);
	
			if (rb.velocity.z < -MaxVelocityZ)
			{
				rb.AddForce(0,0,((-rb.velocity.z+MaxVelocityZ)/Time.deltaTime));
	
			Debug.Log(rb.velocity.x);
			//Debug.Log(goalPos.z);
			//Debug.Log(moveStartPos.z);
	
				}
			}
		}
	}
	
}
	
	

//Called by the camera when a LMB click results in a successful raycast
function MoveTo(pos : Vector3) {
	goalPos = pos; //Set goal position
	goalPos.y = t.position.y; //Set goal y position to player y position
	goalDirection = (goalPos - t.position).normalized; //Calculate goalDirection and normalize
	
	moving = true; //Enable movement
	moveStartPos = t.position; //Store starting position of movement
	}

function OnCollisionEnter(collision : Collision) {    
    if (collision.gameObject.name == "LevelGoal"){
    Debug.Log("HIT");
    Application.LoadLevel("LevelComplete");
  			//Debug.Log(rb.velocity.y);
 } 			
else if (collision){
    rb.AddForce(0,-250,0);
  			//Debug.Log(rb.velocity.y);
 } 			
}

function GetRenderer() {
	return graphicsRenderer;
}