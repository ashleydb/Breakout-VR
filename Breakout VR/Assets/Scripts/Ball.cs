using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float ballInitialVelocity = 600.0f;
	private Rigidbody rb;
	private bool ballInPlay;

	// Called before Start()
	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}

	public void LaunchBall () {
		if (ballInPlay == false) {
			transform.parent = null;
			ballInPlay = true;
			rb.isKinematic = false;
			rb.AddForce (new Vector3 (ballInitialVelocity, ballInitialVelocity, 0.0f));
		}
	}
}
