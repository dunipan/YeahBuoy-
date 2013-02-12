using UnityEngine;
using System.Collections;

public class WaterClick : MonoBehaviour {
	public const float COOLDOWN_DURATION = 0.4f;
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
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hits;
		        hits = Physics.RaycastAll(ray);
		        int i = 0;
		        while (i < hits.Length) {
		            RaycastHit hit = hits[i];
		            GameObject gameObject = hit.transform.gameObject;
					if (gameObject.layer == waterMask){
						GameObject buoy = GameObject.FindGameObjectWithTag("Buoy");
						buoy.BroadcastMessage("ApplyForce",hit.point);
						break;
					}
		            i++;
		        }
			}
		}
	}
}
