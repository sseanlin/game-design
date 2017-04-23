using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletScript : MonoBehaviour {

	private float minShootPeriod = 0.2f;
	private float shootCounter = 0;

	public float damageValue = 25;

	void OnTriggerStay(Collider other) {
		if (shootCounter <= 0) {
			shootCounter = minShootPeriod;
			other.gameObject.SendMessage ("Hit", damageValue, SendMessageOptions.DontRequireReceiver);
		} else {
			shootCounter -= Time.deltaTime;
		}
	}
}
