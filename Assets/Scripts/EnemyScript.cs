﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	public float speed;
	public EnemyType type;
	private float originalSpeed;
	[HideInInspector]public bool isSlowed;
	public float slowRecovery;
	private float originalSlowRecovery;
	//reference to this object's Transform component
	public Transform thisTransform;
	public Transform target;
	public int nextWaypointIndex;
	public float health;
	public float originalHealth;
	public GameObject deathEffect;
	public ParticleSystem hitEffect;

	void Awake(){
		thisTransform = transform;
	}


	void Start(){

		if(GameController.instance.cameraRef.currentWindow == WindowState.MainMenu)
			target = MainMenuSpawner.instance.endPoint;
		else
			target = WaypointScript.waypoints[0];
		
		originalHealth = health;
		originalSpeed = speed;
		originalSlowRecovery = slowRecovery;
		isSlowed = false;
		nextWaypointIndex = 0;
		hitEffect = GetComponentInChildren<ParticleSystem> ();
	}

	//direction the enemy is moving
	Vector3 dir;

	void Update(){
		dir = target.position - thisTransform.position;
		transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);

		if (Vector3.Distance (thisTransform.position, target.position) < 0.4f) {
			
			if (GameController.instance.cameraRef.currentWindow == WindowState.MainMenu) {
				//target = MainMenuSpawner.instance.endPoint;
				//MainMenuSpawner.instance.enemyList.Remove (this.gameObject);
				//this.gameObject.SetActive (false);
				ReduceHealth (health);
			}
			else
				GetNextWaypoint ();
		}
		
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

		//reaches end
		if (nextWaypointIndex >= WaypointScript.waypoints.Length - 1) {
			//GameController.instance.spawnerScriptRef.enemyList.Remove (this.gameObject);
			ReduceHealth (health);
			//this.gameObject.SetActive (false);
			//TODO: change amount of damage based on enemy type
			ResourceManager.instance.ChangeHealth (-10f);


		}
		else {
			nextWaypointIndex++;
			target = WaypointScript.waypoints [nextWaypointIndex];
		}
	}
	//======ReduceHealth Variables=========/
	GameObject effect;
	public void ReduceHealth(float h){

		//TODO: Figure out how to make this look good
		if (hitEffect != null) {
			//hitEffect.Play();
		}

		health -= h;
		if (health < 1) {
			effect = Instantiate (deathEffect, thisTransform.position, thisTransform.rotation);
			Destroy (effect, 1f);
			if (GameController.instance.cameraRef.currentWindow == WindowState.MainMenu) {
				target = MainMenuSpawner.instance.endPoint;
				MainMenuSpawner.instance.enemyStack.Push (this.gameObject);
			}
			else {
				target = WaypointScript.waypoints [0];

				switch (type) {
				case EnemyType.Lv1:
					SpawnerScript.instance.lv1EnemyStack.Push (this.gameObject);
					break;
				case EnemyType.Lv2:
					SpawnerScript.instance.lv2EnemyStack.Push (this.gameObject);
					break;
				case EnemyType.Lv3:
					SpawnerScript.instance.lv3EnemyStack.Push (this.gameObject);
					break;
				case EnemyType.Boss:
					SpawnerScript.instance.bossEnemyStack.Push (this.gameObject);
					break;
				}
			}
			GameController.instance.spawnerScriptRef.enemyList.Remove (this.gameObject);
			this.gameObject.SetActive (false);





			health = originalHealth;
			ResetSpeed ();
			nextWaypointIndex = 0;
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
