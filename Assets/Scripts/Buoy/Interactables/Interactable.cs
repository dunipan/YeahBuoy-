using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
	
	//COOLDOWN SO THE SAME OBJECTS COLLIDING DON'T KEEP SPAMMING
	public float hitCooldownInSeconds = 3.0f;
	
	public List<Rigidbody> recentHits;
	
	private Vector3 lookAtTarget;
	private bool lookAtEnabled;
	private Quaternion lookAtRotationStart;
	private float lookAtRotationPercent;
	
	protected void Start () {
		recentHits = new List<Rigidbody>();
		lookAtEnabled = false;
	}
	
	protected virtual void DoInteraction (Rigidbody rb) { }
	
	protected void OnCollisionEnter(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		Rigidbody rb = GameObjectExtensions.FirstAncestorOfType<Rigidbody>(contact.otherCollider.gameObject);
		
		if (rb){
			if (hitCooldownInSeconds > 0f){
				if (recentHits.IndexOf(rb) == -1){
					recentHits.Add(rb);
					DoInteraction(rb);
					StartCoroutine("DropGameObject", rb);
				}
			}else{
				DoInteraction(rb);
			}
		}
	}
	
	public IEnumerator DropGameObject(Rigidbody rb) {
		yield return new WaitForSeconds(hitCooldownInSeconds);
		recentHits.Remove(rb);
	}
	
	public void LookAt(Vector3 target) { 
		lookAtTarget = target;
		lookAtTarget.y = 0;
		lookAtEnabled = true;
		RestartLookProcess();
	}

	protected void RestartLookProcess() {
		if( lookAtEnabled ) { 
    		lookAtRotationStart = transform.rotation;
    		lookAtRotationPercent = 0f;
		}else {
     		lookAtRotationPercent = 1f; 
		}
	}
	
	protected void LateUpdate() {
		if( lookAtEnabled ){
			Quaternion targetAngle = Quaternion.LookRotation( lookAtTarget - transform.position, transform.up );
			if( lookAtRotationPercent < 1f ) {
				lookAtRotationPercent += Time.deltaTime * 4;
				if( lookAtRotationPercent < 1f ){
					targetAngle = Quaternion.Slerp(lookAtRotationStart, targetAngle, lookAtRotationPercent);
				}
			}
			if (Quaternion.Dot(targetAngle,transform.rotation) < 0.999f){
				transform.rotation = targetAngle;
			}
			
			transform.rotation = targetAngle;
		}
	}
}
