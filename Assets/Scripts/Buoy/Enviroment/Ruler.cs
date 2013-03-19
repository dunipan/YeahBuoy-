using UnityEngine;
using System.Collections;

public class Ruler : MonoBehaviour {
	void Start () {
		this.gameObject.transform.position = new Vector3(0,0.5f,0);
	}
	public void GenerateRuler(Material mat1, Material mat2){
		Transform me = this.gameObject.transform;
		int x = 0;
		while (x < 50){
			
			int z = 0;
			while (z < 50){
				if (x % 10.0 != 0.0 && z % 10.0 != 0.0){
					GameObject go = CreateUnit();
					MeshRenderer mr = go.GetComponent<MeshRenderer>();
					go.transform.parent = me;
					go.transform.position = new Vector3( x, 0, z);
					go.name = x.ToString() + "_" + z.ToString();
					
					if (x % 2.0 == 0.0){
						if (z % 2.0 == 0.0){
							mr.material = mat1;
						}else{
							mr.material = mat2;
						}
					}else{
						if (z % 2.0 == 0.0){
							mr.material = mat2;
						}else{
							mr.material = mat1;
						}
					}
				}
				z++;
			}
			x++;
		}
	}
	
	GameObject CreateUnit(){
		GameObject go = new GameObject();
		go.AddComponent<RulerMesh>();
		return go;
	}
}
