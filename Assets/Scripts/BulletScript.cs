using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	private Transform target;
	private Transform thisTransform;
	public float speed = 70f;

	public void Seek(Transform t){
		target = t;
		print ("seek entered");

	}

	// Use this for initialization
	void Start () {
		thisTransform = transform;
	}
	
	//==========Update Variables===========//
	Vector3 dir;
	float distanceThisFrame;
	void Update () {
		if (target == null) {
			this.gameObject.SetActive (false);
			return;
		}

		dir = target.position - thisTransform.position;
		distanceThisFrame = speed * Time.deltaTime;

		//triggers if target is hit
		if (dir.magnitude <= distanceThisFrame) {
			HitTarget ();
			return;
		}

		thisTransform.Translate (dir.normalized * distanceThisFrame, Space.World);

	}
	void HitTarget(){
		print ("hit");
	}
}
