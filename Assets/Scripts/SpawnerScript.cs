using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

	public float timeBetweenWaves = 5f;
	private float countdown = 0f;
	private int waveNum = 0;
	public GameObject enemyPrefab;
	public Transform spawnPoint;
	public List<GameObject> enemyList;
	public bool spawnerActive;

	void Start(){
		enemyList = new List<GameObject>();
	}

	void Update(){

		if (!spawnerActive)
			return;

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


	//----------Function Variables----------//

	private GameObject gameObject;

	void SpawnEnemy(){
		enemyList.Add (Instantiate (enemyPrefab, spawnPoint.position, spawnPoint.rotation));
	}

}
