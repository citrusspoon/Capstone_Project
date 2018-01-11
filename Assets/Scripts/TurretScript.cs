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
	float distanceToEnemy = Mathf.Infinity;
	GameObject currentEnemy = null;
	GameObject nearestEnemy = null;

	void UpdateTarget(){

		switch(mode) {
		case TargetingMode.Closest:
			UpdateTargetShortest ();
			break;
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
