using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float flyingSpeed;
	public float lifeTime; 
	public float damageValue = 20;

	public void Shoot(Vector3 direction) { 
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.velocity = direction * flyingSpeed;
		Invoke ("CommitSuicide", lifeTime);
	}

	public void CommitSuicide() {
		GameObject.Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other) {
		other.gameObject.SendMessage ("Hit", damageValue, SendMessageOptions.DontRequireReceiver);
		CommitSuicide ();
	}
}
