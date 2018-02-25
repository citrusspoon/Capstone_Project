using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {

	public static TurretManager instance = null;
	[Header("Turret Pool Sizes")]
	public int basicPoolSize;
	public int slowPoolSize;
	public int rocketPoolSize;
	public int FRPowerupPoolSize;
	public int rangePowerupPoolSize;
	[Header("Turret Prefabs")]
	public GameObject basicPrefab;
	public GameObject slowPrefab;
	public GameObject rocketPrefab;
	public GameObject FRPowerupPrefab;
	public GameObject rangePowerupPrefab;
	[Header("Stacks")]
	public Stack<GameObject> basicStack;
	public Stack<GameObject> slowStack;
	public Stack<GameObject> rocketStack;
	public Stack<GameObject> FRPowerupStack;
	public Stack<GameObject> rangePowerupStack;
	[Header("Lists")]
	public List<Vector3> placedTurretPositions;
	public List<GameObject> placedTurrets;
	/*[Header("Tray")]
	public Vector3 basicTraySpawnPoint;
	public Vector3 slowTraySpawnPoint;
	public Vector3 rocketTraySpawnPoint;*/


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
		placedTurretPositions = new List<Vector3> ();
		placedTurrets = new List<GameObject> ();
		basicStack = new Stack<GameObject> ();
		slowStack = new Stack<GameObject> ();
		rocketStack = new Stack<GameObject> ();
		FRPowerupStack = new Stack<GameObject> ();
		rangePowerupStack = new Stack<GameObject> ();
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
		for (int i = 0; i < FRPowerupPoolSize; i++) {
			temp = (GameObject)Instantiate (FRPowerupPrefab);
			temp.SetActive (false);
			FRPowerupStack.Push (temp);
		}
		for (int i = 0; i < rangePowerupPoolSize; i++) {
			temp = (GameObject)Instantiate (rangePowerupPrefab);
			temp.SetActive (false);
			rangePowerupStack.Push (temp);
		}
	}


	// Update is called once per frame
	void Update () {
		StackFailsafe ();
		
	}
	/// <summary>
	/// Instantiates more turrets if the stack runs low.
	/// </summary>
	void StackFailsafe(){
		//if stack runs low
		if (basicStack.Count < 2) {
			temp = (GameObject)Instantiate (basicPrefab);
			temp.SetActive (false);
			basicStack.Push (temp);
		}
		if (slowStack.Count < 2) {
			temp = (GameObject)Instantiate (slowPrefab);
			temp.SetActive (false);
			slowStack.Push (temp);
		}
		if (rocketStack.Count < 2) {
			temp = (GameObject)Instantiate (rocketPrefab);
			temp.SetActive (false);
			rocketStack.Push (temp);
		}
		if (FRPowerupStack.Count < 2) {
			temp = (GameObject)Instantiate (FRPowerupPrefab);
			temp.SetActive (false);
			FRPowerupStack.Push (temp);
		}
		if (rangePowerupStack.Count < 2) {
			temp = (GameObject)Instantiate (rangePowerupPrefab);
			temp.SetActive (false);
			rangePowerupStack.Push (temp);
		}
	}
}
