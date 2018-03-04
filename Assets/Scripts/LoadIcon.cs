using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadIcon : MonoBehaviour {


	public Text loadingText;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("CycleLoadText", 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	int periodCount = 0;
	void CycleLoadText(){

		if (periodCount > 5) {
			periodCount = 0;
			loadingText.text = "Loading";
		}

		loadingText.text = loadingText.text + ".";
		periodCount++;

	}
}
