using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashcardManager : MonoBehaviour {

	public static FlashcardManager instance = null;

	public List<Flashcard> currentFlashcardList;
	public TextMeshPro cardTextMesh;
	public TextMeshPro choice0TextMesh;
	public TextMeshPro choice1TextMesh;
	public TextMeshPro choice2TextMesh;

	public int correctChain;
	public float baseManaGain = 3f;

	public GameObject[] choiceButtons = new GameObject[3];

	public Flashcard currentCard;
	/*
	[System.Serializable]
	public class Flashcard{
		public string term;
		public string definition;

		public Flashcard(string t, string d){
			term = t;
			definition = d;
		}
		public string ToString(){
			return "Term: " + term + "\nDef: " + definition;
		}
	}

	[System.Serializable]
	public class FlashcardSet{
		public Flashcard[] terms;

		public string ToString(){
			string s = "";
			for (int i = 0; i < terms.Length; i++)
				s += terms [i].ToString (); 
			return s;
		}
	}
	*/

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
		correctChain = 0;
		//test stuff
		PopulateFlashcardList ();
		NewCard(currentFlashcardList [4]);

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
	void PopulateFlashcardList(){
		//test stuff
		currentFlashcardList.Clear();

		currentFlashcardList.Add (new Flashcard("What color is the ocean?", "Blue"));
		currentFlashcardList.Add (new Flashcard("How old is Flandre Scarlet?", "495"));
		currentFlashcardList.Add (new Flashcard("What does static mean?", "Something new every time"));
		currentFlashcardList.Add (new Flashcard("What?", "Yes"));
		currentFlashcardList.Add (new Flashcard("Who is considered the father of computers, and is also a mech dude with a steam hammer?", "Charles Babbage"));
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
