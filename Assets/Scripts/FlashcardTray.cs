using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashcardTray : MonoBehaviour {

	public bool isActive = true;
	private bool toggling = false;
	public float traySpeed = 10f;
	public Transform activePosition;
	public Transform inactivePosition;
	private Transform thisTransform;


	// Use this for initialization
	void Start () {
		thisTransform = GetComponent<Transform> ();
	}

	//=======Update Variables=======//
	Vector3 dir;
	Transform target;

	void Update () {

		if (toggling) { 
			if (isActive) {
				target = inactivePosition;
			} else {
				target = activePosition;
			}

			dir = target.position - thisTransform.position;
			transform.Translate (dir.normalized * traySpeed * Time.deltaTime, Space.World);

			if (Vector3.Distance (thisTransform.position, target.position) < 0.9f) {
				toggling = false;
				isActive = !isActive;
			}
		}
	}

	public void ToggleTray(){
		toggling = true;
	}
}
