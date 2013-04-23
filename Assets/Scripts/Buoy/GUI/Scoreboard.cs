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
	public const float points_interval = 0.1f;
	
	//HOW MANY POINTS TAKEN AWAY ON EACH TICK
	public const int points_per_interval = 10;
	
	
	void Start () {
		_prev_score = total_seconds;
		_score = total_seconds;
        textfield = gameObject.GetComponent<GUIText>();
		D.Log<GUIText>(textfield);
		D.Log<string>("GAME OVER IN : " + ( (total_seconds/points_per_interval)*points_interval).ToString() + " SECONDS");
		_over = false;
		StartCoroutine("Ticker");
		
		string key = Application.loadedLevelName + "_score";
		if (PlayerPrefs.HasKey(key)){
			D.Log<string>("YOUR PREVIOUS BEST SCORE WAS : " + PlayerPrefs.GetInt(key).ToString());
		}else{
			D.Log<string>("YOU HAVE NEVER BEAT THIS LEVEL");
		}
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
		float t1 = Time.time;
		while(_score > 0 && _running) {
			subtract_value(points_per_interval);
			yield return new WaitForSeconds(points_interval);
	    }
		D.Warn<string>("GAME DONE IN : " + (Time.time-t1).ToString());
		yield return null;
    }
	
	void onLevelFinish(Rigidbody rb){
		_running = false;
		_over = true;
		PostGame();
	}
	
	void onSwimmerHit(Rigidbody rb){
		_score += 1000;
	}
	
	void onDeadSwimmer(GameObject go){
		_running = false;
		_over = true;
		_score = 0;
		PostGame();
	}
	
	void PostGame(){
		string key = Application.loadedLevelName + "_score";
		if (PlayerPrefs.HasKey(key)){
			if (PlayerPrefs.GetInt(key) < _score){
				PlayerPrefs.SetInt(key, _score);
			}
		}else{
			PlayerPrefs.SetInt(key, _score);
		}
		
		Application.LoadLevel("LevelComplete");
	}
	
	void onPenalty(Rigidbody rb){
		PenaltyObject po = rb.gameObject.GetComponent<PenaltyObject>();
		subtract_value( po.penalty );
		Vector3 screenPos = Camera.mainCamera.WorldToScreenPoint(po.transform.position);
		show_text("-"+po.penalty.ToString(), screenPos);
	}
	
	void OnEnable()	{
		textfield = gameObject.GetComponent<GUIText>();
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, onLevelFinish);
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_PENALTY_OBJECT, onPenalty);
		Messenger<GameObject>.AddListener(WinObject.WIN_OBJECT_DEAD, onDeadSwimmer);
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_WIN_OBJECT, onSwimmerHit);
	}
	
	void OnDisable(){
		textfield = null;
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, onLevelFinish);
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_PENALTY_OBJECT, onPenalty);
		Messenger<GameObject>.RemoveListener(WinObject.WIN_OBJECT_DEAD, onDeadSwimmer);
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_WIN_OBJECT, onSwimmerHit);
	}
}
