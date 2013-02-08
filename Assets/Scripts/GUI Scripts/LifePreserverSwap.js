#pragma strict

var texture: Texture2D;
//var Score : ScoringScript;

function OnGUI () {
if(Input.GetMouseButton(0))
{
//Score = ScoringScript.currentScore;
	GUI.DrawTexture(Rect(390,10.5,32,29),texture);
	Debug.Log("YES");
		}
		}
	
//function Start () {

//myGUITexture.guiTexture.texture = LifePreserver;
//}

//function OnGUI() {
//if (currentScore <=15000)
//{
//myGUITexture.GUITexture.texture = PauseButton2;
//}

