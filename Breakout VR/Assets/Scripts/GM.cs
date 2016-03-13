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

	// Used to toggle the input mode, based on the InputMode enum
	public Button inputModeButton;

	// External Buttons is keyboard, gamepad, etc., Gaze is looking in VR mode, Click is the Cardboard button in VR mode
	public enum InputMode { IM_ExternalButtons = 0, IM_CBGaze, IM_CBClick, IM_CBRotate, IM_CBTilt, IM_Invalid }
	public InputMode vrInputMode = InputMode.IM_ExternalButtons;
	public Text instructionsText;

	// For doing input in VR mode
	public GameObject mainCamera;
	public GameObject inputPlane;

	public Button launchBallButton;
	private GameObject clonePaddle;

	// Called before Start()
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		Setup ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.M))
			ToggleVrInputMode ();
	}

	void Setup() {
		clonePaddle = Instantiate (paddle, transform.position, Quaternion.identity) as GameObject;
		Instantiate (bricksPrefab, transform.position, Quaternion.identity);

		vrInputMode = (InputMode)PlayerPrefs.GetInt("VRInputMode");
		UpdateVrInputModeUI ();

		//inputModeButton.gameObject.SetActive (Cardboard.SDK.VRModeEnabled);
		//Text text = inputModeButton.GetComponentInChildren<Text>();
		//text.text = vrInputMode;
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
		launchBallButton.gameObject.SetActive (true);
	}

	public void DestroyBrick() {
		--bricks;
		CheckGameOver ();
	}

	public void LaunchBall() {
		// Get the ball, which is a child of the paddle, and launch it!
		Ball ball = clonePaddle.gameObject.transform.GetChild (0).gameObject.GetComponent<Ball>();
		ball.LaunchBall ();
		launchBallButton.gameObject.SetActive (false);
	}

	public void ToggleVrInputMode() {
		++vrInputMode;
		if (vrInputMode >= InputMode.IM_Invalid)
			vrInputMode = InputMode.IM_ExternalButtons;

		PlayerPrefs.SetInt("VRInputMode", (int)vrInputMode);
		PlayerPrefs.Save();

		UpdateVrInputModeUI ();
	}

	private void UpdateVrInputModeUI() {
		Text text = inputModeButton.GetComponentInChildren<Text>();

		switch (vrInputMode) {
		case InputMode.IM_CBClick:
			text.text = "C";
			instructionsText.text = "Gaze and click";
			break;
		case InputMode.IM_CBGaze:
			text.text = "G";
			instructionsText.text = "Gaze at point";
			break;
		case InputMode.IM_ExternalButtons:
			text.text = "B";
			instructionsText.text = "Gamepad/Keyboard";
			break;
		case InputMode.IM_CBRotate:
			text.text = "R";
			instructionsText.text = "Turn Head";
			break;
		case InputMode.IM_CBTilt:
			text.text = "T";
			instructionsText.text = "Tilt Head";
			break;
		default:
			text.text = "?";
			instructionsText.text = "Input Mode Error";
			break;
		}
	}
}
