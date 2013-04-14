using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	protected GameObject buoy;
	protected Transform buoy_transform;
	protected Rigidbody buoy_rigidbody;
	protected Transform parent_transform;
	protected Camera camera;
	
	protected float top = 900;
	protected float bottom = 300;
	protected float left = 300;
	protected float right = 300;
	
	protected bool moving = false;
	
	
	void Start () {
		buoy = GameObject.FindGameObjectWithTag("Buoy");
		buoy_transform = buoy.transform;
		buoy_rigidbody = buoy.rigidbody;
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
	
	void EvalOnScreenPosition(Vector3 onScreenPos,  Vector3 worldPosition ){		
		Rect screen_safe = _padding;
		if ( !screen_safe.Contains(onScreenPos) ){			
			Vector3 target = Vector3.Slerp( parent_transform.position, worldPosition, 0.5f * Time.deltaTime);
			if (onScreenPos.x > screen_safe.xMin && onScreenPos.x < screen_safe.xMax){
				target.x = parent_transform.position.x;
			}
			if (onScreenPos.y > screen_safe.yMin && onScreenPos.y < screen_safe.yMax){
				target.z = parent_transform.position.z;
			}
			parent_transform.position = target;
		}
	}
	
	void Update(){
		Vector3 camera_target = buoy_transform.position + buoy_rigidbody.velocity;
		Vector3 onScreenPos = camera.WorldToScreenPoint(camera_target);
		EvalOnScreenPosition(onScreenPos, camera_target);
		if (Input.GetAxis("Mouse ScrollWheel") != 0.0f){
			Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * 0.5f;
		}
	}
}
