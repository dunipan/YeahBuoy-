using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterRaycastDict : MonoBehaviour {
	
	public APAOctree octree;
	public static WaterRaycastDict singleton;
	private int octreeDepth = 3;
	private int objectsPerChunk = 1000;
	public delegate void CallbackMethod();
	
	protected void Awake(){
		singleton = this;
	}
	
	protected void OnDestroy(){
		Debug.Log("Mem Before Clear: " + System.GC.GetTotalMemory(true) / 1024f / 1024f);
		octree.Clear();
		octree = null;
		Destroy(singleton);
		Debug.Log("Mem After Clear: " + System.GC.GetTotalMemory(true) / 1024f / 1024f);
	}
		
	public void Init (CallbackMethod del, Bounds bounds)
	{
		if (octree != null){
			octree.Clear();
		}
		octree = new APAOctree(bounds, octreeDepth);
		StartCoroutine(PopulateOctree (del));		
	}
	

	protected IEnumerator PopulateOctree (CallbackMethod del)
	{
		Water3[] water_behaviours = FindObjectsOfType(typeof(Water3)) as Water3[];
		
		GameObject curGO; 
		Triangle[] curTris = new Triangle[] {};
		MeshFilter curMeshFilter = null;
		APAOctree finalNode;
		for (int i = 0; i < water_behaviours.Length; i++) {
			curGO = water_behaviours[i].gameObject;
			if (curGO == null ) continue;
			
			curMeshFilter = curGO.GetComponent<MeshFilter>();
			if (!curMeshFilter) continue;
			curTris = new Triangle[] {};
			curTris = GetTriangles(curGO);
			for (int k = 0; k < curTris.Length; k++){
				finalNode = octree.IndexTriangle(curTris[k]);
				finalNode.AddTriangle(curTris[k]);	
			}
			
			if (i % objectsPerChunk == 1){
				yield return 0;
			}
		}
		
		del();
		//Debug.Log("Created Database");
		//Debug.Log("Total Indexed Triangles: " + GetTriangleCount(octree));
	
	}
	
	
	protected int GetTriangleCount(APAOctree o){
		int count = 0;
		count = o.triangles.Count;
		foreach(APAOctree oct in o.m_children){
			count += GetTriangleCount(oct) ;
		}
		return count;
	}

	protected Triangle[] GetTriangles(GameObject go){
		Mesh mesh = go.GetComponent<MeshFilter>().sharedMesh;
		int[] vIndex = mesh.triangles;
		Vector3[] verts = mesh.vertices;
		Vector2[] uvs = mesh.uv;
		List<Triangle> triangleList = new List<Triangle>();
		int i = 0;
		while (i < vIndex.Length){
			triangleList.Add(
				new Triangle(
				verts[vIndex[i + 0]], 
				verts[vIndex[i + 1]], 
				verts[vIndex[i + 2]], 
				uvs[vIndex[i + 0]],
				uvs[vIndex[i + 1]],
				uvs[vIndex[i + 2]],
				go.transform));
			i += 3;
		}
		return triangleList.ToArray();
	}
	/*	
	protected void OnDrawGizmos(){
		DrawOctree(octree);
	}
	//*/
	protected void DrawOctree(APAOctree oct){
		Gizmos.DrawWireCube(oct.bounds.center, oct.bounds.size);
		
		foreach(APAOctree o in oct.m_children){
			DrawOctree(o);	
		}
	}
	
	public static APAOctree GetOctree(){
		return singleton.octree;
	}
}
