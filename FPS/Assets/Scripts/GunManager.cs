using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunManager : MonoBehaviour {

	public float minShootPeriod;
	public float muzzleShowPeriod;

	public GameObject muzzleFlash;
	public GameObject bulletCandidate;
	public GameObject flameBulletCandidate;

	private float shootCounter = 0;
	private float muzzleCounter = 0;

	private AudioSource gunShotSound;
	private AudioSource flameSound;

	void Start() {
		gunShotSound = gameObject.GetComponents<AudioSource> () [1];
		flameSound = gameObject.GetComponents<AudioSource> () [0];
	}

	public void Trigger() {
		if (shootCounter <= 0) {
			gunShotSound.Stop ();
			gunShotSound.pitch = Random.Range (0.8f, 1);
			gunShotSound.Play ();

			gameObject.transform.DOShakeRotation (minShootPeriod * 0.8f, 3f);

			muzzleCounter = muzzleShowPeriod;
			muzzleFlash.transform.localEulerAngles = new Vector3 (0, 0, Random.Range (0, 360));

			shootCounter = minShootPeriod;
			GameObject bulletObject = GameObject.Instantiate (bulletCandidate);
			BulletScript bullet = bulletObject.GetComponent<BulletScript> ();
			bullet.transform.position = muzzleFlash.transform.position;
			bullet.transform.rotation = muzzleFlash.transform.rotation;

			bullet.Shoot (muzzleFlash.transform.forward);
		}
	}

	public void FlameTrigger() {
		flameSound.Stop ();
		flameSound.Play ();
		gameObject.transform.GetChild (0).gameObject.SetActive (true);	
	}

	public void FlameStop() {
		flameSound.Stop ();
		gameObject.transform.GetChild (0).gameObject.SetActive (false);	
	}
		
	// Update is called once per frame
	void Update () {
		if (shootCounter > 0) {
			shootCounter -= Time.deltaTime;
		}

		if (muzzleCounter > 0) {
			muzzleFlash.gameObject.SetActive (true);
			muzzleCounter -= Time.deltaTime;
		} else {
			muzzleFlash.gameObject.SetActive (false);
		}

	}
}
