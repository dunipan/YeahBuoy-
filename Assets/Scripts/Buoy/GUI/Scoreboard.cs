using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {
	
	public const int START_SCORE = 10000;
	public const int TICK_VALUE = 10;
	public const float TICK_INTERVAL = 0.01f;
	
	private int _score;
	private GUIText textfield;
	
	void Start () {
		
		_score = Scoreboard.START_SCORE;
        textfield = gameObject.GetComponent<GUIText>();
		
		StartCoroutine("Ticker");
	}
	
	
	IEnumerator Ticker () {
		while(_score > 0) {
			_score -= Scoreboard.TICK_VALUE;
			textfield.text = _score.ToString();
			yield return new WaitForSeconds(Scoreboard.TICK_INTERVAL);
	    }

		yield return null;
    }
}
