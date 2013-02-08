
// textfield to hold the score

// the textfield to update the time to
public var textfield:GUIText;
public var score:int;

// time variables
public var allowedScore:int = 10000;
public var currentScore;
public var GoldScore:int;
public var SilverScore:int;
public var BronzeScore:int;

var LifePreserverTexture: Texture2D;
var NoLifePreserverTexture: Texture2D;
var LifePreserverBackground: Texture2D;

var isGameOver : System.Boolean = false;
var GameOverScreen : Texture;
var TransparentButton: GUIStyle;

function Start()
{
	
	// retrieve the GUIText Component and set the score
	textfield = GameObject.Find("GUI Score/txt-score").GetComponent(GUIText);
    currentScore = allowedScore;
	textfield.text = allowedScore.ToString();
	
	InvokeRepeating("Tick2",.01,.01);
	// start the timer ticking
	Tick2();
	}

function Tick2()
{
currentScore-=10;
// update textfield with score
	textfield.text = currentScore.ToString();

}

		
function UpdateScoreText()
{
	
	}
	
function OnGUI (){

GUI.DrawTexture(Rect(631,10.5,LifePreserverBackground.width/3.9,LifePreserverBackground.height/3.9),LifePreserverBackground);
if(currentScore>GoldScore)
{
	GUI.DrawTexture(Rect(640,12,32,29),LifePreserverTexture);
	GUI.DrawTexture(Rect(680,12,32,29),LifePreserverTexture);
	GUI.DrawTexture(Rect(720,12,32,29),LifePreserverTexture);
		}
if((currentScore<GoldScore)&&(currentScore>SilverScore))
{
	GUI.DrawTexture(Rect(640,12,32,29),NoLifePreserverTexture);
	GUI.DrawTexture(Rect(680,12,32,29),LifePreserverTexture);
	GUI.DrawTexture(Rect(720,12,32,29),LifePreserverTexture);
		}
if((currentScore<SilverScore)&&(currentScore>BronzeScore))
{
	GUI.DrawTexture(Rect(640,12,32,29),NoLifePreserverTexture);
	GUI.DrawTexture(Rect(680,12,32,29),NoLifePreserverTexture);
	GUI.DrawTexture(Rect(720,12,32,29),LifePreserverTexture);
		}	
if(currentScore<BronzeScore)
{
	GUI.DrawTexture(Rect(640,12,32,29),NoLifePreserverTexture);
	GUI.DrawTexture(Rect(680,12,32,29),NoLifePreserverTexture);
	GUI.DrawTexture(Rect(720,12,32,29),NoLifePreserverTexture);
		}
		
if(currentScore<0)
{
	if (Time.timeScale == 1.0){			 	
				Time.timeScale = 0.0;
				Debug.Log("Game Is Paused");     
				
				isGameOver = true; 
	
		}		
	if(isGameOver){
		
		//GUI.Label(Rect(screenCenterX - 50, 50, 200, 40), "GAME OVER", style.active);
	GUI.Label(Rect(0,-4,GameOverScreen.width,GameOverScreen.height),GameOverScreen);
			
	if(GUI.Button(Rect(310, 270, 80, 60), "", TransparentButton)){
			Application.LoadLevel(4);
		}
		
	if(GUI.Button(Rect(200, 260, 80, 55), "", TransparentButton)){
			Application.LoadLevel("Level Select");
		}
												
		}
}

}