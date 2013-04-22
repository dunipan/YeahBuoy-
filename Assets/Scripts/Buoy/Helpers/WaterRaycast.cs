/* *****************************************************************************
 * 
 *								EDUCATION RESEARCH GROUP
 *							MORGRIDGE INSTITUTE FOR RESEARCH
 * 			
 * 				
 * Copyright (c) 2012 EDUCATION RESEARCH, MORGRIDGE INSTITUTE FOR RESEARCH
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated  * documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 *  
 * 
 ******************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterRaycastHit{
	
	public float distance;
	public Transform transform;
	public Vector2 barycentricCoordinate;
	public Vector2 textureCoord;
	public Vector3 point;//LOCAL POINT!
	
	public WaterRaycastHit(){
		this.distance = 0f;
		this.transform = null;
		this.textureCoord = Vector2.zero;
		this.barycentricCoordinate = Vector2.zero;
		this.point = Vector3.zero;
	}
	
	public WaterRaycastHit(Transform transform, float distance, Vector2 barycentricCoordinate){
		this.distance = distance;
		this.transform = transform;
		this.barycentricCoordinate = barycentricCoordinate;
		this.textureCoord = Vector2.zero;
		this.point = Vector3.zero;
	}
}

public class WaterRaycast : MonoBehaviour {
	
	static Vector3 edge1 = new Vector3();
	static Vector3 edge2 = new Vector3();
	static Vector3 tVec = new Vector3();
	static Vector3 pVec = new Vector3();
	static Vector3 qVec = new Vector3();
	
	static float det = 0;
	static float invDet = 0;
	static float u = 0;
	static float v = 0;
	
	static float epsilon = 0.0001f;
	
	static System.Diagnostics.Stopwatch stopWatch;
	

	public static bool Raycast (Ray ray, out WaterRaycastHit hit)
	{	
		hit = new WaterRaycastHit();
		List<WaterRaycastHit> hits = new List<WaterRaycastHit>();
	
		hits = INTERNAL_RaycastAll(ray);
		
		hits = SortResults(hits);
		if (hits.Count > 0){
			hit = hits[0];
			return true;
		}
		return false;
	}
	
	public static WaterRaycastHit[] RaycastAll(Ray ray)
	{
		return INTERNAL_RaycastAll(ray).ToArray();
	}
	
	public static WaterRaycastHit[] RaycastAll(Ray ray, float dist, LayerMask mask){
		List<WaterRaycastHit> hits = INTERNAL_RaycastAll(ray);
		for (int i = 0; i < hits.Count; i++){
			if (hits[i].distance > dist) hits.RemoveAt(i);
			if ((1 << hits[i].transform.gameObject.layer & mask.value) != 1 << hits[i].transform.gameObject.layer){
				hits.RemoveAt(i);
			}
		}
		return hits.ToArray();
	}
	
	static List<WaterRaycastHit> INTERNAL_RaycastAll(Ray ray)
	{
		
		stopWatch = new System.Diagnostics.Stopwatch();
		stopWatch.Start();
		List<WaterRaycastHit> hits = new List<WaterRaycastHit>();
		APAOctree octree = WaterRaycastDict.GetOctree();
		
		if (octree.bounds.IntersectRay(ray)){
			hits = RecurseOctreeBounds(octree, ray);	
		}
		
		hits = SortResults(hits);
		stopWatch.Stop();
		return hits;
	}
	
	static bool INTERNAL_Raycast (Ray ray, out WaterRaycastHit hit)
	{	
		hit = new WaterRaycastHit();
		List<WaterRaycastHit> hits = new List<WaterRaycastHit>();
	
		APAOctree octree = WaterRaycastDict.GetOctree();
		
		if (octree.bounds.IntersectRay(ray)){
			hits = RecurseOctreeBounds(octree, ray);	
		}
		
		hits = SortResults(hits);
		if (hits.Count > 0){
			hit = hits[0];	
		}
		return hits.Count > 0;
	}
	
	static List<WaterRaycastHit> RecurseOctreeBounds(APAOctree octree, Ray ray){
		List<WaterRaycastHit> hits = new List<WaterRaycastHit>();
		float dist = 0f;
		Vector2 baryCoord = new Vector2();
		for (int i = 0; i < octree.m_children.Count; i++){
			if (octree.m_children[i].bounds.IntersectRay(ray)){
				for (int k = 0; k < octree.m_children[i].triangles.Count; k++){
					if (TestIntersection(octree.m_children[i].triangles[k], ray, out dist, out baryCoord)){
						hits.Add(BuildRaycastHit(octree.m_children[i].triangles[k], dist, baryCoord));
					}
				}
				hits.AddRange(RecurseOctreeBounds(octree.m_children[i], ray));	
			}
		}
		return hits;
	}
	
	static WaterRaycastHit BuildRaycastHit(Triangle hitTriangle, float distance, Vector2 barycentricCoordinate){
		WaterRaycastHit returnedHit = new WaterRaycastHit(hitTriangle.trans, distance, barycentricCoordinate);
		
		returnedHit.textureCoord = hitTriangle.uv_pt0 + ((hitTriangle.uv_pt1 - hitTriangle.uv_pt0) * barycentricCoordinate.x) + ((hitTriangle.uv_pt2 - hitTriangle.uv_pt0) * barycentricCoordinate.y);
//HACK:  This only returns the center of the hit triangle.  A close approximate, but not accurate.  
		returnedHit.point = hitTriangle.trans.position + (hitTriangle.pt0 + hitTriangle.pt1 + hitTriangle.pt2) / 3;
		return returnedHit;
		
	}
	
	/// <summary>
	/// Tests the intersection.
	/// Implementation of the Moller/Trumbore intersection algorithm 
	/// </summary>
	/// <returns>
	/// Bool if the ray does intersect
	/// out dist - the distance along the ray at the intersection point
	/// out hitPoint - 
	/// </returns>
	/// <param name='triangle'>
	/// If set to <c>true</c> triangle.
	/// </param>
	/// <param name='ray'>
	/// If set to <c>true</c> ray.
	/// </param>
	/// <param name='dist'>
	/// If set to <c>true</c> dist.
	/// </param>
	/// <param name='baryCoord'>
	/// If set to <c>true</c> barycentric coordinate of the intersection point.
	/// </param>
	/// http://www.cs.virginia.edu/~gfx/Courses/2003/ImageSynthesis/papers/Acceleration/Fast%20MinimumStorage%20RayTriangle%20Intersection.pdf
	static bool TestIntersection(Triangle triangle, Ray ray, out float dist, out Vector2 baryCoord){
		baryCoord = Vector2.zero;
		dist = Mathf.Infinity;
		edge1 = triangle.pt1 - triangle.pt0;
		edge2 = triangle.pt2 - triangle.pt0;
		
		pVec = Vector3.Cross (ray.direction, edge2);
		det = Vector3.Dot ( edge1, pVec);
		if (det < epsilon) {
			return false;	
		}
		tVec = ray.origin - triangle.pt0;
		u = Vector3.Dot (tVec, pVec);
		if (u < 0 || u > det) {
			return false;	
		}
		qVec = Vector3.Cross (tVec, edge1);
		v = Vector3.Dot (ray.direction, qVec);
		if (v < 0 || u + v > det) {
			return false;	
		}
		dist = Vector3.Dot(edge2, qVec);
		invDet = 1 / det;
		dist *= invDet;
		baryCoord.x = u * invDet;
		baryCoord.y = v * invDet;
		return true;
	}
	
	static List<WaterRaycastHit> SortResults(List<WaterRaycastHit> input){
		
		WaterRaycastHit a = new WaterRaycastHit();
		WaterRaycastHit b = new WaterRaycastHit();
		bool swapped = true;
		while (swapped){
			swapped = false;
			for(int i = 1; i < input.Count; i++){
				if (input[i-1].distance > input[i].distance){
					a = input[i-1];
					b = input[i];
					input[i-1] = b;
					input[i] = a;
					swapped = true;
				}
			}
		}
		
		return input;
	}
	

}
