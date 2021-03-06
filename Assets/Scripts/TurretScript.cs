﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour, ITurretInfo {

	private Transform target;
	private EnemyScript targetScript;
	[Header("Attributes")]
	public TargetingMode mode;
	public float range = 18f;
	public float originalRange;
	public float fireRate;
	public float originalFireRate;
	public float cost = 30f;
	private float fireCountdown;
	public float rotationSpeed;
	[HideInInspector]public TurretType type;
	[Header("Other")]
	public GameObject rangeCircle;
	public Transform partToRotate;
	public Transform firePoint;
	public GameObject bulletPrefab;
	private GameController GCInstance;
	private Transform thisTransform;
	private AudioSource audioSrc;
	[HideInInspector]public bool turretConflict = false; //if you try to place a turret on top of another
	//piece of the turret that rotates to face the target


	// Use this for initialization
	void Start () {
		GCInstance = GameController.instance;
		thisTransform = GetComponent<Transform> ();
		type = TurretType.Basic;
		//updates target every half second to save computing power
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
		range = 18f;
		originalRange = range;
		fireRate = 1f;
		originalFireRate = fireRate;
		fireCountdown = 0f;
		rotationSpeed = 10f;
		rangeCircle.transform.localScale = new Vector3 (range * 2 / thisTransform.localScale.x,0.0001f,range * 2 / thisTransform.localScale.z);
		audioSrc = GetComponent<AudioSource> ();
	}
	TurretType ITurretInfo.GetType(){
		return type;
	}
	void ITurretInfo.BoostFireRate(float boostAmount){
		fireRate *= boostAmount;
	}
	void ITurretInfo.BoostRange(float boostAmount){
		range *= boostAmount;
		SetRangeCircleActive (true);
		ChangeRangeCircleColor (Color.green);
		StartCoroutine(DisableRangeCircleWithDelay (3f));

	}

	IEnumerator DisableRangeCircleWithDelay(float s){
		yield return new WaitForSeconds (s);

		SetRangeCircleActive (false);
	}
	void ITurretInfo.DestroySelf(){
		TurretManager.instance.placedTurrets.Remove (this.gameObject);
		TurretManager.instance.placedTurretPositions.Remove (thisTransform.position);
		SetRangeCircleActive (true);
		range = originalRange;
		fireRate = originalFireRate;
		TurretManager.instance.turretCounts[type] = ((int)(TurretManager.instance.turretCounts[type])) - 1;
		this.gameObject.SetActive (false);
		TurretManager.instance.basicStack.Push (this.gameObject);
	}

	
	//========Update Variables============//
	Vector3 dir;
	Quaternion lookRotation;
	Vector3 rotation;

	void Update () {
		if (target == null) //if there is no target nothing below here will run
			return;
		//==================Aiming====================//
		dir = target.position - thisTransform.position;
		lookRotation = Quaternion.LookRotation (dir);
		rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime*rotationSpeed).eulerAngles;

		partToRotate.rotation = Quaternion.Euler (0f, rotation.y, 0f);

		if(target != null)
			Debug.DrawLine (thisTransform.position, target.position, Color.green);

		//=========Firing=========//

		if (fireCountdown <= 0f) {
			Shoot ();
			fireCountdown = 1f / fireRate;
		}
		fireCountdown -= Time.deltaTime;	
	}
	//==========Shoot Variables============//
	GameObject bullet;
	BulletScript bulletScript;
	void Shoot(){
		//bullet = (GameObject)Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
		audioSrc.PlayOneShot(audioSrc.clip, 0.2f);
		bullet = AmmoBank.instance.bullets.Pop();
		bullet.transform.position = firePoint.position;
		bullet.transform.localRotation = firePoint.rotation;
		bullet.SetActive (true);
		bulletScript = bullet.GetComponent<BulletScript> ();

		if (bulletScript != null)
			bulletScript.Seek (target, targetScript);
	}

	//======UpdateTarget Variables=======//
	float shortestDistance = Mathf.Infinity;
	float longestDistance = Mathf.NegativeInfinity;
	float distanceToEnemy = Mathf.Infinity;
	float distanceToFirstEnemy = Mathf.Infinity;
	float distanceToLastEnemy = Mathf.Infinity;
	GameObject currentEnemy = null;
	GameObject nearestEnemy = null;
	GameObject furthestEnemy = null;

	void UpdateTarget(){
		//TODO: add strongest/weakest
		target = null;

		switch(mode) {
		case TargetingMode.Closest:
			UpdateTargetShortest ();
			break;
		case TargetingMode.Furthest:
			UpdateTargetLongest ();
			break;
		case TargetingMode.First:
			UpdateTargetFirst ();
			break;
		case TargetingMode.Last:
			UpdateTargetLast ();
			break;
			
		}

	}


	void UpdateTargetFirst(){

		if (GCInstance.spawnerScriptRef.enemyList.Count > 0) {

			for (int i = 0; i < GCInstance.spawnerScriptRef.enemyList.Count; i++) {

				distanceToFirstEnemy = Vector3.Distance (thisTransform.position, GCInstance.spawnerScriptRef.enemyList [i].transform.position);
				if (distanceToFirstEnemy <= range) {
					target = GCInstance.spawnerScriptRef.enemyList [i].transform;
					targetScript = GCInstance.spawnerScriptRef.enemyList [i].GetComponent<EnemyScript>();
					return;
				}
			}
		}
	}

	void UpdateTargetLast(){

		if (GCInstance.spawnerScriptRef.enemyList.Count > 0) {

			for (int i = GCInstance.spawnerScriptRef.enemyList.Count-1; i >= 0; i--) {

				distanceToLastEnemy = Vector3.Distance (thisTransform.position, GCInstance.spawnerScriptRef.enemyList [i].transform.position);
				if (distanceToLastEnemy <= range) {
					target = GCInstance.spawnerScriptRef.enemyList [i].transform;
					targetScript = GCInstance.spawnerScriptRef.enemyList [i].GetComponent<EnemyScript>();
					return;
				}
			}
		}
	}



	//TODO: fix this
	void UpdateTargetLongest(){
		longestDistance = Mathf.NegativeInfinity;
		distanceToEnemy = Mathf.NegativeInfinity;
		currentEnemy = null;
		furthestEnemy = null;

		for (int i = 0; i < GCInstance.spawnerScriptRef.enemyList.Count; i++) {

			currentEnemy = GCInstance.spawnerScriptRef.enemyList [i];
			distanceToEnemy = Vector3.Distance (thisTransform.position, currentEnemy.transform.position);

			if (distanceToEnemy > longestDistance) {
				longestDistance = distanceToEnemy;
				furthestEnemy = currentEnemy;
			} 
		}

		if (furthestEnemy != null && longestDistance <= range) {
			target = furthestEnemy.transform;

		}
	}

	void UpdateTargetShortest(){
		shortestDistance = Mathf.Infinity;
		distanceToEnemy = Mathf.Infinity;
		currentEnemy = null;
		nearestEnemy = null;
		for (int i = 0; i < GCInstance.spawnerScriptRef.enemyList.Count; i++) {

			currentEnemy = GCInstance.spawnerScriptRef.enemyList [i];
			distanceToEnemy = Vector3.Distance (thisTransform.position, currentEnemy.transform.position);

			if (distanceToEnemy < shortestDistance) {
				shortestDistance = distanceToEnemy;
				nearestEnemy = currentEnemy;
			} 

		}
		if (nearestEnemy != null && shortestDistance <= range) {
			target = nearestEnemy.transform;
			targetScript = nearestEnemy.GetComponent<EnemyScript> ();
		}
	}
	public void ToggleRangeCircle(){
		rangeCircle.SetActive (!rangeCircle.activeSelf);
	}
	public void SetRangeCircleActive(bool b){
		rangeCircle.transform.localScale = new Vector3 (range * 2 / thisTransform.localScale.x,0.0001f,range * 2 / thisTransform.localScale.z);
		rangeCircle.SetActive (b);
	}
	public void ChangeRangeCircleColor(Color c){
		rangeCircle.GetComponent<Renderer> ().material.color = new Color(c.r, c.g, c.b, 0.5f);

	}
	/*
	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "Turret")
			turretConflict = true;
	}
	void OnTriggerExit(Collider c){
		if (c.gameObject.tag == "Turret")
			turretConflict = false;
	}*/


	//Draws the wirefram that show turret range in editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}

}
