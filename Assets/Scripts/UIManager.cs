using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;



	[Header("Main Menu Elements")]
	public Button startButton;

	[Header("Game Elements")]
	public GameObject gameElementsRoot;
	public GameObject healthDisplay;
	public GameObject manaDisplay;
	public GameObject waveDisplay;
	public Button reimportButton;
	public Button advanceWaveButton;

	[Header("Import Menu Elements")]
	public GameObject importElementsRoot;
	public GameObject recentSetsImportButton;
	public GameObject loadedSets;
	public GameObject IDElements;
	public GameObject recentSetsDropdown;

	[Header("Game Over Elements")]
	public GameObject gameOverElementsRoot;
	public TextMeshPro gameOverText;
	public TextMeshPro statsText;
	public Button restartButton;

	[Header("Main Menu Elements")]
	public GameObject startElementsRoot;


	[Header("Other Elements")]
	public GameObject testDisplay;
	public Text chainDisplay;




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
		
	}

	public void SetTestDisplayActive(bool b){
		testDisplay.SetActive (b);
	}
	public void SetAllDisplaysActive(bool b){
		SetMainMenuElementsActive (b);
		SetImportMenuElementsActive (b);
		SetGameElementsActive (b);
		SetTestDisplayActive (b);
		SetGameOverElementsActive (b);
	}
	/// <summary>
	/// Sets the active state of all UI elements specific to the game window
	/// </summary>
	/// <param name="w">Game window</param>
	/// <param name="b"> The elements active state</param>
	public void SetWindowDisplaysActive(WindowState w, bool b){
		switch (w) {
		case WindowState.MainMenu:
			SetMainMenuElementsActive (b);
			break;
		case WindowState.ImportMenu:
			SetImportMenuElementsActive (b);
			break;
		case WindowState.Game:
			SetGameElementsActive (b);
			break;
		}
	}

	public void UpdateGameStateDependentElments(GameState s){
		switch (s) {
		case GameState.WaveActive:
			reimportButton.gameObject.SetActive (false);
			advanceWaveButton.gameObject.SetActive (false);
			break;
		case GameState.WaveInactive:
			reimportButton.gameObject.SetActive (true);
			advanceWaveButton.gameObject.SetActive (true);
			break;
		}
	}
	public void SetImportMenuElementsActive(bool b){
		importElementsRoot.SetActive (b);
	}
	public void SetMainMenuElementsActive(bool b){
		startElementsRoot.SetActive (b);
	}
	public void SetGameElementsActive(bool b){
		gameElementsRoot.SetActive (b);

		/*healthDisplay.SetActive (b);
		manaDisplay.SetActive (b);
		waveDisplay.SetActive (b);*/
	}
	public void SetGameOverElementsActive(bool b){
		gameOverElementsRoot.SetActive (b);
		restartButton.gameObject.SetActive (b);
	}
	public void UpdateGameOverStatsElement(){
		statsText.text = "Longest Chain: " + StatsManager.instance.longestChain + "\nMost Missed Cards: \n" + StatsManager.instance.MostMissedCardsToString();
	}
	public void UpdateChainDisplay(int c){
		chainDisplay.text = "Chain: " + c;
	}
}
