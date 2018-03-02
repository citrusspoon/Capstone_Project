using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

	//Screen world x = -50 ~ 50
	//				y = -80 ~ 80


	/*
		Flow: CheckTrayTouch() responds when tray turret is touched. Sends type of turret selected to SelectTurret(). Mana is checked, if sufficient
		a "ghostTurret" is created (what the user drags around while placing). Location validity is constantly being checked by CheckLocation(). 
		If location is valid, and finger is lifted, turret is placed, mana is subtracted, and turret is added to appropriate list
	*/
	public static TouchManager instance = null;
	public GameObject testObject;
	private bool placingTurret = false;
	public bool choiceSelectionMade = false;
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
	/// <summary>
	/// Checks for when the user touches a turret to be dragged from the tray.
	/// </summary>
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
				}else if (hit.transform.tag == "FRPowerup") {
					type = TurretType.FRPowerup;
				}else if (hit.transform.tag == "RangePowerup") {
					type = TurretType.RangePowerup;
				}else if (hit.transform.tag == "TrayGuard") {
					type = TurretType.Guard;
				}
				else if(hit.transform.tag == "Choice0" && !choiceSelectionMade){
					FlashcardManager.instance.Select (0);
					return;
				}
				else if(hit.transform.tag == "Choice1" && !choiceSelectionMade){
					FlashcardManager.instance.Select (1);
					return;
				}
				else if(hit.transform.tag == "Choice2" && !choiceSelectionMade){
					FlashcardManager.instance.Select (2);
					return;
				}
				else {
					return;
				}
			}
			placingTurret = true;
			SelectTurret (type);

		}
	}
	GameObject ghostTurret;
	Transform ghostTurretTransform;

	/// <summary>
	/// Selects a turret from the tray, and creates a copy to be placed.
	/// </summary>
	/// <param name="t">The type of turret to be selected</param>
	void SelectTurret(TurretType t){
		//TODO: have on screen not enough mana indication
		switch (t) {
		case TurretType.Basic:
			if (ResourceManager.instance.mana < TurretManager.instance.basicStack.Peek ().GetComponent<TurretScript> ().cost) {
				print ("Not enough mana");
				placingTurret = false;
				return;
			}
			ghostTurret = TurretManager.instance.basicStack.Pop ();
			selectedTurretType = TurretType.Basic;
			break;
		case TurretType.Slow:
			if (ResourceManager.instance.mana < TurretManager.instance.slowStack.Peek ().GetComponent<SlowTurret> ().cost) {
				print ("Not enough mana");
				placingTurret = false;
				return;
			}
			ghostTurret = TurretManager.instance.slowStack.Pop ();
			selectedTurretType = TurretType.Slow;
			break;
		case TurretType.Rocket:
			if (ResourceManager.instance.mana < TurretManager.instance.rocketStack.Peek ().GetComponent<RocketTurret> ().cost) {
				print ("Not enough mana");
				placingTurret = false;
				return;
			}
			ghostTurret = TurretManager.instance.rocketStack.Pop ();
			selectedTurretType = TurretType.Rocket;
			break;
		case TurretType.FRPowerup:
			if (ResourceManager.instance.mana < TurretManager.instance.FRPowerupStack.Peek ().GetComponent<FRPowerup> ().cost) {
				print ("Not enough mana");
				placingTurret = false;
				return;
			}
			ghostTurret = TurretManager.instance.FRPowerupStack.Pop ();
			selectedTurretType = TurretType.FRPowerup;
			break;
		case TurretType.RangePowerup:
			if (ResourceManager.instance.mana < TurretManager.instance.rangePowerupStack.Peek ().GetComponent<RangePowerup> ().cost) {
				print ("Not enough mana");
				placingTurret = false;
				return;
			}
			ghostTurret = TurretManager.instance.rangePowerupStack.Pop ();
			selectedTurretType = TurretType.RangePowerup;
			break;
		case TurretType.Guard:
			if (ResourceManager.instance.mana < TurretManager.instance.guardStack.Peek ().GetComponent<GuardTurret> ().cost) {
				print ("Not enough mana");
				placingTurret = false;
				return;
			}
			ghostTurret = TurretManager.instance.guardStack.Pop ();
			selectedTurretType = TurretType.Guard;
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
			if (selectedTurretType != TurretType.FRPowerup && selectedTurretType != TurretType.RangePowerup) {
				TurretManager.instance.placedTurretPositions.Add (ghostTurret.transform.position);
				TurretManager.instance.placedTurrets.Add (ghostTurret);
			}
			if (selectedTurretType == TurretType.Guard)
				TurretManager.instance.placedGuardTurrets.Add (ghostTurret);
			
			switch (selectedTurretType) {
			case TurretType.Basic:
				ghostTurret.GetComponent<TurretScript> ().SetRangeCircleActive(false);
				ResourceManager.instance.ChangeMana(-1*ghostTurret.GetComponent<TurretScript>().cost);
				break;
			case TurretType.Slow:
				ghostTurret.GetComponent<SlowTurret> ().SetRangeCircleActive(false);
				ResourceManager.instance.ChangeMana(-1*ghostTurret.GetComponent<SlowTurret>().cost);
				break;
			case TurretType.Rocket:
				ghostTurret.GetComponent<RocketTurret> ().SetRangeCircleActive(false);
				ResourceManager.instance.ChangeMana(-1*ghostTurret.GetComponent<RocketTurret>().cost);
				break;
			case TurretType.Guard:
				ghostTurret.GetComponent<GuardTurret> ().SetRangeCircleActive (false);
				ResourceManager.instance.ChangeMana (-1 * ghostTurret.GetComponent<GuardTurret> ().cost);
				ResourceManager.instance.changeMaxMana (20f);
				break;
			case TurretType.FRPowerup:
				//ghostTurret.GetComponent<RocketTurret> ().SetRangeCircleActive(false);
				ResourceManager.instance.ChangeMana (-1 * ghostTurret.GetComponent<FRPowerup> ().cost);
				//dp powerup stuff
				turretUnderPowerup.GetComponent<ITurretInfo>().BoostFireRate(ghostTurret.GetComponent<FRPowerup>().boostAmount);
				ghostTurret.SetActive (false);
				TurretManager.instance.FRPowerupStack.Push (ghostTurret);
				break;
			case TurretType.RangePowerup:
				//ghostTurret.GetComponent<RocketTurret> ().SetRangeCircleActive(false);
				ResourceManager.instance.ChangeMana (-1 * ghostTurret.GetComponent<RangePowerup> ().cost);
				//dp powerup stuff
				turretUnderPowerup.GetComponent<ITurretInfo>().BoostRange(ghostTurret.GetComponent<RangePowerup>().boostAmount);
				ghostTurret.SetActive (false);
				TurretManager.instance.rangePowerupStack.Push (ghostTurret);
				break;
			}
			//print ("turret placed");
			TurretManager.instance.turretCounts[selectedTurretType] = ((int)(TurretManager.instance.turretCounts[selectedTurretType])) + 1;

		}
	}

	RaycastHit locHit;
	Ray nRay,sRay,eRay,wRay, cRay;//north south east and west center?
	Vector3 ghostPos;
	float turretDistTolerance = 5f;
	GameObject turretUnderPowerup;
	/// <summary>
	/// Raycasts around the turret being placed to determine if it is currently above a valid placement location.
	/// </summary>
	void CheckLocation(){
		//sets raycasts based on turret type
		locationValid = true;
		turretUnderPowerup = null;
		//==========Checks for surrrounding turrets===========//
		ghostPos = ghostTurret.transform.position;

		//loop method

		if (selectedTurretType == TurretType.FRPowerup || selectedTurretType == TurretType.RangePowerup) {
			for (int i = 0; i < TurretManager.instance.placedTurretPositions.Count && locationValid; i++) {
				//loop and calculate distance between turrets
				if (Vector3.Distance (ghostPos, TurretManager.instance.placedTurretPositions [i]) < turretDistTolerance+3f) 
					turretUnderPowerup = TurretManager.instance.placedTurrets [i];
				
			}
			if (turretUnderPowerup == null)
				locationValid = false;

		} else {

			for (int i = 0; i < TurretManager.instance.placedTurretPositions.Count && locationValid; i++) {
				//loop and calculate distance between turrets
				if (Vector3.Distance (ghostPos, TurretManager.instance.placedTurretPositions [i]) < turretDistTolerance)
					locationValid = false;
			}
		}
			



		//raycast method
		/*
		switch (selectedTurretType) {
		case TurretType.Basic:
			nRay = new Ray(new Vector3(x,1,z+3.5f), Vector3.forward*3);Debug.DrawRay (new Vector3(x ,1,z+3.5f), Vector3.forward*3, Color.cyan);
			sRay = new Ray(new Vector3(x,1,z-3.5f), Vector3.back*3);Debug.DrawRay (new Vector3(x ,1,z-3.5f), Vector3.back*3, Color.cyan);
			eRay = new Ray(new Vector3(x+3.5f,1,z), Vector3.right*3);Debug.DrawRay (new Vector3(x +3.5f,1,z), Vector3.right*3, Color.cyan);
			wRay = new Ray(new Vector3(x-3.5f,1,z), Vector3.left*3);Debug.DrawRay (new Vector3(x -3.5f,1,z), Vector3.left*3, Color.cyan);
			cRay = new Ray(new Vector3(x,1,z), Vector3.down*3); Debug.DrawRay (new Vector3(x,1,z), Vector3.down*3, Color.cyan);
			nRay = new Ray(new Vector3(x,1,z), Vector3.forward);Debug.DrawRay (new Vector3(x ,1,z), Vector3.forward*4, Color.cyan);
			sRay = new Ray(new Vector3(x,1,z), Vector3.back);Debug.DrawRay (new Vector3(x ,1,z), Vector3.back*4, Color.cyan);
			eRay = new Ray(new Vector3(x,1,z), Vector3.right);Debug.DrawRay (new Vector3(x ,1,z), Vector3.right*4, Color.cyan);
			wRay = new Ray(new Vector3(x,1,z), Vector3.left);Debug.DrawRay (new Vector3(x ,1,z), Vector3.left*4, Color.cyan);
			cRay = new Ray(new Vector3(x,1,z), Vector3.left);Debug.DrawRay (new Vector3(x ,1,z), Vector3.down*4, Color.cyan);
			break;
		case TurretType.Slow:


			break;
		case TurretType.Rocket:
			break;

		}



		//checks each raycast and sets the location to invalid if any hit a non turretnode

		if (Physics.Raycast (nRay, out hit, 4f)) {
			if (hit.transform.tag == "Turret") {
				locationValid = false;
				print ("hit north");
			}
		}if (Physics.Raycast (sRay, out hit, 4f)) {
			if (hit.transform.tag == "Turret") {
				locationValid = false;
				print ("hit south");
			}
		}if (Physics.Raycast (eRay, out hit, 4f)) {
			if (hit.transform.tag == "Turret") {
				locationValid = false;
				print ("hit east");
			}
		}if (Physics.Raycast (wRay, out hit, 4f)) {
			if (hit.transform.tag == "Turret") {
				locationValid = false;
				print ("hit west");
			}
		}
		if (Physics.Raycast (cRay, out hit, 4f)) {
			if (hit.transform.tag == "Turret") {
				locationValid = false;
				print ("hit center");
			}
		}

*/
		UpdateGhostRangeCircleColor ();
		if (!locationValid)
			return;


		//============Checks ground=============//

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
		case TurretType.Guard:
			nRay = new Ray(new Vector3(x,5,z+5f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z+5f), Vector3.down*5, Color.red);
			sRay = new Ray(new Vector3(x,5,z-5f), Vector3.down*5);Debug.DrawRay (new Vector3(x ,5,z-5f), Vector3.down*5, Color.red);
			eRay = new Ray(new Vector3(x+4.5f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x +4.5f,5,z), Vector3.down*5, Color.red);
			wRay = new Ray(new Vector3(x-4.5f,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x -4.5f,5,z), Vector3.down*5, Color.red);
			//cRay = new Ray(new Vector3(x,5,z), Vector3.down*5);Debug.DrawRay (new Vector3(x,5,z), Vector3.down*5, Color.red);

			break;

		}


		//checks each raycast and sets the location to invalid if any hit a non turretnode

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
		UpdateGhostRangeCircleColor ();
		//print (locationValid);

		
	}
	/// <summary>
	/// Updates the location of the to be placed turret to follow the touch
	/// </summary>
	void UpdateGhostTurret(){
		if (Input.touchCount == 1) {
			touch = Input.GetTouch (0);
			x = -50f + 100f * touch.position.x / Screen.width;
			z = -80f + 160f * touch.position.y / Screen.height;
			ghostTurretTransform.position = new Vector3 (x, 0.6f, z);
		}
	}
	void UpdateGhostRangeCircleColor(){
		switch (selectedTurretType) {
		case TurretType.Basic:
			if (locationValid)
				ghostTurret.GetComponent<TurretScript> ().ChangeRangeCircleColor (Color.green);
			else
				ghostTurret.GetComponent<TurretScript> ().ChangeRangeCircleColor (Color.red);
				
			break;
		case TurretType.Slow:
			if (locationValid)
				ghostTurret.GetComponent<SlowTurret> ().ChangeRangeCircleColor (Color.green);
			else
				ghostTurret.GetComponent<SlowTurret> ().ChangeRangeCircleColor (Color.red);
			break;
		case TurretType.Rocket:
			if (locationValid)
				ghostTurret.GetComponent<RocketTurret> ().ChangeRangeCircleColor (Color.green);
			else
				ghostTurret.GetComponent<RocketTurret> ().ChangeRangeCircleColor (Color.red);
			break;
		case TurretType.Guard:
			if (locationValid)
				ghostTurret.GetComponent<GuardTurret> ().ChangeRangeCircleColor (Color.green);
			else
				ghostTurret.GetComponent<GuardTurret> ().ChangeRangeCircleColor (Color.red);
			break;
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
