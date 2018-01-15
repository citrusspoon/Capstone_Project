using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	private Transform target;
	private EnemyScript targetScript;
	private Transform thisTransform;
	public float speed = 70f;
	public GameObject hitEffect;
	public int power = 1;

	public void Seek(Transform t, EnemyScript e){
		target = t;
		targetScript = e;
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
			StartCoroutine(HitTarget ());
			return;
		}

		thisTransform.Translate (dir.normalized * distanceThisFrame, Space.World);

	}
	//======HitTarget Variables=======//
	GameObject effect;
	IEnumerator HitTarget(){
		
		if (hitEffect != null) {
			effect = Instantiate (hitEffect, thisTransform.position, thisTransform.rotation);
			Destroy (effect, 2f);
		}

		yield return new WaitForSeconds(0.2f);

		AmmoBank.instance.bullets.Push (this.gameObject);
		this.gameObject.SetActive (false);
		targetScript.ReduceHealth (power);
	}
}
