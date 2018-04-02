using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSpawner : MonoBehaviour {

	public static MainMenuSpawner instance = null;

	public Transform spawnPoint;
	public Transform endPoint;
	public bool spawnerActive = true;
	public float timeBetweenEnemies = 1f;
	private float countdown = 0f;
	public Stack<GameObject> enemyStack;
	public GameObject enemyPrefab;
	public int enemyPoolSize = 20;
	//public List<GameObject> enemyList;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	GameObject temp;
	void Start () {
		//enemyList = new List<GameObject> ();
		enemyStack = new Stack<GameObject>();
		for (int i = 0; i < enemyPoolSize; i++) {
			temp = (GameObject)Instantiate (enemyPrefab);
			temp.SetActive (false);
			enemyStack.Push (temp);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!spawnerActive)
			return;

		StackFailsafe ();

			if (countdown <= 0f) {
				SpawnEnemy ();
				countdown = timeBetweenEnemies;
			}
			countdown -= Time.deltaTime;


	}
	GameObject tempEnemy;
	void SpawnEnemy(){

		tempEnemy = enemyStack.Pop();

		tempEnemy.transform.position = spawnPoint.position;
		tempEnemy.transform.rotation = spawnPoint.rotation;
		tempEnemy.SetActive (true);

		SpawnerScript.instance.enemyList.Add (tempEnemy);
	}
	public void DestroyEnemiesOnBoard(){
		int x = SpawnerScript.instance.enemyList.Count;
		for (int i = 0; i < x; i++)
			SpawnerScript.instance.enemyList [0].GetComponent<EnemyScript> ().ReduceHealth (SpawnerScript.instance.enemyList [0].GetComponent<EnemyScript> ().health);
	}
	public void StackFailsafe(){
		//if stack runs low
		if (enemyStack.Count < 5) {
			temp = (GameObject)Instantiate (enemyPrefab);
			temp.SetActive (false);
			enemyStack.Push (temp);
		}

	}
}
