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

	public GameObject[] choiceButtons = new GameObject[3];

	public Flashcard currentCard;

	public class Flashcard{
		public string question;
		public string answer;

		public Flashcard(string q, string a){
			question = q;
			answer = a;
		}


	}

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
		//test stuff
		PopulateFlashcardList ();
		NewCard(currentFlashcardList [4]);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Select(int index){
		TouchManager.instance.choiceSelectionMade = true;
		//if correct answer is selected
		if (choices [index] == currentCard.answer) {
			//turn button green
			choiceButtons [index].GetComponent<Renderer> ().material.color = Color.green;
			//pause for 1 second
			//add resources
			//load new card
		} else {
			//turn button red
			choiceButtons [index].GetComponent<Renderer> ().material.color = Color.red;
			//turn correct button green
			for (int i = 0; i < choices.Count; i++)
				if(choices [i] == currentCard.answer)
					choiceButtons [i].GetComponent<Renderer> ().material.color = Color.green;
			//pause for one second
			//load new card
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
				choicePool.Add (currentFlashcardList [i].answer);
		}
		r = Random.Range(0, choicePool.Count);
		choices.Add (c.answer);
	
		choices.Add (choicePool[r]);

		choicePool.RemoveAt (r);
		r = Random.Range(0, choicePool.Count);

		choices.Add (choicePool[r]);

		r = Random.Range (0, choices.Count);

		//TODO: better shuffle choices

		swapChoice = choices [0];
		choices [0] = choices [r];
		choices [r] = swapChoice;

		cardTextMesh.text = c.question;
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


}
