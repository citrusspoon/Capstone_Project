using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

	//Screen world x = -50 ~ 50
	//				y = -80 ~ 80
	public static TouchManager instance = null;
	public GameObject testObject;
	private bool placingTurret = false;
	//======Update Variables======/

	Touch touch;
	float x,z;
	RaycastHit hit;
	Ray ray;


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
		CheckTrayTouch();
		if(placingTurret)
			CheckPlaceTurret ();

		if (placingTurret)
			UpdateGhostTurret();


	}

	TurretType type;
	void CheckTrayTouch(){
		if (Input.touchCount == 1 && !placingTurret) {
			touch = Input.GetTouch (0);
			x = -50f + 100f * touch.position.x / Screen.width;
			z = -80f + 160f * touch.position.y / Screen.height;

			ray = new Ray(new Vector3(x,20,z), Vector3.down*30);
			Debug.DrawRay (new Vector3(x,20,z), Vector3.down*30, Color.green);

			if (Physics.Raycast (ray, out hit))
			if (hit.transform.tag == "TrayBasic") {
				type = TurretType.Basic;
			} else if (hit.transform.tag == "TraySlow") {
				type = TurretType.Slow;
			} else if (hit.transform.tag == "TrayRocket") {
				type = TurretType.Rocket;
			} else {
				return;
			}
			placingTurret = true;
			SelectTurret (type);

		}
	}
	GameObject ghostTurret;
	Transform ghostTurretTransform;
	void SelectTurret(TurretType t){
		print ("Selected " + t);
		switch (t) {
		case TurretType.Basic:
			ghostTurret = TurretManager.instance.basicStack.Pop ();
			break;
		case TurretType.Slow:
			ghostTurret = TurretManager.instance.slowStack.Pop ();
			break;
		case TurretType.Rocket:
			ghostTurret = TurretManager.instance.rocketStack.Pop ();
			break;
			
		}
		ghostTurret.transform.position = new Vector3 (x, 0.6f, z);
		ghostTurret.transform.localRotation = Quaternion.Euler (0,0,0);
		ghostTurretTransform = ghostTurret.GetComponent<Transform> ();
		ghostTurret.SetActive (true);

	}
	void CheckPlaceTurret(){
		if (Input.touchCount < 1) {
			placingTurret = false;
			print ("turret placed");
		}
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
