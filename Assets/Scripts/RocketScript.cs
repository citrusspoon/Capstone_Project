using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour {


	private Transform target;
	private EnemyScript targetScript;
	private Transform thisTransform;
	public float speed;
	public GameObject hitEffect;
	public float power;
	public float rotationSpeed;
	public float blastRadius;
	private List<GameObject> enemiesInBlast;

	public void Seek(Transform t, EnemyScript e){
		target = t;
		targetScript = e;
	}

	// Use this for initialization
	void Start () {
		thisTransform = transform;
		speed = 50f;
		power = 3f;
		rotationSpeed = 20f;
		enemiesInBlast = new List<GameObject> ();
		GetComponent<SphereCollider> ().radius = blastRadius;
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Enemy" && c.gameObject.GetComponent<EnemyScript>().health > 0)
			enemiesInBlast.Add (c.gameObject);
	}
	void OnTriggerExit(Collider c){
		enemiesInBlast.Remove(c.gameObject);
	}

	//==========Update Variables===========//
	Vector3 dir;
	float distanceThisFrame;
	Quaternion lookRotation;
	Vector3 rotation;
	void Update () {
		if (target == null) {
			this.gameObject.SetActive (false);
			return;
		}
		print (enemiesInBlast.Count);

		for (int i = 0; i < enemiesInBlast.Count; i++) {
			Debug.DrawLine (thisTransform.position, enemiesInBlast[i].transform.position, Color.red);
		}

		dir = target.position - thisTransform.position;

		lookRotation = Quaternion.LookRotation (dir);
		rotation = Quaternion.Lerp(thisTransform.rotation, lookRotation, Time.deltaTime*rotationSpeed).eulerAngles;

		thisTransform.rotation = Quaternion.Euler (0f, rotation.y, 0f);

		distanceThisFrame = speed * Time.deltaTime;

		//triggers if target is hit
		if (dir.magnitude <= distanceThisFrame) {
			HitTarget ();
			return;
		}

		thisTransform.Translate (dir.normalized * distanceThisFrame, Space.World);

	}
	//======HitTarget Variables=======//
	GameObject effect;
	void HitTarget(){

		AmmoBank.instance.rockets.Push (this.gameObject);
		this.gameObject.SetActive (false);
		//targetScript.ReduceHealth (power);
		AOEHit ();
	}

	void AOEHit(){

		for (int i = 0; i < enemiesInBlast.Count; i++) {
			enemiesInBlast [i].GetComponent<EnemyScript> ().ReduceHealth(power);
		}

		enemiesInBlast.Clear ();
	}

}
