using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TargetingMode {First, Last, Closest, Furthest};
public enum TurretType {Basic, Slow, Rocket, FRPowerup, RangePowerup, Destroy, Guard};
public enum GameState {WaveActive, WaveInactive};
public enum WindowState {Game, ImportMenu, MainMenu};

public class GameController : MonoBehaviour {

	public static GameController instance = null;

	[Header("References")]
	public SpawnerScript spawnerScriptRef;
	public FlashcardTray flashcardTrayRef;
	public TouchManager touchManagerRef;
	public TurretManager turretManagerRef;
	public TurretTray turretTrayRef;
	public CameraController cameraRef;

	[Header("Other")]
	//TODO: replace with non-ui button
	public Button startButton;

	//private bool toggling = false;
	//public float traySpeed = 10f;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		Setup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Setup(){
		UIManager.instance.SetAllDisplaysActive (false);
		ChangeGameWindow (WindowState.MainMenu);
	}

	/// <summary>
	/// Starts the game from the main menu.
	/// </summary>
	public void StartGame(){
		startButton.gameObject.SetActive (false);
		ChangeGameWindow (WindowState.ImportMenu);
	}
	public void Ready (){
		ChangeGameWindow (WindowState.Game);
	}

	public void ChangeGameWindow(WindowState w){

		UIManager.instance.SetWindowDisplaysActive (cameraRef.currentWindow, false);
		switch (w) {
		case WindowState.MainMenu:
			cameraRef.SetTo (WindowState.MainMenu);
			break;
		case WindowState.ImportMenu:
			cameraRef.SetTo (WindowState.ImportMenu);
			break;
		case WindowState.Game:
			cameraRef.SetTo (WindowState.Game);
			break;
		}
		UIManager.instance.SetWindowDisplaysActive (w, true);

	}
}
