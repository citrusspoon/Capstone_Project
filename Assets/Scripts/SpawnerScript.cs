using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

	public float timeBetweenWaves = 5f;
	private float countdown = 0f;
	private int waveNum = 0;

	public Transform spawnPoint;
	public List<GameObject> enemyList;
	public List<Wave> waveList;
	public bool spawnerActive;
	public int totalWaves = 10;

	[Header("Enemy Prefabs")]
	public GameObject lv1EnemyPrefab;
	public GameObject lv2EnemyPrefab;
	public GameObject lv3EnemyPrefab;
	public GameObject bossEnemyPrefab;

	[Header("Enemy Pool Sizes")]
	public int lv1EnemyPoolSize;
	public int lv2EnemyPoolSize;
	public int lv3EnemyPoolSize;
	public int bossEnemyPoolSize;

	[Header("Stacks")]
	public Stack<GameObject> lv1EnemyStack;
	public Stack<GameObject> lv2EnemyStack;
	public Stack<GameObject> lv3EnemyStack;
	public Stack<GameObject> bossEnemyStack;


	//TODO: start making the waves do stuff. add temp button to progress wave. add wave display. when spawning eneimes, take randomly from 
	//all possible ones in wave so it doesnt just go in order
	public class Wave
	{
		public int lv1Count, lv2Count, lv3Count, bossCount;
		public int waveNum;

		public Wave(int lv1, int lv2, int lv3, int b){
			lv1Count = lv1;
			lv2Count = lv2;
			lv3Count = lv3;
			bossCount = b;
			waveNum = 0;
		}
		public Wave(int lv1, int lv2, int lv3, int b, int w){
			lv1Count = lv1;
			lv2Count = lv2;
			lv3Count = lv3;
			bossCount = b;
			waveNum = w;
		}
		public Wave(){
			lv1Count = 0;
			lv2Count = 0;
			lv3Count = 0;
			bossCount = 0;
			waveNum = 0;
		}
	}
	//=====Start Variables=========//
	GameObject temp;
	void Start(){
		enemyList = new List<GameObject>();
		waveList = new List<Wave> ();


		lv1EnemyStack = new Stack<GameObject>();
		lv2EnemyStack = new Stack<GameObject>();
		lv3EnemyStack = new Stack<GameObject>();
		bossEnemyStack = new Stack<GameObject>();
		for (int i = 0; i < lv1EnemyPoolSize; i++) {
			temp = (GameObject)Instantiate (lv1EnemyPrefab);
			temp.SetActive (false);
			lv1EnemyStack.Push (temp);
		}
		for (int i = 0; i < lv2EnemyPoolSize; i++) {
			temp = (GameObject)Instantiate (lv2EnemyPrefab);
			temp.SetActive (false);
			lv2EnemyStack.Push (temp);
		}
		for (int i = 0; i < lv3EnemyPoolSize; i++) {
			temp = (GameObject)Instantiate (lv3EnemyPrefab);
			temp.SetActive (false);
			lv3EnemyStack.Push (temp);
		}
		for (int i = 0; i < bossEnemyPoolSize; i++) {
			temp = (GameObject)Instantiate (bossEnemyPrefab);
			temp.SetActive (false);
			bossEnemyStack.Push (temp);
		}


		CreateWaves ();
	}

	void Update(){

		if (!spawnerActive)
			return;

		StackFailsafe ();

		if(countdown <= 0f){
			StartCoroutine (SpawnWave());
			countdown = timeBetweenWaves;
		}
		countdown -= Time.deltaTime;
		//print (countdown);

	}

	IEnumerator SpawnWave(){
		for (int i = 0; i < waveNum; i++) {
			SpawnEnemy ();
			yield return new WaitForSeconds (1f);
		}

		waveNum++;
	}

	void SpawnEnemy(){
		enemyList.Add (Instantiate (lv1EnemyPrefab, spawnPoint.position, spawnPoint.rotation));
	}

	/// <summary>
	/// Instantiates more enemies if the stack runs low.
	/// </summary>
	void StackFailsafe(){
		//if stack runs low
		if (lv1EnemyStack.Count < 5) {
			temp = (GameObject)Instantiate (lv1EnemyPrefab);
			temp.SetActive (false);
			lv1EnemyStack.Push (temp);
		}
		if (lv2EnemyStack.Count < 5) {
			temp = (GameObject)Instantiate (lv2EnemyPrefab);
			temp.SetActive (false);
			lv2EnemyStack.Push (temp);
		}
		if (lv3EnemyStack.Count < 5) {
			temp = (GameObject)Instantiate (lv3EnemyPrefab);
			temp.SetActive (false);
			lv3EnemyStack.Push (temp);
		}
		if (bossEnemyStack.Count < 5) {
			temp = (GameObject)Instantiate (bossEnemyPrefab);
			temp.SetActive (false);
			bossEnemyStack.Push (temp);
		}
	}

	void CreateWaves(){
		waveList.Add (new Wave(20,0,0,0,1));
		waveList.Add (new Wave(40,0,0,0,2));
		waveList.Add (new Wave(30,20,0,0,3));
		waveList.Add (new Wave(40,40,0,0,4));
		waveList.Add (new Wave(40,30,10,0,5));
		waveList.Add (new Wave(50,40,20,0,6));
		waveList.Add (new Wave(50,50,40,0,7));
		waveList.Add (new Wave(50,30,10,1,8));
		waveList.Add (new Wave(80,50,50,0,9));
		waveList.Add (new Wave(50,0,0,3,10));
	}


}
