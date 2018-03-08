using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public bool gameView = true;
	public Transform gameViewTransform;
	public Transform quizletViewTransform;
	public GameObject testUI;
	public GameObject quizletElements;
	private Transform thisTransform;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwapCamera(){
		if (gameView)
			transform.position = quizletViewTransform.position;
		else
			transform.position = gameViewTransform.position;

		gameView = !gameView;
		testUI.SetActive (gameView);
		quizletElements.SetActive (!gameView);
	}
}
