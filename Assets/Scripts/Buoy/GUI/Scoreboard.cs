using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {
	
	public const string TIME_PENALTY = "timePenalty";
	
	public const int START_SCORE = 10000;
	public const int TICK_VALUE = 10;
	public const float TICK_INTERVAL = 0.01f;
	
	private int _score;
	private bool _running = true;
	private GUIText textfield;
	
	void Start () {
		_score = Scoreboard.START_SCORE;
        textfield = gameObject.GetComponent<GUIText>();
		
		StartCoroutine("Ticker");
	}
	
	
	IEnumerator Ticker () {
		while(_score > 0 && _running) {
			_score -= Scoreboard.TICK_VALUE;
			if (_score == 6660){
				Messenger<int>.Broadcast(Scoreboard.TIME_PENALTY, _score);
			}else if (_score == 3330){
				Messenger<int>.Broadcast(Scoreboard.TIME_PENALTY, _score);
			}
			textfield.text = _score.ToString();
			yield return new WaitForSeconds(Scoreboard.TICK_INTERVAL);
	    }

		yield return null;
    }
	
	void onLevelFinish(Rigidbody rb){
		_running = false;
	}
	
	void OnEnable()	{
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_WIN_OBJECT, onLevelFinish);
	}
	
	void OnDisable(){
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_WIN_OBJECT, onLevelFinish);
	}
}
