using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

	public float timeBetweenWaves = 5f;
	private float countdown = 2f;
	private int waveNum = 0;
	public GameObject enemyPrefab;
	public Transform spawnPoint;

	void Update(){

		if(countdown <= 0f){
			StartCoroutine ("SpawnWave");
			countdown = timeBetweenWaves;
		}
		countdown -= Time.deltaTime;
	}

	IEnumerator SpawnWave(){
		for (int i = 0; i < waveNum; i++) {
			SpawnEnemy ();
			yield return new WaitForSeconds (1f);
		}

		waveNum++;
	}

	void SpawnEnemy(){

		Instantiate (enemyPrefab, spawnPoint.position, spawnPoint.rotation);
	}

}
