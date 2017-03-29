using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public static double[] sceneBoundaries = new double[2] {-20, 20};
	public static double[] cameraBoundaries = new double[2] {-8.31, 9.81};

	public Text winText;
	public GameObject bird;

	private int sceneIndex;

	void Awake () {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;

		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
		}

		if (sceneIndex == 1) {
			winText.enabled = false;
		}



		//DontDestroyOnLoad (gameObject);
		//SceneManager.LoadScene ("Scenes/Stage1", LoadSceneMode.Single);
	}

	void Update () {
		
		if (bird.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).fullPathHash == Animator.StringToHash ("Base Layer.ChangeScene")) {
			if (sceneIndex == 0) {
				SceneManager.LoadScene ("Scenes/Stage2", LoadSceneMode.Single);
			} else if (sceneIndex == 1) {
				winText.enabled = true;
			}
		}
	}
			
}
