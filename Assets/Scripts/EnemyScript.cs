using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	public float speed = 10f;
	private float originalSpeed;
	[HideInInspector]public bool isSlowed = false;
	public float slowRecovery = 2f;
	private float originalSlowRecovery;
	//reference to this object's Transform component
	public Transform thisTransform;
	private Transform target;
	private int nextWaypointIndex = 0;
	public float health = 3f;
	public GameObject deathEffect;

	void Awake(){
		thisTransform = transform;
	}


	void Start(){
		target = WaypointScript.waypoints[0];
		originalSpeed = speed;
		originalSlowRecovery = slowRecovery;
	}

	//direction the enemy is moving
	Vector3 dir;

	void Update(){
		dir = target.position - thisTransform.position;
		transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);

		if (Vector3.Distance (thisTransform.position, target.position) < 0.4f)
			GetNextWaypoint ();
		
			//HandleSlowRecovery ();
		


	}

	/*
	/// <summary>
	/// Resets speed after slowRecovery seconds.  
	/// </summary>
	void HandleSlowRecovery(){
		if (slowRecovery <= 0f) {
			speed = originalSpeed;
			isSlowed = false;
		}
		slowRecovery -= Time.deltaTime;
	}*/

	void GetNextWaypoint(){

		if (nextWaypointIndex >= WaypointScript.waypoints.Length - 1) {
			GameController.instance.spawnerScriptRef.enemyList.Remove (this.gameObject);
			this.gameObject.SetActive (false);
		}
		else {
			nextWaypointIndex++;
			target = WaypointScript.waypoints [nextWaypointIndex];
		}
	}
	//======ReduceHealth Variables=========/
	GameObject effect;
	public void ReduceHealth(float h){
		health -= h;
		if (health < 1) {
			effect = Instantiate (deathEffect, thisTransform.position, thisTransform.rotation);
			Destroy (effect, 1f);
			GameController.instance.spawnerScriptRef.enemyList.Remove (this.gameObject);
			this.gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// Slow enemy speed by the percent passed
	/// </summary>
	/// <param name="percent">Percent to slow enemy by.</param>
	public void Slow(float percent){
		speed -= speed * percent;
		isSlowed = true;
		slowRecovery = originalSlowRecovery;
	}
	/// <summary>
	/// Resets enemy speed to default.
	/// </summary>
	public void ResetSpeed(){
		speed = originalSpeed;
		isSlowed = false;
	}
}
