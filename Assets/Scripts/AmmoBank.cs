using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBank : MonoBehaviour {

	public static AmmoBank instance = null;
	[Header("Ammo Pool Sizes")]
	public int bulletPoolSize;
	public int rocketPoolSize;
	[Header("Ammo Prefabs")]
	public GameObject bulletPrefab;
	public GameObject rocketPrefab;
	[Header("Stacks")]
	public Stack<GameObject> rockets;
	public Stack<GameObject> bullets;


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
		rockets = new Stack<GameObject> ();
		for (int i = 0; i < bulletPoolSize; i++) {
			temp = (GameObject)Instantiate (bulletPrefab);
			temp.SetActive (false);
			bullets.Push (temp);
		}
		for (int i = 0; i < rocketPoolSize; i++) {
			temp = (GameObject)Instantiate (rocketPrefab);
			temp.SetActive (false);
			rockets.Push (temp);
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
		if (rockets.Count < 5) {
			temp = (GameObject)Instantiate (rocketPrefab);
			temp.SetActive (false);
			rockets.Push (temp);
		}
	}
}
