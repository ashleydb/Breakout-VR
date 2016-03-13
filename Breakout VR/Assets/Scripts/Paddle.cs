using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public float paddleSpeed = 1;
	private Vector3 playerPos = new Vector3(0.0f, -9.5f, 0.0f);

	// For doing input in VR mode
	//private Rigidbody inputPlaneRigidbody;
	//private GameObject inputPlane;
	private Collider inputPlaneCollider;
	private Camera mainCamera;

	void Start() {
		//inputPlaneRigidbody = GM.instance.inputPlane.GetComponent<Rigidbody> ();
		//inputPlane = GM.instance.inputPlane;
		inputPlaneCollider = GM.instance.inputPlane.GetComponent<Collider> ();
		mainCamera = GM.instance.mainCamera.GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {

		// See which type of input we should check for
		GM.InputMode inputMode = GM.instance.vrInputMode;
		//if (!Cardboard.SDK.VRModeEnabled) {
		//	inputMode = GM.InputMode.IM_ExternalButtons;
		//}

		float inputValue = 0.0f;

		// Take in the relevant input
		switch (inputMode) {
		case GM.InputMode.IM_ExternalButtons:
			inputValue = Input.GetAxis ("Horizontal");
			break;

		case GM.InputMode.IM_CBGaze:
			inputValue = CheckForGazeCollision();
			break;

		case GM.InputMode.IM_CBClick:
			if (Input.GetButton ("Fire1"))
				inputValue = CheckForGazeCollision();
			break;

		case GM.InputMode.IM_CBRotate:
			inputValue = CheckForGazeRotation();
			break;

		case GM.InputMode.IM_CBTilt:
			inputValue = CheckForHeadTilt();
			break;
		}

		// Move the paddle
		float xPos = transform.position.x + (inputValue * paddleSpeed * Time.deltaTime);
		playerPos = new Vector3 (Mathf.Clamp(xPos, -8.0f, 8.0f), -9.5f, 0.0f);
		transform.position = playerPos;			
	}

	// Move the paddle to level with the point you are looking at.
	private float CheckForGazeCollision() {
		/*
		 * // Attempting to fire a ray from the camera to see if it collides with a plane, using that point to move towards
		Vector3 origin = mainCamera.transform.position;
		Vector3 direction = mainCamera.transform.rotation.eulerAngles;
		RaycastHit hitInfo;
		if (Physics.Raycast(origin, direction, out hitInfo)) {
			if (hitInfo.rigidbody == inputPlaneRigidbody) {
				return hitInfo.point.x;
			}
		}
		return 0.0f;
		*/

		// Checks if a single ray from the camera collides with a single plane we've put in the scene

		// Get a ray from the middle of the screen
		Ray ray = mainCamera.ViewportPointToRay (new Vector3 (0.5F, 0.5F, 0));

		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		//float distance = 0.0f;

		RaycastHit hitInfo;

		// if the ray hits the plane...
		//if (inputPlaneCollider.Raycast (ray, out distance)) {
		if (inputPlaneCollider.Raycast(ray, out hitInfo, 100.0f)) {
			// get the hit point:
			//Vector3 location = ray.GetPoint (distance);
			Vector3 location = hitInfo.point;

			// Compare the point to the position of the paddle to determine movement, with some deadzone to prevent jitter
			// TODO: scale movement by distance between points?
			float distance = location.x - gameObject.transform.position.x;
			if (distance < -0.5f) {
				return -1.0f;
			} else if (distance > 0.5f) {
				return 1.0f;
			}
		}

		return 0.0f;
	}

	// Move the paddle by rotating your head (around Y axis)
	private float CheckForGazeRotation() {
		// Just turning your head left or right from the origin will make the paddle move,
		// so we have a deadzone in the middle to make it stop.
		// TODO: Could check how far we've rotated to scale the speed of movement...
		float yRot = mainCamera.transform.rotation.eulerAngles.y;

		if (yRot > 5.0f && yRot < 180.0f)
			return 1.0f;
		else if (yRot > 180.0f && yRot < 355.0f)
			return -1.0f;
		
		return 0.0f;
	}

	// Move the paddle by tilting your head (around Z axis.) Note that you get some Z rotation during Y rotation.
	private float CheckForHeadTilt() {
		// Just tilting your head left or right will make the paddle move,
		// so we have a deadzone in the middle to require a more deliberate movement.
		// TODO: Could check how far we've rotated to scale the speed of movement...
		float zRot = mainCamera.transform.rotation.eulerAngles.z;

		if (zRot > 5.0f && zRot < 180.0f)
			return -1.0f;
		else if (zRot > 180.0f && zRot < 355.0f)
			return 1.0f;
		
		return 0.0f;
	}
}
