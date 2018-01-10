using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {

	private Transform target;
	public float range;
	private GameController GCInstance;
	private Transform thisTransform;

	// Use this for initialization
	void Start () {
		GCInstance = GameController.instance;
		thisTransform = GetComponent<Transform> ();
		//updates target every half second to save computing power
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//======UpdateTarget Variables=======//
	float shortestDistance = Mathf.Infinity;
	float distanceToEnemy = Mathf.Infinity;
	GameObject currentEnemy = null;
	GameObject nearestEnemy = null;

	void UpdateTarget(){

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
