using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingMode {First, Last, Closest, Furthest};
public enum TurretType {Basic, Slow, Rocket, FRPowerup, RangePowerup};
public enum GameState {WaveActive, WaveInactive};

public class GameController : MonoBehaviour {

	public static GameController instance = null;

	public SpawnerScript spawnerScriptRef;
	public FlashcardTray flashcardTrayRef;
	public TouchManager touchManagerRef;
	public TurretManager turretManagerRef;
	public TurretTray turretTrayRef;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//print(spawnerScriptRef.enemyList.Count);
	}
}
