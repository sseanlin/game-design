using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine;
using DG.Tweening;

public class GestureManager : MonoBehaviour {

	public TapGesture singleTap;
	public TapGesture doubleTap;
	public ScreenTransformGesture transformGesture;
	public FlickGesture flickGesture;

	public Animator animator;

	public Camera mainCamera;

	public LayerMask modelLayer;

	public GameObject[] characters;

	private int shakeDetectCnt = 20;
	private int shakeCnt = 0;
	private float avgAcceleration = 0f;
	private float shakeThreshold = 1.5f;
	private float actionTime = 0f;

	private int currentChar = 0;
	private int nextChar = 1;

	private bool flickable = true;

	private Vector3 characterPosition;

	// Use this for initialization
	void Start () {

		characterPosition = characters [0].transform.position;

		singleTap.Tapped += (object sender, System.EventArgs e) => {
			Debug.Log("T");
			TouchHit hit;
			singleTap.GetTargetHitResult(out hit);

			Ray cameraToHit = new Ray(mainCamera.transform.position, hit.Point - mainCamera.transform.position);

			if(Physics.Raycast(cameraToHit, float.MaxValue, modelLayer)){
				animator.SetTrigger("Pose1");
			}
		};

		doubleTap.Tapped += (object sender, System.EventArgs e) => {
			Debug.Log("TT");
			TouchHit hit;
			singleTap.GetTargetHitResult(out hit);

			Ray cameraToHit = new Ray(mainCamera.transform.position, hit.Point - mainCamera.transform.position);

			if(Physics.Raycast(cameraToHit, float.MaxValue, modelLayer)){
				animator.SetTrigger("Pose2");
			}
		};

		transformGesture.Transformed += (object sender, System.EventArgs e) => {
			gameObject.transform.Rotate(Vector3.up, transformGesture.DeltaPosition.x);
			if(transformGesture.ActiveTouches.Count == 2) {
				Vector3 centroid = transformGesture.ActiveTouches[0].Hit.Point + transformGesture.ActiveTouches[1].Hit.Point;
				Vector3 zoomDirection = centroid - mainCamera.transform.position;
				zoomDirection.Normalize();
				if(0f < transformGesture.DeltaScale && transformGesture.DeltaScale < 1f) {
					mainCamera.transform.position += (0.05f * zoomDirection);
				}
				if(transformGesture.DeltaScale > 1) {
					mainCamera.transform.position -= (0.05f * zoomDirection);
				}
			}
		};

		flickGesture.Flicked += (object sender, System.EventArgs e) => {
			//Debug.Log("#0 " + characters[0].transform.position);
			//Debug.Log("#1 " + characters[1].transform.position);

			//nextChar = (currentChar + 1) % 2;
			if (flickable) {
				flickable = false;
				FlickGesture gesture = sender as FlickGesture;

				if (gesture.ScreenFlickVector.x < 0) {
					characters [currentChar].transform.position = characterPosition;
					characters [currentChar].transform.DOMoveX (characterPosition.x - 4f, 0.2f).SetEase(Ease.InOutSine);

					characters [nextChar].transform.position = characterPosition + new Vector3(4f, 0f, 0f);
					characters [nextChar].SetActive (true);
					characters [nextChar].transform.DOMoveX(characterPosition.x, 0.2f).SetEase(Ease.InOutSine).OnComplete(AfterFlick);
				} else {
					characters [currentChar].transform.position = characterPosition;
					characters [currentChar].transform.DOMoveX (characterPosition.x + 4f, 0.2f).SetEase(Ease.InOutSine);
						
					characters [nextChar].transform.position = characterPosition + new Vector3(-4f, 0f, 0f);
					characters [nextChar].SetActive (true);
					characters [nextChar].transform.DOMoveX(characterPosition.x, 0.2f).SetEase(Ease.InOutSine).OnComplete(AfterFlick);
				}
			}
		};
	}

	void AfterFlick() {
		//characters [currentChar].SetActive (false);
		characters[currentChar].GetComponent<GestureManager>().enabled = false;
		characters[nextChar].GetComponent<GestureManager>().enabled = true;
		currentChar = (currentChar + 1) % 2;
		nextChar = (currentChar + 1) % 2;
		animator = characters [currentChar].GetComponent<Animator> ();
		flickable = true;
	}
	
	// Update is called once per frame
	void Update () {
		shakeCnt++;
		Vector3 acceleration = Input.acceleration;

		if (actionTime <= 0) {
			if (shakeCnt % shakeDetectCnt == 0) {
				avgAcceleration /= shakeDetectCnt;
				if (avgAcceleration > shakeThreshold) {
					animator.SetTrigger ("Pose3");
					actionTime = 3.5f;
				}
				avgAcceleration = 0;
			} else {
				avgAcceleration += acceleration.magnitude;
			}
		} else {
			actionTime -= Time.deltaTime;
		}
	}
}
