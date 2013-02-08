using UnityEngine;
using System.Collections;

public class WaterClick : MonoBehaviour {
	int waterMask;
	
	void Start () {
		waterMask = LayerMask.NameToLayer("Water");
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
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
