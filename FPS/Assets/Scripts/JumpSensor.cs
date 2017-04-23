using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSensor : MonoBehaviour {

	private int touchedCount = 0;

	void OnTriggerEnter(Collider other) {
		touchedCount++;
	}

	void OnTriggerExit(Collider other) {
		touchedCount--;
	}

	public bool CanJump() {
		return touchedCount > 0;
	}
}
