using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

	public Transform centerOfRotationX;
	public Transform centerOfRotationY;
	public float rotationSpeedX;
	public float rotationSpeedY;

	public float moveSpeed;

	public Rigidbody rb;

	public JumpSensor jumpSensor;
	public float jumpSpeed;
	public GunManager gun;

	public GameUIManager uiManager;
	public int HP;

	private Animator playerAnimator;
	private AudioSource walkSound;

	// Use this for initialization
	void Start () {
		playerAnimator = gameObject.GetComponent<Animator> ();
		walkSound = gameObject.GetComponent<AudioSource> ();
	}

	void Hit(int damageValue) {
		if (HP <= 0) {
			return;
		}
		HP -= damageValue;
		uiManager.SetHP (HP);

		if (HP > 0) {
			uiManager.PlayHitAnimation ();
		} else {
			uiManager.PlayerDiedAnimation ();

			rb.gameObject.GetComponent<Collider> ().enabled = false;
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
			this.enabled = false;
			centerOfRotationX.transform.DOLocalRotate (new Vector3 (-60, 0, 0), 0.5f);
			centerOfRotationY.transform.DOLocalMoveY (-1.5f, 0.5f).SetRelative (true);
		}
	}
	
	// Update is called once per frame
	void Update () { 
		Vector3 moveDirection = Vector3.zero;

		if (Input.GetKey (KeyCode.W)) { 
			moveDirection.z += 1; 
		} 
		if (Input.GetKey (KeyCode.S)) { 
			moveDirection.z -= 1; 
		}
		if (Input.GetKey (KeyCode.D)) { 
			moveDirection.x += 1; 
		}
		if (Input.GetKey (KeyCode.A)) { 
			moveDirection.x -= 1; 
		}

		if(Input.GetMouseButton(0)) {
			gun.Trigger ();
		} else if (Input.GetMouseButtonDown (1)) {
			gun.FlameTrigger ();
		} else if (Input.GetMouseButtonUp(1)){
			gun.FlameStop ();
		}

		moveDirection = moveDirection.normalized;

		float currentSpeed;

		if (moveDirection.magnitude == 0 || !jumpSensor.CanJump()) { 
			currentSpeed = 0;
			walkSound.Stop ();
		} 
		else {
			if (!walkSound.isPlaying) {
				walkSound.Play ();
			}
			if (moveDirection.z < 0) {
				currentSpeed = -moveSpeed;
			} else {
				currentSpeed = moveSpeed;
			}
		}

		playerAnimator.SetFloat("Speed", currentSpeed);

		Vector3 worldSpaceDirection = moveDirection.z * centerOfRotationY.forward +
									  moveDirection.x * centerOfRotationY.right;

		Vector3 velocity = rb.velocity;
		velocity.z = worldSpaceDirection.z * moveSpeed;
		velocity.x = worldSpaceDirection.x * moveSpeed;
		if (Input.GetKey (KeyCode.Space) && jumpSensor.CanJump ()) {
			velocity.y = jumpSpeed;
		}
		rb.velocity = velocity;

		float currentRotationX = Input.GetAxis ("Vertical") * rotationSpeedX;
		float currentRotationY = Input.GetAxis ("Horizontal") * rotationSpeedY;

		currentRotationX = -Mathf.Clamp (currentRotationX, -90, 90);

		centerOfRotationX.transform.localEulerAngles += new Vector3 (currentRotationX, 0, 0) * rotationSpeedX;
		centerOfRotationY.transform.localEulerAngles += new Vector3 (0, currentRotationY, 0) * rotationSpeedY;

		centerOfRotationY.transform.position += Time.deltaTime * currentSpeed * centerOfRotationY.forward;
	}
}
