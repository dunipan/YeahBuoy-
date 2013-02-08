#pragma strict

var LevelMenu : Texture;
var TransparentButton : GUIStyle;

function OnGUI () {
GUI.Label(Rect(0,-4,LevelMenu.width,LevelMenu.height),LevelMenu);
	
		if(GUI.Button(Rect(180, 80, 150, 150), "", TransparentButton)){
			Application.LoadLevel("Beach Level 1");
			
		} 
		if(GUI.Button(Rect(400, 90, 150, 150), "", TransparentButton)){
			Application.LoadLevel("Lake Level 1");
		}
		
		
	}

