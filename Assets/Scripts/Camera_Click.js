var panningSpeed : float; //Speed used when panning the camera
var followDamping : float; //Damping on the movement of the camera
var followPlayer : boolean; //Used for fixing camera motion on player or freeing its movement
var player : Player_Click; //Transform of the player game object
var screenEdgeWidth : float; //Width of the "screen-edge", used for panning the camera

private var xVel : float; //x velocity when camera is following the player
private var zVel : float; //z velocity when camera is following the player
private var playerT : Transform; //Reference to the transform component of the Player
private var offsetFromPlayer : Vector3; //Position offset of camera from player when the game starts
private var t : Transform; //Reference to the transform component of the game object

public var hitPointX: WakeInstantiate;
public var hitPointY: WakeInstantiate;
public var hitPointZ: WakeInstantiate;

function Awake() {
	t = transform; //Cache transform component
	if(player) { //If player exists
		playerT = player.transform; //Cache player's transform component
		offsetFromPlayer = t.position - playerT.position; //Calculate initial offset from player
	}
}

function Update() {
	if(Input.GetMouseButtonDown(0)) { //If LMB is clicked
		var ray = camera.ScreenPointToRay(Input.mousePosition); //Create a ray
		var hit = RaycastHit(); //Create a RaycastHit struct to hold information about the raycast
		if(Physics.Raycast(ray, hit)) { //Cast the ray into the scene
			player.MoveTo(hit.point); //Set the Player's goal to the point in the scene that was hit by the ray
			hitPointX.hitPointX = hit.point.x;
			hitPointY.hitPointY = hit.point.y;
			hitPointZ.hitPointZ = hit.point.z-2;
			hitPointX.Instantiate();
			Debug.Log("X"+hit.point.x);
			Debug.Log("HitPointX"+hitPointX);
			Debug.Log("Z"+hit.point.z);
			}
	}
}

//For cameras that follow another transform's position, always update using LateUpdate
function LateUpdate () {
	if(followPlayer && playerT) {
		t.position.x = Mathf.SmoothDamp(t.position.x, playerT.position.x + offsetFromPlayer.x, xVel, followDamping);
		t.position.z = Mathf.SmoothDamp(t.position.z, playerT.position.z + offsetFromPlayer.z, zVel, followDamping);
	} else {
		//Check the mouse pointer position to see if it's at the edges of the screen
		var mousePos = Input.mousePosition;
		var panMovement : Vector2;
		//Set direction of panning movement
		if(mousePos.x < screenEdgeWidth && mousePos.x > 0)
			panMovement.x = -1;
		else if(mousePos.x > Screen.width - screenEdgeWidth && mousePos.x < Screen.width)
			panMovement.x = 1;
		if(mousePos.y < screenEdgeWidth && mousePos.y > 0)
			panMovement.y = -1;
		else if(mousePos.y > Screen.height - screenEdgeWidth && mousePos.y < Screen.height)
			panMovement.y = 1;
		//Calculate new position based on panning direction and speed
		t.position = t.position + Vector3(panMovement.x, 0, panMovement.y) * panningSpeed * Time.deltaTime;
	}
}