﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTurret : MonoBehaviour, ITurretInfo {

	[Header("Attributes")]
	public float range;
	public float fireRate;
	public float originalRange;
	public float originalFireRate;
	public float cost = 90f;
	[HideInInspector]public TurretType type;
	[Header("Other")]
	public GameObject rangeCircle;
	public GameObject brokenIndicator;
	private GameController GCInstance;
	private Transform thisTransform;
	[HideInInspector]public bool turretConflict = false;
	[HideInInspector]public bool broken = false;

	// Use this for initialization
	void Start () {
		GCInstance = GameController.instance;
		thisTransform = GetComponent<Transform> ();
		type = TurretType.Guard;
		range = 7f;
		originalRange = range;
		fireRate = 1f;
		originalFireRate = fireRate;
		rangeCircle.transform.localScale = new Vector3 (range * 2 / thisTransform.localScale.x,0.0001f,range * 2 / thisTransform.localScale.z);
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
		TurretManager.instance.placedGuardTurrets.Remove (this.gameObject);
		TurretManager.instance.placedTurretPositions.Remove (thisTransform.position);
		SetRangeCircleActive (true);
		fireRate = originalFireRate;
		range = originalRange;
		TurretManager.instance.turretCounts[type] = ((int)(TurretManager.instance.turretCounts[type])) - 1;
		this.gameObject.SetActive (false);
		TurretManager.instance.guardStack.Push (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void breakTurret(){
		broken = true;
		brokenIndicator.SetActive (true);
	}
	public void unBreakTurret(){
		broken = false;
		brokenIndicator.SetActive (false);
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
}
