using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionListScript : MonoBehaviour {

	public List<Collider> collisionObjects;

	void OnTriggerEnter(Collider other) {
		collisionObjects.Add (other);
	}

	void OnTriggerExit(Collider other) {
		collisionObjects.Remove (other);
	}
}
