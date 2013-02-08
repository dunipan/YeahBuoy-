using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenaltyObject : MonoBehaviour {
	
	List<Collider> hits;
	
	void Start () {
		hits = new List<Collider>();
	}
	
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		Debug.Log(collision);
		if (hits.IndexOf(contact.otherCollider) == -1){
			hits.Add(contact.otherCollider);
			Destroy(contact.otherCollider);
		}
	}
}
