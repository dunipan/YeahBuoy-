using UnityEngine;
 
[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class RulerMesh : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreateRulerMesh();
    }
 
    Mesh CreateRulerMesh()
    {
        Mesh mesh = new Mesh();
 
        Vector3[] vertices = new Vector3[]
        {
            new Vector3( 1f, 0,  1f),
            new Vector3( 1f, 0, 0),
            new Vector3(0, 0,  1f),
            new Vector3(0, 0, 0),
        };
 
        Vector2[] uv = new Vector2[]
        {
            new Vector2(1f, 1f),
            new Vector2(1f, 0),
            new Vector2(0, 1f),
            new Vector2(0, 0),
        };
 
        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3,
        };
 
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
		mesh.normals = vertices;
 
        return mesh;
    }
}