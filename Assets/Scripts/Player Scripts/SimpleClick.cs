using UnityEngine;
using System.Collections;

public class SimpleClick : MonoBehaviour {
	public const float COOLDOWN_DURATION = 0.2f;
	protected LayerMask waterMask;
	float cooldown;
	
	
	// Use this for initialization
	void Start () {
		cooldown = 0f;
		waterMask = LayerMask.NameToLayer("Water");
		D.Log<string>("Simple Click added");
		D.Log<int>(waterMask);
	}
	
	void FixedUpdate () {
		if(Input.GetMouseButtonDown(0)){
			if (Time.timeSinceLevelLoad - cooldown > COOLDOWN_DURATION){
				cooldown = Time.timeSinceLevelLoad;
				DoClick(Camera.main.ScreenPointToRay(Input.mousePosition));
			}else{
				D.Warn<string>("BLOCKED BY COOLDOWN");	
			}
		}
	}
	
	protected void DoClick (Ray r)
	{
		Debug.DrawRay(r.origin, r.direction * 100, Color.red, 10f);
		D.Log<string>("DoClick");
		D.Log<int>( waterMask );
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, Mathf.Infinity, 1 << waterMask)) {
			BroadcastForce(hit.point);
			D.Log<Vector3>(hit.point);
			D.Log<GameObject>(hit.collider.gameObject);
			D.Log<int>(hit.collider.gameObject.layer);
		}
	}
	
	protected void BroadcastForce(Vector3 point){
		GameObject buoy = GameObject.FindGameObjectWithTag("Buoy");
		buoy.BroadcastMessage("ApplyForce", point);
	}
}
