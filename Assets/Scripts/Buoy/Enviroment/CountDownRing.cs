using UnityEngine;
using System.Collections;

public class CountDownRing : MonoBehaviour {

	void Start () {
		//SkinnedMeshRenderer mr = gameObject.GetComponent<SkinnedMeshRenderer>();
		//MeshFilter mr = gameObject.GetComponent<MeshFilter>();
		/*
		if (mr != null){
			Mesh mesh = mr.sharedMesh;
			Mesh newmesh = new Mesh();
			newmesh.vertices = mesh.vertices;
        	newmesh.triangles = mesh.triangles;
        	newmesh.uv = mesh.uv;
        	newmesh.normals = mesh.normals;
        	newmesh.colors = mesh.colors;
			newmesh.colors32 = mesh.colors32;
        	newmesh.tangents = mesh.tangents;
			newmesh.boneWeights = mesh.boneWeights;
			newmesh.bindposes = mesh.bindposes;
			
			mr.sharedMesh = null;
			mr.sharedMesh = newmesh;
		}
		*/
	}
	
	string debug_name(){
		Transform t = transform;
		string output = t.gameObject.name;
		while(t.parent != null){
			t = t.parent;
			output = t.gameObject.name + "." + output;
		}
		return output;
	}
	
	protected int itterations = 0;
	public void ShowTime(float seconds){
		D.Warn<string>(debug_name());
		D.Warn<string>(" PLAY FOR : " + seconds.ToString() + " seconds");
		//SkinnedMeshRenderer mr = gameObject.GetComponent<SkinnedMeshRenderer>();
		
		itterations = Mathf.FloorToInt(seconds * 10.0f);
		//StartCoroutine(Spin(mr.sharedMesh) );
		MeshFilter mr = gameObject.GetComponent<MeshFilter>();
		StartCoroutine(Spin(mr.mesh) );
	}
	
	private IEnumerator Spin (Mesh m) {
		float adjustment = 0.997f/(itterations * 1.0f);
		D.Warn<string>( debug_name() + " : ITTERATIONS : " + itterations.ToString());
		D.Warn<string>( debug_name() + " : ADJUSTMENT  : " + adjustment.ToString());
		float t1 = Time.time;
		for (int i = 0; i < itterations; i++){
			if ( i % 100 == 0){
				D.Warn<string>( debug_name() + " : " + i.ToString() + "/" + itterations.ToString());
			}
			Vector2[] new_uv = new Vector2[m.uv.Length];
			for(int c = 0; c < m.uv.Length; c++){
				new_uv[c] = new Vector2( Mathf.Min(m.uv[c].x+adjustment, 1.0f), m.uv[c].y);
				//new_uv[c] = new Vector2(1.0f, m.uv[c].y);
			}
			m.uv = new_uv;
			yield return new WaitForSeconds(0.1f);
		}
		D.Warn<string>("___________________ DONE ____________________ IN : " + (Time.time-t1).ToString());
		yield return null;
	}
	/*
	void FixedUpdate () {
		SkinnedMeshRenderer mr = gameObject.GetComponent<SkinnedMeshRenderer>();
		if (mr != null){
			
			Mesh m = mr.sharedMesh;
			
			Vector2[] new_uv = new Vector2[m.uv.Length];
			for(int i = 0; i < m.uv.Length; i++){
				
				new_uv[i] = m.uv[i] + new Vector2(0.005f, 0f);
				if (i == 0){
					D.Log<float>(new_uv[i].y);
				}
			}
			m.uv = new_uv;
			
		}
	}*/
}
