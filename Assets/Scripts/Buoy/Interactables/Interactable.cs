using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
	
	//COOLDOWN SO THE SAME OBJECTS COLLIDING DON'T KEEP SPAMMING
	public float hitCooldownInSeconds = 3.0f;
	
	public List<Rigidbody> recentHits;
	
	protected void Start () {
		recentHits = new List<Rigidbody>();
	}
	
	void Update () { }
	
	protected virtual void DoInteraction (Rigidbody rb) { }
	
	void OnCollisionEnter(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		Debug.LogWarning(contact);
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
}
