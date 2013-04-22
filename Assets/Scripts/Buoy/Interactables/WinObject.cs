using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinObject : Interactable {
	public const string WIN_OBJECT_DEAD = "winObjectDead";
	
	protected bool _running;
	public int points_left = 1001;
	public float points_interval = 0.1f;
	
	private bool safe = false;
		
	void Start () {
		base.Start();
		World.winning_object_created(this.gameObject);
		
		if (points_left > 0){
			_running = true;
			StartCoroutine("Ticker");
		}else{
			_running = false;
		}
		
		D.Log<string>(gameObject.name + " : DEAD IN " + (points_left * points_interval).ToString() + " SECONDS");
	}
	
	protected void ShowTimer(){
		if ( this.guiText ){
			return;
		}
		GUIText text = this.gameObject.AddComponent<GUIText>();
		text.fontSize = 16;
		text.pixelOffset = Camera.mainCamera.WorldToScreenPoint(this.gameObject.transform.position);
	}
	
	protected override void DoInteraction(Rigidbody rb) {
		Buoy buoy = rb.gameObject.GetComponent<Buoy>();
		if (buoy && !safe){
			safe = true;
			World.winning_object_hit(this.gameObject);
		}
	}
	
	IEnumerator Ticker () {
		while(points_left > 0 && _running) {
			points_left -= 1;
			yield return new WaitForSeconds(points_interval);
	    }
		if (points_left <= 0 && !safe){
			D.Log<string>("WIN_OBJECT_DEAD");
			D.Log<GameObject>(gameObject);
			Messenger<GameObject>.Broadcast(WinObject.WIN_OBJECT_DEAD, this.gameObject);
		}
		yield return null;
    }
	
	void FixedUpdate(){
		Vector3 t = transform.position;
		t.y = GetTargetY();
		transform.position = t;
	}
}
