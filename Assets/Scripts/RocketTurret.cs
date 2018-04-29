using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RocketTurret : MonoBehaviour, ITurretInfo {

	private Transform target;
	private EnemyScript targetScript;
	[Header("Attributes")]
	public TargetingMode mode;
	public float range;
	public float fireRate;
	public float originalRange;
	public float originalFireRate;
	public float cost = 80f;
	private float fireCountdown;
	public float rotationSpeed;
	[HideInInspector]public TurretType type;
	[Header("Other")]
	public GameObject rangeCircle;
	public Transform partToRotate;
	public Transform firePoint;
	public GameObject rocketPrefab;
	private GameController GCInstance;
	private Transform thisTransform;
	private AudioSource audioSrc;
	[HideInInspector]public bool turretConflict = false; //if you try to place a turret on top of another


	// Use this for initialization
	void Start () {

		range = 18f;
		fireRate = 1f;
		fireCountdown = 0f;
		originalRange = range;
		originalFireRate = fireRate;
		rotationSpeed = 10f;
		type = TurretType.Rocket;
		GCInstance = GameController.instance;
		audioSrc = GetComponent<AudioSource> ();
		thisTransform = GetComponent<Transform> ();
		//updates target every half second to save computing power
		rangeCircle.transform.localScale = new Vector3 (range * 2 / thisTransform.localScale.x,0.0001f,range * 2 / thisTransform.localScale.z);
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
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
		fireRate = originalFireRate;
		range = originalRange;
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
	GameObject rocket;
	RocketScript rocketScript;
	void Shoot(){
		rocket = AmmoBank.instance.rockets.Pop();
		rocket.transform.position = firePoint.position;
		rocket.transform.localRotation = firePoint.rotation;
		rocket.SetActive (true);
		rocketScript = rocket.GetComponent<RocketScript> ();

		if (rocketScript != null)
			rocketScript.Seek (target, targetScript);

		audioSrc.PlayOneShot (audioSrc.clip, 1f);
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
