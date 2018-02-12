using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerScript : MonoBehaviour {

	public float timeBetweenEnemies = 1f;
	private float countdown = 0f;
	private int waveNum = 0;
	public GameState currentGameState;

	public static SpawnerScript instance = null;

	public Transform spawnPoint;
	//list of enemies currently on the board
	public List<GameObject> enemyList;
	//list of enemies to be spawned
	public List<GameObject> enemyWaitingRoom;
	public List<Wave> waveList;
	public bool spawnerActive;
	public int totalWaves = 10;
	public Text waveNumText;

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


	//TODO: start making the waves do stuff. when spawning eneimes, take randomly from 
	//all possible ones in wave so it doesnt just go in order
	public class Wave
	{
		public int lv1Count, lv2Count, lv3Count, bossCount;
		public int waveNum;
		public int totalCount;

		public Wave(int lv1, int lv2, int lv3, int b){
			lv1Count = lv1;
			lv2Count = lv2;
			lv3Count = lv3;
			bossCount = b;
			totalCount = lv1Count + lv2Count + lv3Count + bossCount;
			waveNum = 0;
		}
		public Wave(int lv1, int lv2, int lv3, int b, int w){
			lv1Count = lv1;
			lv2Count = lv2;
			lv3Count = lv3;
			bossCount = b;
			totalCount = lv1Count + lv2Count + lv3Count + bossCount;
			waveNum = w;
		}
		public Wave(){
			lv1Count = 0;
			lv2Count = 0;
			lv3Count = 0;
			bossCount = 0;
			totalCount = lv1Count + lv2Count + lv3Count + bossCount;
			waveNum = 0;
		}
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	//=====Start Variables=========//
	GameObject temp;
	void Start(){
		enemyList = new List<GameObject>();
		enemyWaitingRoom = new List<GameObject>();
		waveList = new List<Wave> ();
		currentGameState = GameState.WaveInactive;

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
		UpdateWaveDisplay (waveNum+1);
	}

	void Update(){

		if (!spawnerActive)
			return;

		StackFailsafe ();

		if (currentGameState == GameState.WaveActive && enemyWaitingRoom.Count > 0) {
			if (countdown <= 0f) {
				SpawnEnemy ();
				countdown = timeBetweenEnemies;
			}
			countdown -= Time.deltaTime;
		}

		if (currentGameState == GameState.WaveActive && enemyList.Count < 1 && enemyWaitingRoom.Count < 1) {
			EndWave ();
		}


	}
	public void StartNextWave(){
		GameController.instance.turretTrayRef.ToggleTray ();
		PopulateEnemyWaitingRoom ();
		currentGameState = GameState.WaveActive;
	}
	void EndWave(){
		GameController.instance.turretTrayRef.ToggleTray ();
		currentGameState = GameState.WaveInactive;
		UpdateWaveDisplay (++waveNum + 1);
	}
	/// <summary>
	/// Adds enemies for the current wave to the enemy waiting room from the stacks
	/// </summary>
	void PopulateEnemyWaitingRoom(){

		enemyList.Clear ();

		for(int j = 0; j < waveList[waveNum].lv1Count; j++)
			enemyWaitingRoom.Add (lv1EnemyStack.Pop());

		for(int j = 0; j < waveList[waveNum].lv2Count; j++)
			enemyWaitingRoom.Add (lv2EnemyStack.Pop());
		
		for(int j = 0; j < waveList[waveNum].lv3Count; j++)
			enemyWaitingRoom.Add (lv3EnemyStack.Pop());

		for(int j = 0; j < waveList[waveNum].bossCount; j++)
			enemyWaitingRoom.Add (bossEnemyStack.Pop());
			
		
	}
	/// <summary>
	/// Updates the wave number text.
	/// </summary>
	/// <param name="w">Wave number</param>
	public void UpdateWaveDisplay(int w){
		waveNumText.text = "Wave: " + w;
	}

	IEnumerator SpawnSubWave(){
		//while (enemyWaitingRoom.Count > 0) {
			SpawnEnemy ();
			yield return new WaitForSeconds (2f);
		//}

		waveNum++;
	}

	GameObject tempEnemy;
	void SpawnEnemy(){

		tempEnemy = enemyWaitingRoom [Random.Range (0, enemyWaitingRoom.Count)];

		tempEnemy.transform.position = spawnPoint.position;
		tempEnemy.transform.rotation = spawnPoint.rotation;
		tempEnemy.SetActive (true);

		enemyList.Add (tempEnemy);
		//dont know if this will work
		enemyWaitingRoom.Remove (tempEnemy);
		//Instantiate (lv1EnemyPrefab, spawnPoint.position, spawnPoint.rotation)
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
