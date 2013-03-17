using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {
	
	public const string TIME_PENALTY = "timePenalty";
	
	private int _score;
	private int _prev_score;
	private bool _running = true;
	private GUIText textfield;
	
	
	//TOTAL SECONDS IN THE CURRENT WORLD
	public int total_seconds = 10000;
	
	//MIN SCORE FOR 'Gold'
	public int gold_min = 6660;
	
	//MIN SCORE FOR 'Silver'
	public int silver_min = 3330;
	
	//HOW MANY SECONDS BETWEEN TICKS
	public const float points_interval = 0.01f;
	
	//HOW MANY POINTS TAKEN AWAY ON EACH TICK
	public const int points_per_interval = 10;
	
	
	void Start () {
		_prev_score = _score = total_seconds;
        textfield = gameObject.GetComponent<GUIText>();
		
		StartCoroutine("Ticker");
	}
	public void show_text( string text, Vector3 loc ){
		GameObject go = new GameObject();
		OnScreenText ost = go.AddComponent<OnScreenText>();
		go.transform.parent = World.current_world.transform;
		ost.textValue = text;
		ost.textPos = loc;
	}
	
	public void subtract_value( int val ){
		_prev_score = _score;
		_score -= val;
		if (_score < gold_min && _prev_score >= gold_min ){
			Messenger<int>.Broadcast(Scoreboard.TIME_PENALTY, _score);
		}else if (_score < silver_min && _prev_score >= silver_min){
			Messenger<int>.Broadcast(Scoreboard.TIME_PENALTY, _score);
		}
		textfield.text = _score.ToString();
	}
	
	IEnumerator Ticker () {
		while(_score > 0 && _running) {
			subtract_value(points_per_interval);
			yield return new WaitForSeconds(points_interval);
	    }

		yield return null;
    }
	
	void onLevelFinish(Rigidbody rb){
		_running = false;
	}
	
	void onPenalty(Rigidbody rb){
		PenaltyObject po = rb.gameObject.GetComponent<PenaltyObject>();
		subtract_value( po.penalty );
		Vector3 screenPos = Camera.mainCamera.WorldToScreenPoint(po.transform.position);
		show_text("-"+po.penalty.ToString(), screenPos);
	}
	
	void OnEnable()	{
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, onLevelFinish);
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_PENALTY_OBJECT, onPenalty);
	}
	
	void OnDisable(){
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, onLevelFinish);
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_PENALTY_OBJECT, onPenalty);
	}
}
