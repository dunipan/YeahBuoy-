using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	
	public static World _world;
	public static List<GameObject> _left_to_hit;
	public Font popup_font;
	
	public float width = 25.5f;
	public float height = 25f;
	protected float _padding_up = 10f;
	protected float _padding_depth = 100f;
	protected float _padding_overflow = 10f;
	protected float _inner_padding = 0.25f;
	
	public bool show_ruler = false;
	public Material ruler_mesh1;
	public Material ruler_mesh2;
	
	public Rect WorldRect;
	
	void Start () {
		World._world = this;
		//PUT OURSELVES AT 0,0,0
		gameObject.transform.position = Vector3.zero;
		
		//     N
		// W       E
		//     S
		
		//WEST WALL
		GameObject west_wall = new GameObject();
		west_wall.name = "_west_wall";
		west_wall.transform.parent = gameObject.transform;
		BoxCollider west_wall_box_collider = west_wall.AddComponent<BoxCollider>();
		west_wall_box_collider.size = new Vector3(_padding_depth, _padding_up, _padding_overflow+height);
		west_wall.transform.position = new Vector3(west_wall_box_collider.size.x*-0.5f+_inner_padding, 0, height/2);
		
		//EAST WALL
		GameObject east_wall = new GameObject();
		east_wall.name = "_east_wall";
		east_wall.transform.parent = gameObject.transform;
		BoxCollider east_wall_box_collider = east_wall.AddComponent<BoxCollider>();
		east_wall_box_collider.size = new Vector3(_padding_depth, _padding_up, _padding_overflow+height);
		east_wall.transform.position = new Vector3(east_wall_box_collider.size.x*0.5f+width-_inner_padding, 0, height/2);
		
		//SOUTH WALL
		GameObject south_wall = new GameObject();
		south_wall.name = "_south_wall";
		south_wall.transform.parent = gameObject.transform;
		BoxCollider south_wall_box_collider = south_wall.AddComponent<BoxCollider>();
		south_wall_box_collider.size = new Vector3(width+_padding_overflow,_padding_up, _padding_depth);
		south_wall.transform.position = new Vector3( width / 2, 0, south_wall_box_collider.size.z*-0.5f+_inner_padding);
		
		//NORTH WALL
		GameObject north_wall = new GameObject();
		north_wall.name = "_north_wall";
		north_wall.transform.parent = gameObject.transform;
		BoxCollider north_wall_box_collider = north_wall.AddComponent<BoxCollider>();
		north_wall_box_collider.size = new Vector3(width+_padding_overflow*2,_padding_up, _padding_depth);
		north_wall.transform.position = new Vector3( width / 2, 0, north_wall_box_collider.size.z*0.5f+height-_inner_padding);
		
		//FLOOR
		GameObject floor_wall = new GameObject();
		floor_wall.name = "_floor_wall";
		floor_wall.transform.parent = gameObject.transform;
		floor_wall.transform.position = new Vector3(width*0.5f,_padding_up*-0.5f-0.5f,height*0.5f);
		BoxCollider floor_wall_box_collider = floor_wall.AddComponent<BoxCollider>();
		floor_wall_box_collider.size = new Vector3(width+_padding_overflow,_padding_up,height+_padding_overflow);
		
		
		WorldRect = new Rect( _inner_padding, height -_inner_padding, width - _inner_padding*2, height- _inner_padding*2);
		
		//CAMERA CONTROL CLASS
		GameObject camera_container = new GameObject();
		camera_container.name = "__CAMERA";
				
		Vector3 camera_pos = Camera.mainCamera.gameObject.transform.position;
		camera_pos.y = 0;
		camera_container.transform.localPosition = camera_pos;
		camera_container.transform.parent = gameObject.transform;
		
		Camera.mainCamera.gameObject.transform.parent = camera_container.transform;
		Camera.mainCamera.gameObject.AddComponent<CameraController>();
		
		if (show_ruler && ruler_mesh1 != null && ruler_mesh2 != null){
			GameObject ruler = new GameObject();
			ruler.name = "RULER";
			ruler.transform.parent = this.gameObject.transform;
			Ruler r = ruler.AddComponent<Ruler>();
			r.GenerateRuler(ruler_mesh1, ruler_mesh2);
		}
		
	}
	
	public Vector3 CapPosition(Vector3 worldPos){
		Vector2 pos = new Vector2( worldPos.x, worldPos.z);
		if (!WorldRect.Contains( pos )){
			if (pos.x < WorldRect.xMin){
				worldPos.x = WorldRect.xMin;
			}else if(pos.x > WorldRect.xMax){
				worldPos.x = WorldRect.xMax;
			}
			
			if (pos.y < WorldRect.yMin){
				worldPos.z = WorldRect.yMin;
			}else if(pos.y > WorldRect.yMax){
				worldPos.z = WorldRect.yMax;
			}
		}
		return worldPos;
	}
	
	public static World current_world
	{
	    get { return World._world; }
	}
	
	public static List<GameObject> left_to_hit
	{
	    get { 
			if (World._left_to_hit == null){
				World._left_to_hit = new List<GameObject>();
			}
			return World._left_to_hit; 
		}
	}
	
	public static void winning_object_created(GameObject go){
		if (World.left_to_hit.IndexOf(go) == -1){
			World.left_to_hit.Add(go);
		}
	}
	
	public static void winning_object_hit(GameObject go){
		if (World.left_to_hit.IndexOf(go) > -1){
			World.left_to_hit.Remove(go);
		}
		if (World.left_to_hit.Count == 0){
			Messenger<Rigidbody>.Broadcast(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, go.rigidbody);
		}else{
			Messenger<Rigidbody>.Broadcast(Buoy.BUOY_HIT_WIN_OBJECT, go.rigidbody);	
		}
	}
	
	void onLevelFinish(Rigidbody rb){
		World.left_to_hit.Clear();
	}
	
	void OnEnable()	{
		Messenger<Rigidbody>.AddListener(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, onLevelFinish);
	}
	
	void OnDisable(){
		Messenger<Rigidbody>.RemoveListener(Buoy.BUOY_HIT_FINAL_WIN_OBJECT, onLevelFinish);
	}
}
