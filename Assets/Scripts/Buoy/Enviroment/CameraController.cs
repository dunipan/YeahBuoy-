using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	protected GameObject buoy;
	protected Transform buoy_transform;
	protected Transform parent_transform;
	protected Camera camera;
	
	protected float top = 900;
	protected float bottom = 300;
	protected float left = 300;
	protected float right = 300;
	
	void Start () {
		buoy = GameObject.FindGameObjectWithTag("Buoy");
		buoy_transform = buoy.transform;
		parent_transform = gameObject.transform.parent;
		camera = Camera.mainCamera;
		camera.gameObject.transform.localPosition = new Vector3(0, 10, camera.orthographicSize*-1);
	}
	
	public Rect _padding
	{
	    get {
			return new Rect(left/camera.orthographicSize, bottom/camera.orthographicSize, Screen.width - (left+right)/camera.orthographicSize, Screen.height - (top+bottom)/camera.orthographicSize);
		}
	}
	
	void Update () {
		Vector3 onScreen = camera.WorldToScreenPoint(buoy_transform.position);
		Rect r = _padding;
		if ( !r.Contains(onScreen) ){
			Vector3 target = parent_transform.position;
			target = Vector3.Slerp( parent_transform.position, buoy_transform.position, 0.5f * Time.deltaTime);
			if (onScreen.x > r.xMin && onScreen.x < r.xMax){
				target.x = parent_transform.position.x;
			}
			if (onScreen.y > r.yMin && onScreen.y < r.yMax){
				target.z = parent_transform.position.z;
			}
			parent_transform.position = target;
		}
	}
}
