using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBank : MonoBehaviour {

	public static AmmoBank instance = null;

	public int poolSize = 50;
	public Stack<GameObject> bullets;
	public GameObject bulletPrefab;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}


	//=====Start Variables=========//
	GameObject temp;
	void Start () {
		bullets = new Stack<GameObject> ();
		for (int i = 0; i < poolSize; i++) {
			temp = (GameObject)Instantiate (bulletPrefab);
			temp.SetActive (false);
			bullets.Push (temp);
		}
	}
	

	void Update () {
		
		StackFailsafe();
	}


	/// <summary>
	/// Instantiates more bullets if the stack runs low.
	/// </summary>
	void StackFailsafe(){
		//if stack runs low
		if (bullets.Count < 5) {
			temp = (GameObject)Instantiate (bulletPrefab);
			temp.SetActive (false);
			bullets.Push (temp);
		}
	}
}
