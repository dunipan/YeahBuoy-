#pragma strict

var isPaused : System.Boolean = false;

var pauseButton : Rect;

var screenCenterX : float;
var screenCenterY : float;

var style : GUIStyle;

var PauseMenu : Texture;
var TransparentButton : GUIStyle;

function Start () {
	Time.timeScale = 1.0;
}

function Update () {
	screenCenterX = Screen.width / 2;
	screenCenterY = Screen.height / 2;
}

function OnGUI () {

	//Pause Button
	if(isPaused == false){
		if(GUI.Button(pauseButton, "", style)){
			if (Time.timeScale == 1.0){			 	
				Time.timeScale = 0.0;
				Debug.Log("Game Is Paused");     
				
				isPaused = true;      		 
			}else{
				Time.timeScale = 1.0;
				Debug.Log("Game Is Un-Paused");
				
				isPaused = false;
			}
		}
	}
	
	//Pause Menu
	if(isPaused){
		
		//GUI.Label(Rect(screenCenterX - 50, 50, 200, 40), "GAME PAUSED", style.active);
	GUI.Label(Rect(0,-4,PauseMenu.width,PauseMenu.height),PauseMenu);
	
		if(GUI.Button(Rect(85, 370, 120, 40), "", TransparentButton)){
			Time.timeScale = 1.0;
			
			isPaused = false;
		} 
		if(GUI.Button(Rect(80, 135, 80, 50), "", TransparentButton)){
			Application.LoadLevel(4);
		}
		
		if(GUI.Button(Rect(85, 210, 80, 55), "", TransparentButton)){
			Application.LoadLevel("Level Select");
		}
	}
}