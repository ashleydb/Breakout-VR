using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

	public int lives = 3;
	public int bricks = 20;
	public float resetDelay = 1.0f;
	public Text livesText;
	public GameObject gameOver;
	public GameObject youWin;
	public GameObject bricksPrefab;
	public GameObject paddle;
	public GameObject deathParticles;
	public static GM instance = null;

	private GameObject clonePaddle;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		Setup ();
	}

	void Setup() {
		clonePaddle = Instantiate (paddle, transform.position, Quaternion.identity) as GameObject;
		Instantiate (bricksPrefab, transform.position, Quaternion.identity);
	}

	void CheckGameOver() {
		if (bricks < 1) {
			youWin.SetActive (true);
			Time.timeScale = 0.25f;
			Invoke ("Reset", resetDelay);
		} else if (lives < 1) {
			gameOver.SetActive (true);
			Time.timeScale = 0.25f;
			Invoke ("Reset", resetDelay);
		}
	}

	void Reset() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	
	public void LoseLife() {
		--lives;
		livesText.text = "Lives: " + lives;
		Instantiate (deathParticles, clonePaddle.transform.position, Quaternion.identity);
		Destroy (clonePaddle);
		Invoke ("SetupPaddle", resetDelay);
		CheckGameOver ();
	}

	void SetupPaddle() {
		clonePaddle = Instantiate (paddle, transform.position, Quaternion.identity) as GameObject;
	}

	public void DestroyBrick() {
		--bricks;
		CheckGameOver ();
	}
}
