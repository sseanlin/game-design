using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class BirdController : MonoBehaviour {

	public double deadSpeed;

	private Animator animator;
	private AudioSource collisionSound;
	private AudioSource deadSound;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		animator.SetBool ("dead", false);

		collisionSound = gameObject.GetComponents<AudioSource> ()[0];
		deadSound = gameObject.GetComponents<AudioSource> ()[1];
	}
	
	// Update is called once per frame
	void Update () {
		if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.ChangeScene")) {
			//Debug.Log ("CHANGE SCENE!");
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	void OnCollisionEnter2D(Collision2D collision2d) {
		if (!animator.GetBool ("dead")) {
			if (Vector3.Magnitude (collision2d.relativeVelocity) > deadSpeed) {
				animator.SetBool ("dead", true);
				deadSound.Play ();
			} else if (collision2d.gameObject.tag == "Asteroid") {
				collisionSound.Play ();
			}
		}
	}

	void Puff() {
		GetComponent<ParticleSystem> ().Play ();
	}
}
