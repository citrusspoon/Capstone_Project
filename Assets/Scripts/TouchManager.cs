﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

	//Screen world x = -50 ~ 50
	//				y = -80 ~ 80
	public static TouchManager instance = null;
	public GameObject testObject;
	private bool placingTurret = false;
	private bool locationValid = true;
	//======Update Variables======/

	Touch touch;
	float x,z;
	RaycastHit hit;
	Ray ray;
	TurretType selectedTurretType;



	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	void Update () {
		//TestDrag ();
		//print(Input.touchCount);

		if (placingTurret) {
			CheckPlaceTurret ();
			UpdateGhostTurret ();
			CheckLocation ();
		} else {
			CheckTrayTouch();
		}

	
			


	}

	TurretType type;
	void CheckTrayTouch(){
		if (Input.touchCount == 1 && !placingTurret) {
			touch = Input.GetTouch (0);
			x = -50f + 100f * touch.position.x / Screen.width;
			z = -80f + 160f * touch.position.y / Screen.height;

			ray = new Ray(new Vector3(x,20,z), Vector3.down*30);
			Debug.DrawRay (new Vector3(x,20,z), Vector3.down*30, Color.green);

			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag == "TrayBasic") {
					type = TurretType.Basic;
				} else if (hit.transform.tag == "TraySlow") {
					type = TurretType.Slow;
				} else if (hit.transform.tag == "TrayRocket") {
					type = TurretType.Rocket;
				} else {
					return;
				}
			}
			placingTurret = true;
			SelectTurret (type);

		}
	}
	GameObject ghostTurret;
	Transform ghostTurretTransform;
	void SelectTurret(TurretType t){
		//print ("Selected " + t);
		switch (t) {
		case TurretType.Basic:
			ghostTurret = TurretManager.instance.basicStack.Pop ();
			selectedTurretType = TurretType.Basic;
			break;
		case TurretType.Slow:
			ghostTurret = TurretManager.instance.slowStack.Pop ();
			selectedTurretType = TurretType.Slow;
			break;
		case TurretType.Rocket:
			ghostTurret = TurretManager.instance.rocketStack.Pop ();
			selectedTurretType = TurretType.Rocket;
			break;
			
		}
		ghostTurret.transform.position = new Vector3 (x, 0.6f, z);
		ghostTurret.transform.localRotation = Quaternion.Euler (0,0,0);
		ghostTurretTransform = ghostTurret.GetComponent<Transform> ();
		ghostTurret.SetActive (true);

	}
	void CheckPlaceTurret(){
		if (Input.touchCount < 1 && locationValid) {
			placingTurret = false;
			//print ("turret placed");
		}
	}
	RaycastHit locHit;
	Ray nRay,sRay,eRay,wRay, cRay;//north south east and west
	//bool nValid, sValid, eValid, wValid;
	void CheckLocation(){
		//sets raycasts based on turret type
		switch (selectedTurretType) {
		case TurretType.Basic:
			nRay = new Ray(new Vector3(x,5,z+3.5f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z+3.5f), Vector3.down*5, Color.red);
			sRay = new Ray(new Vector3(x,5,z-3.5f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z-3.5f), Vector3.down*5, Color.red);
			eRay = new Ray(new Vector3(x+3.5f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x +3.5f,5,z), Vector3.down*5, Color.red);
			wRay = new Ray(new Vector3(x-3.5f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x -3.5f,5,z), Vector3.down*5, Color.red);
			break;
		case TurretType.Slow:
			nRay = new Ray(new Vector3(x,5,z+4f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z+4f), Vector3.down*5, Color.red);
			sRay = new Ray(new Vector3(x,5,z-4f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z-5f), Vector3.down*5, Color.red);
			eRay = new Ray(new Vector3(x+2.0f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x +2.0f,5,z), Vector3.down*5, Color.red);
			wRay = new Ray(new Vector3(x-2.0f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x -2.0f,5,z), Vector3.down*5, Color.red);
			//cRay = new Ray(new Vector3(x,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x,5,z), Vector3.down*5, Color.red);

			break;
		case TurretType.Rocket:
			nRay = new Ray(new Vector3(x,5,z+3.5f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z+3.5f), Vector3.down*5, Color.red);
			sRay = new Ray(new Vector3(x,5,z-3.5f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z-3.5f), Vector3.down*5, Color.red);
			eRay = new Ray(new Vector3(x+3.5f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x +3.5f,5,z), Vector3.down*5, Color.red);
			wRay = new Ray(new Vector3(x-3.5f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x -3.5f,5,z), Vector3.down*5, Color.red);
			break;

		}


		//checks each raycast and sets the location to invalid if any hit a non turretnode
		locationValid = true;
		if (Physics.Raycast (nRay, out hit)) {
			if (hit.transform.tag != "TurretNode") {
				locationValid = false;
			}
		}if (Physics.Raycast (sRay, out hit)) {
			if (hit.transform.tag != "TurretNode") {
				locationValid = false;
			}
		}if (Physics.Raycast (eRay, out hit)) {
			if (hit.transform.tag != "TurretNode") {
				locationValid = false;
			}
		}if (Physics.Raycast (wRay, out hit)) {
			if (hit.transform.tag != "TurretNode") {
				locationValid = false;
			}
		}

		//print (locationValid);

		
	}
	void UpdateGhostTurret(){
		if (Input.touchCount == 1) {
			touch = Input.GetTouch (0);
			x = -50f + 100f * touch.position.x / Screen.width;
			z = -80f + 160f * touch.position.y / Screen.height;
			ghostTurretTransform.position = new Vector3 (x, 0.6f, z);
		}
	}

	void TestDrag(){
		if (Input.touchCount == 1) {
			touch = Input.GetTouch (0);
			x = -50f + 100f * touch.position.x / Screen.width;
			z = -80f + 160f * touch.position.y / Screen.height;
			testObject.transform.position = new Vector3 (x,0,z);
		}
	}
}
