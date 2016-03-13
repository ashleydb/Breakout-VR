using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public float paddleSpeed = 1;
	private Vector3 playerPos = new Vector3(0.0f, -9.5f, 0.0f);

	// Update is called once per frame
	void Update () {
		float xPos = transform.position.x + (Input.GetAxis ("Horizontal") * paddleSpeed);
		playerPos = new Vector3 (Mathf.Clamp(xPos, -8.0f, 8.0f), -9.5f, 0.0f);
		transform.position = playerPos;			
	}
}
