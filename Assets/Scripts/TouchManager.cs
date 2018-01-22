using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

	//Screen world x = -50 ~ 50
	//				y = -80 ~ 80

	public GameObject testObject;
	//======Update Variables======/

	Touch touch;
	float x,y;
	void Update () {
		if (Input.touchCount == 1) {
			touch = Input.GetTouch (0);
			x = -50f + 100f * touch.position.x / Screen.width;
			y = -80f + 160f * touch.position.y / Screen.height;
			testObject.transform.position = new Vector3 (x,0,y);
		}
	}
}
