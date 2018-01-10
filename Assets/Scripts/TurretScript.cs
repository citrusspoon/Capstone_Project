using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {

	public Transform target;
	public float range = 15f;

	// Use this for initialization
	void Start () {
		//updates target every half second to save computing power
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateTarget(){
		
	}


	//Draws the wirefram that show turret range in editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}

}
