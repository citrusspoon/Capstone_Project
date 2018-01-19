using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour {

	[Header("Enemy Prefabs")]
	public GameObject enemyLV1;
	public GameObject enemyLV2;
	[Header("Other")]
	public Transform spawnPoint;

	void Start(){

	}

	void Update(){


	}

	public void SpawnEnemy(GameObject enemy){
		GameController.instance.spawnerScriptRef.enemyList.Add (Instantiate (enemy, spawnPoint.position, spawnPoint.rotation));
	}
}
