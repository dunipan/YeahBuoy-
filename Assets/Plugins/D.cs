using UnityEngine;
using System.Collections;

public static class D {
	public static float min_level = 0f;
	
	private static void _log(string output, float level){
		if (level > D.min_level){
			Debug.Log("[" + Time.timeSinceLevelLoad.ToString() + "] " +output);
		}
	}
	private static void _warn(string output, float level){
		if (level > D.min_level){
			Debug.LogWarning("[" + Time.timeSinceLevelLoad.ToString() + "] " +output);
		}
	}
	
	public static void Log<T>(T output, float level=10f){
		if (output == null){
			D._warn("Value was null", level);
		}else{
			D._log(output.ToString(), level);
		}
	}
	
	
	public static void Warn<T>(T output, float level=10f){
		if (output == null){
			D._warn("Value was null", level);
		}else{
			D._warn(output.ToString(), level);
		}
	}
}
