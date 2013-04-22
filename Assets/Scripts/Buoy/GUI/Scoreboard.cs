using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {
	
	public const string TIME_PENALTY = "timePenalty";
	
	private int _score;
	private int _prev_score;
	private static bool _running = true;
	private static bool _over = false;
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
		
		D.Log<string>("GAME OVER IN : " + (total_seconds*points_interval).ToString() + " SECONDS");
		_over = false;
		StartCoroutine("Ticker");
	}
	
	public static bool clock_ticking
	{
	    get { return _running; }
	}
	
	public static bool is_over{
		get{ return _over; }
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
		_over = true;
	}
	
	void onSwimmerHit(Rigidbody rb){
		_score += 1000;
	}
	
	void onDeadSwimmer(GameObject go){
		_running = false;
		_over = true;
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
		Messenger<GameObject>.AddListener(WinObject.WIN_OBJECT_DEAD, onDeadSwimmer);
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_WIN_OBJECT, onSwimmerHit);
	}
	
	void OnDisable(){
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, onLevelFinish);
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_PENALTY_OBJECT, onPenalty);
		Messenger<GameObject>.RemoveListener(WinObject.WIN_OBJECT_DEAD, onDeadSwimmer);
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_WIN_OBJECT, onSwimmerHit);
	}
}
