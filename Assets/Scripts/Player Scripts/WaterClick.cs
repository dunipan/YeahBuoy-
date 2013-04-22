using UnityEngine;
using System.Collections;

public class WaterClick : MonoBehaviour {
	public const float COOLDOWN_DURATION = 0.2f;
	int waterMask;
	float cooldown;
	
	void Start () {
		cooldown = 0f;
		waterMask = LayerMask.NameToLayer("Water");
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if (Time.timeSinceLevelLoad - cooldown > COOLDOWN_DURATION){
				cooldown = Time.timeSinceLevelLoad;
				WaterRaycastHit hit;
				if (WaterRaycast.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
					GameObject buoy = GameObject.FindGameObjectWithTag("Buoy");
					buoy.BroadcastMessage("ApplyForce", hit.point-hit.transform.position);
				}
			}else{
				D.Warn<string>("BLOCKED BY COOLDOWN");	
			}
		}
	}
}
