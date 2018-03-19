using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public WindowState currentWindow;
	public Transform gameWindowTransform;
	public Transform importMenuTransform;
	public Transform mainMenuTransform;
	public GameObject testUI;
	public GameObject quizletElements;
	private Transform thisTransform;

	private bool moving = false;
	public float camSpeed = 100f;
	// Use this for initialization
	void Start () {
		thisTransform = transform;
		MoveTo (WindowState.Game);
	}
	Vector3 dir;
	Transform target;
	// Update is called once per frame
	void Update () {

		if (moving) { 
			switch (currentWindow) {
			case WindowState.Game:
				target = gameWindowTransform;
				break;
			case WindowState.ImportMenu:
				target = importMenuTransform;
				break;
			case WindowState.MainMenu:
				target = mainMenuTransform;
				break;
			}


			dir = target.position - thisTransform.position;
			transform.Translate (dir.normalized * camSpeed * Time.deltaTime, Space.World);

			if (Vector3.Distance (thisTransform.position, target.position) < 2f) {
				moving = false;
			}
		}
		
	}
	public void MoveTo(WindowState s){
		currentWindow = s;
		moving = true;
	}

	public void SwapCamera(){
		if (currentWindow == WindowState.Game)
			MoveTo(WindowState.ImportMenu);
		else
			MoveTo(WindowState.Game);

		testUI.SetActive (currentWindow == WindowState.Game);
		quizletElements.SetActive (currentWindow == WindowState.ImportMenu);
	}
}
