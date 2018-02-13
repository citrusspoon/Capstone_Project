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
		cardTextMesh.text = currentFlashcardList [4].question;
		choice1TextMesh.text = currentFlashcardList [4].answer;
		choice2TextMesh.text = currentFlashcardList [3].answer;
		choice3TextMesh.text = currentFlashcardList [2].answer;
	}
	
	// Update is called once per frame
	void Update () {
		
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
