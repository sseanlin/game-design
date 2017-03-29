using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

	public float dragMaxRadius;
	public float elasticForce;
	public LineRenderer frontLine;
	public LineRenderer backLine;

	public Transform frontA, frontB;
	public Transform backA, backB;

	public GameObject mainCamera;
	public GameObject bird;
	public Vector3 mainCameraOrigin;

	public CameraController mainCameraController;

	public int chance;

	public Text loseText;

	private Rigidbody2D rb;
	private Vector3 origin;
	private bool released;

	private float timer;
	private double velocityZeroThreshold;
	private double plankCollisionThreshold;

	private AudioSource shootSound;

	void OnMouseUp() {
		rb.AddForce((origin - gameObject.transform.position) * elasticForce);
		rb.gravityScale = 1;
		released = true;
		shootSound.Play ();
	}

	void OnMouseDrag() {
		Vector3 mousePoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10);
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (mousePoint);

		if (Vector3.Distance (mousePosition, origin) < dragMaxRadius) {
			gameObject.transform.position = mousePosition;
		} 
		else {
			gameObject.transform.position = origin + Vector3.ClampMagnitude(mousePosition - origin, dragMaxRadius);
		}

		frontLine.enabled = true;
		backLine.enabled = true;
	}
		
	void Start () {
		origin = gameObject.transform.position;
		rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.gravityScale = 0;
		released = false;

		frontLine.enabled = false;
		backLine.enabled = false;
		frontLine.sortingLayerName = "CatapultFront";
		frontLine.sortingOrder = 2;
		backLine.sortingLayerName = "CatapultBack";
		backLine.sortingOrder = 2;

		timer = 0;
		velocityZeroThreshold = 1E-6;
		plankCollisionThreshold = 0.5;

		shootSound = gameObject.GetComponent<AudioSource> ();

		loseText.enabled = false;
	}

	void Update () {
		if (released){
			Vector3 velocity = gameObject.GetComponent<Rigidbody2D> ().velocity;
			if (Vector3.Dot (velocity, frontA.position - frontB.position) < 0) {
				frontLine.enabled = false;
				backLine.enabled = false;
			}
			timer += Time.deltaTime;
			if (timer > 1) {
				float x = gameObject.transform.position.x;
				if (Vector3.Magnitude (bird.GetComponent<Rigidbody2D> ().velocity) < velocityZeroThreshold &&
					(Vector3.Magnitude (rb.velocity) < velocityZeroThreshold ||
						x < GameManager.sceneBoundaries [0] || x > (GameManager.sceneBoundaries [1]))) {
					chance = (chance == 0) ? 0 : (chance - 1);
					if (!bird.GetComponent<Animator>().GetBool("dead")) {
						if (chance == 0) { 
							loseText.enabled = true;
						} else {
							Reset ();
							mainCameraController.Reset ();
							Start ();
						}
					}
				}
			}
		}
		if (frontLine.enabled && backLine.enabled) {
			frontLine.SetPosition (0, frontA.position);
			frontLine.SetPosition (1, frontB.position);
			backLine.SetPosition (0, backA.position);
			backLine.SetPosition (1, backB.position);
		}
	}

	void Reset() {
		gameObject.transform.position = origin;
		gameObject.transform.eulerAngles = Vector3.zero;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0;
	}

	void OnCollisionEnter2D(Collision2D collision2d) {
		if (collision2d.gameObject.tag == "Plank" && Vector3.Magnitude(collision2d.relativeVelocity) > plankCollisionThreshold) {
			collision2d.gameObject.GetComponent<AudioSource> ().Play ();
		}
	}
}