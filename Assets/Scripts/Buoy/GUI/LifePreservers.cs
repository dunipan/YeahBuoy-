using UnityEngine;
using System.Collections;

public class LifePreservers : MonoBehaviour {
	
	public const int NUMBER_OF_PRESERVERS = 3;
	
	public const float LIFE_PRESERVER_Y_OFFSET = 18.0f;
	public const float BACKGROUND_Y_OFFSET = 10.0f;
	
	public const float LIFE_PRESERVER_WIDTH = 32.0f;
	public const float LIFE_PRESERVER_HEIGHT = 29.0f;
	public const float LIFE_PRESERVER_SPACING = 12.0f;
	
	public const float BACKGROUND_WIDTH = 481.0f * 0.3f;
	public const float BACKGROUND_HEIGHT = 160.0f * 0.3f;
	
	public Texture2D LifePreserverTexture;
	public Texture2D NoLifePreserverTexture;
	public Texture2D LifePreserverBackground;
	
	private int _score;
	
	void Start () {
		_score = LifePreservers.NUMBER_OF_PRESERVERS;
	}
	
	private float LifePreserverXOffset( int pos ){
		if (pos > 0){
			return LifePreservers.LIFE_PRESERVER_WIDTH * pos + LifePreservers.LIFE_PRESERVER_SPACING * pos;
		}
		return LifePreservers.LIFE_PRESERVER_WIDTH * pos;
	}
	
	void OnGUI () {
		//MAKE SURE THE TEXTURES ARE SET
		if (!LifePreserverTexture){
			Debug.LogError("Assign a 'LifePreserverTexture' Texture in the inspector");
			return;
		}
		if (!NoLifePreserverTexture){
			Debug.LogError("Assign a 'NoLifePreserverTexture' Texture in the inspector");
			return;
		}
		if (!LifePreserverBackground){
			Debug.LogError("Assign a 'LifePreserverBackground' Texture in the inspector");
			return;
		}
		
		//GET LOCATION OF BACKGROUND
		float tl_x = (Screen.width / 2.0f) - ( LifePreservers.BACKGROUND_WIDTH / 2.0f );
		float tl_y = LifePreservers.BACKGROUND_Y_OFFSET;
		
		//DRAW THE BACKGROUND
		GUI.DrawTexture(
						new Rect(tl_x, tl_y, LifePreservers.BACKGROUND_WIDTH, LifePreservers.BACKGROUND_HEIGHT),
						LifePreserverBackground
						);
		
		//FIGURE OUT THE TOP LEFT OF THE LIFE PRESERVERS
		tl_x = (Screen.width / 2) - ( LifePreservers.NUMBER_OF_PRESERVERS * LifePreservers.LIFE_PRESERVER_WIDTH + (LifePreservers.NUMBER_OF_PRESERVERS-1)*LifePreservers.LIFE_PRESERVER_SPACING ) / 2;
		tl_y = LifePreservers.LIFE_PRESERVER_Y_OFFSET;
		
		int diff = LifePreservers.NUMBER_OF_PRESERVERS - _score;
		
		//DRAW THE LIFE PRESERVERS
		int i = 0;
		
		while (i < LifePreservers.NUMBER_OF_PRESERVERS){
			Rect lpRect = new Rect( tl_x + LifePreserverXOffset(i), tl_y, LifePreservers.LIFE_PRESERVER_WIDTH, LifePreservers.LIFE_PRESERVER_HEIGHT);
			if (i >= diff)
			{
				GUI.DrawTexture(lpRect, LifePreserverTexture);
			}else{
				GUI.DrawTexture(lpRect, NoLifePreserverTexture);
			}
			i++;
		}
	}
	
	void onTimePenalty(int score){
		D.Log<string>( score.ToString() );
		_score -= 1;
	}
	
	void onBuoyHit(Rigidbody rb){
		_score -= 1;
	}
	
	void OnEnable()	{
		Messenger<int>.AddListener(Scoreboard.TIME_PENALTY, onTimePenalty);
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_OBJECT, onBuoyHit);
	}
	
	void OnDisable(){
		Messenger<int>.RemoveListener(Scoreboard.TIME_PENALTY, onTimePenalty);
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_OBJECT, onBuoyHit);
	}
}
