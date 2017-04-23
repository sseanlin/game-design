using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MonsterScript : MonoBehaviour {

	private Animator monsterAnimator;
	private float minHitPeriod = 1;
	private float hitCounter = 0;
	private float minShootPeriod = 5;
	private float shootCounter = 0;
	private bool isShooting = false;

	private Rigidbody rb;

	private AudioSource fireBallSound;

	public float HP = 100;

	public float moveSpeed;
	public GameObject target;
	public CollisionListScript playerSensor;
	public CollisionListScript attackSensor;

	public float shootChance;
	public GameObject fistPositionObject;
	public GameObject shockWaveCandidate;

	public GameUIManager uiManager;

	public void ShootingStart() {
		isShooting = true;
	}

	public void ShootingEnd() {
		isShooting = false;
		monsterAnimator.SetBool ("Shoot", false);
	}

	// Use this for initialization
	void Start () {
		monsterAnimator = gameObject.GetComponent<Animator> ();
		rb = gameObject.GetComponent<Rigidbody> ();
		fireBallSound = gameObject.GetComponent<AudioSource> ();
	}

	public void AttackPlayer() {
		if (attackSensor.collisionObjects.Count > 0) {
			attackSensor.collisionObjects [0].transform.GetChild (0).GetChild (0).SendMessage ("Hit", 20);
		}
	}

	public void ShootPlayer() {
		fireBallSound.Stop ();
		fireBallSound.Play ();
		GameObject shockWaveObject = GameObject.Instantiate (shockWaveCandidate);
		ShockWaveScript shockWave = shockWaveObject.GetComponent<ShockWaveScript> ();
		shockWave.transform.position = fistPositionObject.transform.position;

		shockWave.Shoot (fistPositionObject.transform.forward);
	}

	public void Hit(float damageValue) {
		if (hitCounter <= 0) {
			target = GameObject.FindGameObjectWithTag ("Player");
			hitCounter = minHitPeriod;
			HP -= damageValue;

			monsterAnimator.SetFloat ("HP", HP);
			monsterAnimator.SetTrigger ("Hit");

			if (HP <= 0) {
				BuryTheBody();
			}
		}
	}

	void BuryTheBody() {
		gameObject.GetComponent<Rigidbody> ().useGravity = false;
		gameObject.GetComponent<Collider> ().enabled = false;

		gameObject.transform.DOMoveY(-0.8f, 1f).SetRelative(true).SetDelay(1).OnComplete (
			()=> {
				gameObject.transform.DOMoveY(-0.8f, 1f).SetRelative(true).SetDelay(2).OnComplete (
					()=> {
						GameObject.Destroy(gameObject);
						uiManager.TrollDiedAnimation ();
					}
				);
			}
		);
	}
	
	// Update is called once per frame
	void Update () {
		if (playerSensor.collisionObjects.Count > 0) {
			target = playerSensor.collisionObjects [0].gameObject;
		}

		if (HP > 0) {
			if (shootCounter > 0) {
				shootCounter -= Time.deltaTime;
			}

			if (hitCounter > 0) {
				hitCounter -= Time.deltaTime;
			} else if (target != null) {
				Vector3 lookAt = target.transform.position;
				lookAt.y = gameObject.transform.position.y;
				gameObject.transform.LookAt (lookAt);
				monsterAnimator.SetBool ("Run", true);
					
				if (attackSensor.collisionObjects.Count > 0) {
					monsterAnimator.SetBool ("Attack", true);
					gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				} else {
					monsterAnimator.SetBool ("Attack", false);

					if (Random.value < shootChance && shootCounter <= 0) {
						shootCounter = minShootPeriod;
						monsterAnimator.SetBool ("Shoot", true);
					}

					if (isShooting) {
						gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
					} else {
						rb.velocity = moveSpeed * transform.forward;
					}
				}
			}
		} else {
			gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;	
		}
	}
}
