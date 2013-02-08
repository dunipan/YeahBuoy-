#pragma strict

var LevelComplete : Texture;
var TransparentButton : GUIStyle;

function OnGUI () {
GUI.Label(Rect(0,-4,LevelComplete.width,LevelComplete.height),LevelComplete);
	
		if(GUI.Button(Rect(215, 330, 75, 75), "", TransparentButton)){
			Application.LoadLevel("LevelSelect");
			
		} 
		if(GUI.Button(Rect(330, 340, 75, 75), "", TransparentButton)){
			Application.LoadLevel("Lake Level 1");
		}
		
		
	}

