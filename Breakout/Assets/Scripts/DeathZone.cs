using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		GM.instance.LoseLife ();
	}
}
