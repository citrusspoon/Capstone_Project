using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {

	public static TurretManager instance = null;
	[Header("Turret Pool Sizes")]
	public int basicPoolSize;
	public int slowPoolSize;
	public int rocketPoolSize;
	[Header("Turret Prefabs")]
	public GameObject basicPrefab;
	public GameObject slowPrefab;
	public GameObject rocketPrefab;
	[Header("Stacks")]
	public Stack<GameObject> basicStack;
	public Stack<GameObject> slowStack;
	public Stack<GameObject> rocketStack;


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
		basicStack = new Stack<GameObject> ();
		slowStack = new Stack<GameObject> ();
		slowStack = new Stack<GameObject> ();
		for (int i = 0; i < basicPoolSize; i++) {
			temp = (GameObject)Instantiate (basicPrefab);
			temp.SetActive (false);
			basicStack.Push (temp);
		}
		for (int i = 0; i < slowPoolSize; i++) {
			temp = (GameObject)Instantiate (slowPrefab);
			temp.SetActive (false);
			slowStack.Push (temp);
		}
		for (int i = 0; i < rocketPoolSize; i++) {
			temp = (GameObject)Instantiate (rocketPrefab);
			temp.SetActive (false);
			rocketStack.Push (temp);
		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
