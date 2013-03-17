using UnityEngine;
using System.Collections;

public class OnScreenText : MonoBehaviour {
	
	public static Vector2 textMove = new Vector2(0, 35f);
	public string textValue;
	public Vector3 textPos;
	protected GUIText text;
	public float moved_value = 0;
	
	void Start () {
		text = gameObject.AddComponent<GUIText>();
		text.text = textValue;
		text.fontSize = 16;
		text.pixelOffset = textPos;
		text.font = World.current_world.popup_font;
	}
	
	void Update(){
		if( moved_value > 3f){
			Destroy(this.gameObject);
		}
	 	text.pixelOffset += (textMove * Time.deltaTime);
		moved_value += 1 * Time.deltaTime;
	}
}
