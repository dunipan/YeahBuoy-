using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
	
	public static World _world;
	public static List<GameObject> _left_to_hit;
	public Font popup_font;
	// Use this for initialization
	void Start () {
		World._world = this;
		//PUT OURSELVES AT 0,0,0
		gameObject.transform.position = Vector3.zero;
		
		//     N
		// W       E
		//     S
		
		GameObject west_wall = new GameObject();
		west_wall.name = "_west_wall";
		west_wall.transform.parent = gameObject.transform;
		west_wall.transform.position = new Vector3(-4,0,0);
		
		BoxCollider west_wall_box_collider = west_wall.AddComponent<BoxCollider>();
		west_wall_box_collider.size = new Vector3(10,100,100);
		
		
		GameObject east_wall = new GameObject();
		east_wall.name = "_east_wall";
		east_wall.transform.parent = gameObject.transform;
		east_wall.transform.position = new Vector3(30,0,0);
		
		BoxCollider east_wall_box_collider = east_wall.AddComponent<BoxCollider>();
		east_wall_box_collider.size = new Vector3(10,100,100);
		
		GameObject south_wall = new GameObject();
		south_wall.name = "_south_wall";
		south_wall.transform.parent = gameObject.transform;
		south_wall.transform.position = new Vector3(0,0,-5);
		
		BoxCollider south_wall_box_collider = south_wall.AddComponent<BoxCollider>();
		south_wall_box_collider.size = new Vector3(100,100,10);
		
		GameObject north_wall = new GameObject();
		north_wall.name = "_north_wall";
		north_wall.transform.parent = gameObject.transform;
		north_wall.transform.position = new Vector3(0,0,30);
		
		BoxCollider north_wall_box_collider = north_wall.AddComponent<BoxCollider>();
		north_wall_box_collider.size = new Vector3(100,100,10);
		
		GameObject floor_wall = new GameObject();
		floor_wall.name = "_floor_wall";
		floor_wall.transform.parent = gameObject.transform;
		floor_wall.transform.position = new Vector3(15,-11,15);
		
		BoxCollider floor_wall_box_collider = floor_wall.AddComponent<BoxCollider>();
		floor_wall_box_collider.size = new Vector3(100,20,100);
		
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
