using UnityEngine;
using System.Collections;

public class WaterClick : MonoBehaviour {
	public const float COOLDOWN_DURATION = 0.2f;
	protected int waterMask;
	float cooldown;
	
	void Start () {
		cooldown = 0f;
		waterMask = LayerMask.NameToLayer("Water");
	}
	
	void FixedUpdate () {
		if(Input.GetMouseButtonDown(0)){
			if (Time.timeSinceLevelLoad - cooldown > COOLDOWN_DURATION){
				cooldown = Time.timeSinceLevelLoad;
				/*
				WaterRaycastHit hit;
				if (WaterRaycast.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
					GameObject buoy = GameObject.FindGameObjectWithTag("Buoy");
					buoy.BroadcastMessage("ApplyForce", hit.point-hit.transform.position);
				}else{
					D.Warn<string>("RAYCAST WAS FALSE");
					D.Warn<bool>(WaterRaycast.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit));
					Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
					
				}
				*/
				DoClick(Camera.main.ScreenPointToRay(Input.mousePosition));
			}else{
				D.Warn<string>("BLOCKED BY COOLDOWN");	
			}
		}
	}
	
	protected void DoClick (Ray r)
	{
		WaterRaycastHit hit;
		if (WaterRaycast.Raycast(r, out hit)){
			BroadcastForce( hit.point-hit.transform.position);
		}else{
			D.Log<string>("miss?");
			RaycastHit[] hits = Physics.RaycastAll(r);
			for (int i = 0; i < hits.Length; i++){
				if (hits[i].transform.gameObject.layer == 4){
					D.Log<string>("BROADCAST POINT : " + hits[i].point);
					BroadcastForce(hits[i].point);
				}
			}
			Debug.DrawRay(r.origin, r.direction*25, Color.red, 0.1f);
		}
		
	}
	
	protected void BroadcastForce(Vector3 point){
		GameObject buoy = GameObject.FindGameObjectWithTag("Buoy");
		buoy.BroadcastMessage("ApplyForce", point);
	}
}
