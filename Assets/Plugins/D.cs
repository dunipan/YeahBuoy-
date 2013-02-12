using UnityEngine;
using System.Collections;

public static class D {
	public static float min_level = 0f;
	
	private static void _log(string output, float level){
		if (level > D.min_level){
			Debug.Log("[" + Time.timeSinceLevelLoad.ToString() + "] " +output);
		}
	}
	
	public static void Log(string output, float level=10f){
		
	}
	
	
	public static void Log(GameObject output, float level=10f){
		
	}
}
