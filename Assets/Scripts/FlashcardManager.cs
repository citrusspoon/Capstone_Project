using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashcardManager : MonoBehaviour {

	public static FlashcardManager instance = null;

	public List<Flashcard> currentFlashcardList;
	public TextMeshPro cardTextMesh;
	public TextMeshPro choice1TextMesh;
	public TextMeshPro choice2TextMesh;
	public TextMeshPro choice3TextMesh;


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
	//=======New Card Variables=========//
	List<string> choices = new List<string> ();
	public List<string> choicePool = new List<string> (); 
	int r;
	/// <summary>
	/// Loads a new card and answer choices onto the tray. Choices consist of the answer to the card, and two randomly chosen answers from the other cards.
	/// </summary>
	/// <param name="c">Flashcard to load.</param>
	public void NewCard(Flashcard c){
		choices.Clear ();
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

		//TODO: shuffle choices

		cardTextMesh.text = c.question;
		choice1TextMesh.text = choices[0];
		choice2TextMesh.text = choices[1];
		choice3TextMesh.text = choices[2];
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

	private Random rng = new Random();  

}
