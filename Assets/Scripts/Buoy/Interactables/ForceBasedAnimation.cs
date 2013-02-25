using UnityEngine;
using System.Collections;

public class ForceBasedAnimation : MonoBehaviour {
	public const string FORCE_BASED_ANIMATION_CHANGE = "forceBasedAnimationChange";
	
	public const float MEDIUM_FORCE = 5f;
	public const float LARGE_FORCE = 8f;
	
	public bool small_hit = true;
	public bool medium_hit = true;
	public bool large_hit = true;
	public bool moving = false;
	
	private Animation _cached_animation;
	private 
	
	void Start(){
		_cached_animation = gameObject.GetComponent<Animation>();
	}
	
	private void _play_animation(bool play){
		if (play){
			if(!_cached_animation.isPlaying){
				_cached_animation.Play();
			}
		}else{
			if(_cached_animation.isPlaying){
				_cached_animation.Stop();
			}
		}
	}
	
	bool childOf(GameObject target){
		Transform p = transform.parent;
		while( p != null){
			if (p.gameObject == target){
				return true;
			}
			p = p.parent;
		}
		return false;
	}
	
	void onNewForcePlay(GameObject obj, float force ){
		if (childOf(obj)){
			if (force > LARGE_FORCE){
				_play_animation(large_hit);
			}else if(force > MEDIUM_FORCE){
				_play_animation(medium_hit);
			}else if(force > 0.0f){
				_play_animation(small_hit);
			}else{
				_play_animation(moving);
			}
		}
	}
	
	void OnEnable()	{
		Messenger<GameObject, float>.AddListener(ForceBasedAnimation.FORCE_BASED_ANIMATION_CHANGE, onNewForcePlay);
	}
	
	void OnDisable(){
		Messenger<GameObject, float>.RemoveListener(ForceBasedAnimation.FORCE_BASED_ANIMATION_CHANGE, onNewForcePlay);
	}
}
