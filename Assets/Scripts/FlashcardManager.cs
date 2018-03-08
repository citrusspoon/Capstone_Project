using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashcardManager : MonoBehaviour {

	public static FlashcardManager instance = null;

	public List<Flashcard> currentFlashcardList;
	public List<FlashcardSet> loadedFlashcardSets;
	public TextMeshPro cardTextMesh;
	public TextMeshPro choice0TextMesh;
	public TextMeshPro choice1TextMesh;
	public TextMeshPro choice2TextMesh;

	public TextMeshProUGUI[] loadedSetsMenuText; 
	public Button[] removeSetButtons;
	public Toggle[] swapToggles;

	public int correctChain;
	public float baseManaGain = 3f;

	public GameObject[] choiceButtons = new GameObject[3];

	public Flashcard currentCard;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		currentFlashcardList = new List<Flashcard> ();
		loadedFlashcardSets = new List<FlashcardSet> ();
		correctChain = 0;
		//TODO: add an initial set that is independent of quizlet


		//test stuff
		//PopulateFlashcardList ();
		//NewCard(currentFlashcardList [4]);

	}

	//TODO: display correct chain on screen

	// Update is called once per frame
	void Update () {
		
	}

	public void Select(int index){
		TouchManager.instance.choiceSelectionMade = true;
		//if correct answer is selected
		if (choices [index] == currentCard.definition) {
			//turn button green
			choiceButtons [index].GetComponent<Renderer> ().material.color = Color.green;
			ResourceManager.instance.ChangeMana (baseManaGain + correctChain*2);
			correctChain++;
		} else {
			//turn button red
			choiceButtons [index].GetComponent<Renderer> ().material.color = Color.red;
			//turn correct button green
			for (int i = 0; i < choices.Count; i++)
				if(choices [i] == currentCard.definition)
					choiceButtons [i].GetComponent<Renderer> ().material.color = Color.green;
			if (guardPossible ()) {
				//TODO: some kind of guard effect/popup
			} else {
				correctChain = 0;
			}
				
		}
		StartCoroutine (PauseAndLoadNextCard());

	}

	IEnumerator PauseAndLoadNextCard(){
		yield return new WaitForSeconds (1.0f);
		ResetChoiceColors ();
		NewCard (currentFlashcardList[Random.Range(0,currentFlashcardList.Count)]);
		TouchManager.instance.choiceSelectionMade = false;

	}

	void ResetChoiceColors(){
		for (int i = 0; i < choiceButtons.Length; i++)
			choiceButtons [i].GetComponent<Renderer> ().material.color = Color.white;
	}

	//=======New Card Variables=========//
	//TODO: possibly swap term and definition?
	List<string> choices = new List<string> ();
	public List<string> choicePool = new List<string> (); 
	int r;
	string swapChoice;
	/// <summary>
	/// Loads a new card and answer choices onto the tray. Choices consist of the answer to the card, and two randomly chosen answers from the other cards.
	/// </summary>
	/// <param name="c">Flashcard to load.</param>
	public void NewCard(Flashcard c){
		choices.Clear ();
		choicePool.Clear ();
		for (int i = 0; i < currentFlashcardList.Count; i++) {
			if(currentFlashcardList[i] != c)
				choicePool.Add (currentFlashcardList [i].definition);
		}
		r = Random.Range(0, choicePool.Count);
		choices.Add (c.definition);
	
		choices.Add (choicePool[r]);

		choicePool.RemoveAt (r);
		r = Random.Range(0, choicePool.Count);

		choices.Add (choicePool[r]);

		r = Random.Range (0, choices.Count);

		//TODO: better shuffle choices

		swapChoice = choices [0];
		choices [0] = choices [r];
		choices [r] = swapChoice;

		cardTextMesh.text = c.term;
		choice0TextMesh.text = choices[0];
		choice1TextMesh.text = choices[1];
		choice2TextMesh.text = choices[2];
		currentCard = c;
	}
	/*
	public void PopulateFlashcardList(FlashcardSet f){
		//test stuff
		currentFlashcardList.Clear();
		for (int i = 0; i < f.terms.Length; i++)
			currentFlashcardList.Add (f.terms [i]);

		NewCard (currentFlashcardList[Random.Range(0,currentFlashcardList.Count)]);
			
	
	}*/

	/// <summary>
	/// Removes the specified set. Will not allow there to be no loaded sets.
	/// </summary>
	/// <param name="index">Index of set in menu</param>
	public void RemoveFlashcardSet(int index){
		if (loadedSetsMenuText [index].text != "Empty" && loadedFlashcardSets.Count > 1) {
			loadedFlashcardSets.RemoveAt (index);
			UpdateLoadedSetsUIElements (index);
		}
	}
	public void AddFlashcardSet(FlashcardSet f){
		for (int i = 0; i < loadedFlashcardSets.Count; i++) {
			if (f.id == loadedFlashcardSets [i].id)
				return;
		}
		loadedFlashcardSets.Add (f);
		swapToggles [loadedFlashcardSets.Count - 1].isOn = false;
		UpdateLoadedSetsUIElements ();
	}
	/// <summary>
	/// Swaps the term and definitions of all cards in the set.
	/// </summary>
	/// <param name="index">Index of loaded flashcard set.</param>
	public void SwapFlashcardSetElements(int index){
		if (loadedSetsMenuText [index].text == "Empty")
			return;
		loadedFlashcardSets [index].SwapFlashcardElements (swapToggles[index].isOn);
		PopulateFlashcardList ();
	}
	/// <summary>
	/// Updates the text for the currently loaded sets in the quizlet menu.
	/// </summary>
	void UpdateLoadedSetsUIElements(){
		for (int i = 0; i < loadedFlashcardSets.Count; i++) {
				loadedSetsMenuText [i].text = loadedFlashcardSets [i].title;
		}
		PopulateFlashcardList ();
	}
	/// <summary>
	/// Updates the text for the currently loaded sets in the quizlet menu. Sets recently removed set to "Empty".
	/// </summary>
	/// <param name="remIndex">Index of set recently removed</param>
	void UpdateLoadedSetsUIElements(int remIndex){
		loadedSetsMenuText [remIndex].text = "Empty";
		int j = 0;
		//sets text to the title of the flashcard set
		for (int i = 0; i < loadedFlashcardSets.Count; i++) {
			loadedSetsMenuText [i].text = loadedFlashcardSets [i].title;
			swapToggles [i].isOn = loadedFlashcardSets [i].swapped;
			j++;
		}
		//sets remaining texts to empty
		while (j < loadedSetsMenuText.Length) {
			loadedSetsMenuText [j].text = "Empty";
			j++;
		}
		//sets swap toggle to false for all empty items
		for (int k = 0; k < loadedSetsMenuText.Length; k++) {
			if (loadedSetsMenuText [k].text == "Empty")
				swapToggles [k].isOn = false;
		}

		PopulateFlashcardList ();
	}
	void PopulateFlashcardList(){
		//test stuff
		currentFlashcardList.Clear();

		for (int i = 0; i < loadedFlashcardSets.Count; i++) {
			for (int j = 0; j < loadedFlashcardSets [i].terms.Length; j++) {
				currentFlashcardList.Add (loadedFlashcardSets[i].terms[j]);
			}
		}
		if(currentFlashcardList.Count > 0)
			NewCard (currentFlashcardList[0]);

		/*
		currentFlashcardList.Add (new Flashcard("What color is the ocean?", "Blue"));
		currentFlashcardList.Add (new Flashcard("How old is Flandre Scarlet?", "495"));
		currentFlashcardList.Add (new Flashcard("What does static mean?", "Something new every time"));
		currentFlashcardList.Add (new Flashcard("What?", "Yes"));
		currentFlashcardList.Add (new Flashcard("Who is considered the father of computers, and is also a mech dude with a steam hammer?", "Charles Babbage"));
		*/
	}
	/// <summary>
	/// Iterates through currently placed guard turrets, and if one is unbroken, returns true and breaks the turret.
	/// </summary>
	/// <returns><c>true</c>, if guard is possible, <c>false</c> otherwise.</returns>
	bool guardPossible(){
		for(int i = 0; i < TurretManager.instance.placedGuardTurrets.Count; i++){
			if (!TurretManager.instance.placedGuardTurrets [i].GetComponent<GuardTurret> ().broken) {
				TurretManager.instance.placedGuardTurrets [i].GetComponent<GuardTurret> ().breakTurret();
				return true;
			}	
		}
		return false;
	}


}
