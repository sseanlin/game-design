using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveScript : MonoBehaviour {

	public float flyingSpeed;
	public float lifeTime; 
	public float damageValue = 30;
	public float rotationFrequency = 40;

	private Vector3 dir;

	public void Shoot(Vector3 direction) { 
		dir = direction;
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.velocity = direction * flyingSpeed;
		Invoke ("CommitSuicide", lifeTime);
	}

	public void CommitSuicide() {
		GameObject.Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other) {
		other.transform.GetChild(0).GetChild(0).SendMessage ("Hit", damageValue, SendMessageOptions.DontRequireReceiver);
		CommitSuicide ();
	}

	void Update() {
		gameObject.transform.Rotate (dir, Time.deltaTime * 360 * rotationFrequency);
	}
}
