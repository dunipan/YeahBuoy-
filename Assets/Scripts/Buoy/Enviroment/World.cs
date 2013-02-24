using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	protected static GameObject _messenger;
	
	// Use this for initialization
	void Start () {
		//PUT OURSELVES AT 0,0,0
		gameObject.transform.position = Vector3.zero;
		
		//CREATE THE MESSENGER FOR BROADCASTING
		World._messenger = new GameObject();
		World._messenger.tag = "Messenger";
		World._messenger.AddComponent<Messenger>();
		World._messenger.transform.parent = gameObject.transform;
		
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
	
	public static GameObject messenger
	{
	    get { return _messenger; }
	}
	
}
