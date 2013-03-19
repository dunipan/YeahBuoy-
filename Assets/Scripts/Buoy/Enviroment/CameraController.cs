using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	protected GameObject buoy;
	protected Transform buoy_transform;
	protected Transform parent_transform;
	protected Camera camera;
	
	protected float padding = 360;
	
	void Start () {
		buoy = GameObject.FindGameObjectWithTag("Buoy");
		buoy_transform = buoy.transform;
		parent_transform = gameObject.transform.parent;
		camera = Camera.mainCamera;
		camera.gameObject.transform.localPosition = new Vector3(0, 10, camera.orthographicSize*-1);
	}
	
	public int _padding
	{
	    get { return Mathf.RoundToInt( padding / camera.orthographicSize ); }
	}
	
	void Update () {
		Vector3 onScreen = camera.WorldToScreenPoint(buoy_transform.position);
		if( onScreen.x < _padding || onScreen.x > Screen.width - _padding || onScreen.y < _padding || onScreen.y > Screen.height - _padding) {
			Vector3 target = parent_transform.position;
			target.x = buoy_transform.position.x;
			target.z = buoy_transform.position.z;
			parent_transform.position = Vector3.Slerp( parent_transform.position, target, 0.3f * Time.deltaTime);
		}
	}
}
