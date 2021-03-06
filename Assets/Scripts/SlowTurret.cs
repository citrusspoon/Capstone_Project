﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurret : MonoBehaviour, ITurretInfo {

	public float range;
	public float fireRate;
	public float originalRange;
	public float originalFireRate;
	private float fireCountdown;
	[HideInInspector]public TurretType type;
	public float cost = 50f;
	//how much to reduce speed by
	public float slowPercent;
	public float power;
	public ParticleSystem slowParticles;
	private List<GameObject> enemiesInRange;
	public GameObject rangeCircle;
	public Transform thisTransform;
	[HideInInspector]public bool turretConflict = false; //if you try to place a turret on top of another

	// Use this for initialization
	void Start () {
		enemiesInRange = new List<GameObject> ();
		GetComponent<SphereCollider> ().radius = range;
		thisTransform = transform;
		range = 18f;
		fireRate = 1f;
		originalRange = range;
		originalFireRate = fireRate;
		fireCountdown = 0f;
		slowPercent = 0.3f;
	 	power = 0f;
		rangeCircle.transform.localScale = new Vector3 (range * 2 / thisTransform.localScale.x,0.0001f,range * 2 / thisTransform.localScale.z);
		type = TurretType.Slow;
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
		TurretManager.instance.slowStack.Push (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (enemiesInRange.Count == 0) {//if there are no enemies in range nothing below here will run
			slowParticles.Stop();
			return;
		}
		slowParticles.Play ();


		//=========Firing=========//

		if (fireCountdown <= 0f) {
			Shoot ();
			fireCountdown = 1f / fireRate;
		}
		fireCountdown -= Time.deltaTime;	
	}

	//==========Shoot Variables========//
	private EnemyScript enemy;
	void Shoot(){
		for (int i = 0; i < enemiesInRange.Count; i++) {
			enemy = enemiesInRange [i].GetComponent<EnemyScript> ();
			if (!enemy.isSlowed) {
				enemy.Slow (slowPercent);
				enemy.ReduceHealth (power);
			}
		}
	}




	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Enemy")
			enemiesInRange.Add (c.gameObject);
		/*if (c.gameObject.tag == "Turret")
			turretConflict = true;*/
	}

	void OnTriggerExit(Collider c){
		if (c.gameObject.tag == "Enemy") {
			c.gameObject.GetComponent<EnemyScript> ().ResetSpeed ();
			enemiesInRange.Remove (c.gameObject);
		}
		/*if (c.gameObject.tag == "Turret")
			turretConflict = false;*/

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

	//Draws the wirefram that show turret range in editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}
}
