using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject mainCamera;
	public Transform asteroidTransform;

	private Vector3 origin;


	// Use this for initialization
	void Start () {
		origin = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float x = asteroidTransform.position.x;
		if (GameManager.cameraBoundaries [0] < x && x < GameManager.cameraBoundaries [1]) {
			gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
		}
	}

	public void Reset(){
		mainCamera.transform.position = origin;
	}
}
