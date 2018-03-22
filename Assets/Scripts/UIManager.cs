using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;



	[Header("Main Menu Elements")]
	public Button startButton;

	[Header("Game Elements")]
	public GameObject healthDisplay;
	public GameObject manaDisplay;
	public GameObject waveDisplay;

	[Header("Import Menu Elements")]
	public GameObject importElementsRoot;
	public GameObject recentSetsImportButton;
	public GameObject loadedSets;
	public GameObject IDElements;
	public GameObject recentSetsDropdown;

	[Header("Other Elements")]
	public GameObject testDisplay;


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
	public void SetMainMenuElementsActive(bool b){
		startButton.gameObject.SetActive (b);
	}
	public void SetImportMenuElementsActive(bool b){
		importElementsRoot.SetActive (b);
	}
	public void SetGameElementsActive(bool b){
		healthDisplay.SetActive (b);
		manaDisplay.SetActive (b);
		waveDisplay.SetActive (b);
	}
}
