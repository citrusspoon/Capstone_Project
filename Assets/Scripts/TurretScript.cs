using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingMode {First, Last, Closest, Furthest};

public class TurretScript : MonoBehaviour {

	/*
		TODO: Create list of turret targeting options with enum
		TODO: Use list to track enemies that enter turret sight
	*/

	private Transform target;
	public TargetingMode mode;
	public float range;
	public float rotationSpeed;
	private GameController GCInstance;
	private Transform thisTransform;
	//piece of the turret that rotates to face the target
	public Transform partToRotate;
	private List<GameObject> enemiesInRange;

	// Use this for initialization
	void Start () {
		GCInstance = GameController.instance;
		enemiesInRange = new List<GameObject> ();
		thisTransform = GetComponent<Transform> ();
		//updates target every half second to save computing power
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
	}
	
	//========Update Variables============//
	Vector3 dir;
	Quaternion lookRotation;
	Vector3 rotation;

	void Update () {
		if (target == null)
			return;
		
		dir = target.position - thisTransform.position;
		lookRotation = Quaternion.LookRotation (dir);
		rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime*rotationSpeed).eulerAngles;

		partToRotate.rotation = Quaternion.Euler (0f, rotation.y, 0f);

		if(target != null)
			Debug.DrawLine (thisTransform.position, target.position, Color.green);
		
	}

	//======UpdateTarget Variables=======//
	float shortestDistance = Mathf.Infinity;
	float longestDistance = Mathf.NegativeInfinity;
	float distanceToEnemy = Mathf.Infinity;
	float distanceToFirstEnemy = Mathf.Infinity;
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
			
		}

	}


	void UpdateTargetFirst(){

		if (GCInstance.spawnerScriptRef.enemyList.Count > 0) {

			for (int i = 0; i < GCInstance.spawnerScriptRef.enemyList.Count; i++) {

				distanceToFirstEnemy = Vector3.Distance (thisTransform.position, GCInstance.spawnerScriptRef.enemyList [i].transform.position);
				if (distanceToFirstEnemy <= range) {
					target = GCInstance.spawnerScriptRef.enemyList [i].transform;
					return;
				}
			}

			//distanceToFirstEnemy = Vector3.Distance (thisTransform.position, GCInstance.spawnerScriptRef.enemyList [0].transform.position);

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
		}
	}


	//Draws the wirefram that show turret range in editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}

}
