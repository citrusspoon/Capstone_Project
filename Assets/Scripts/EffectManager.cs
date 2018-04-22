using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {


	public static EffectManager instance = null;
	public GameObject rocketExplosion;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//TODO: make efficient
	GameObject temp;
	public void ExplosionAtPos(Vector3 pos){
		temp = Instantiate (rocketExplosion) as GameObject;
		temp.transform.position = pos;

		Destroy (temp, 5);
	}
}
