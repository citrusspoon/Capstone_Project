using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashcardManager : MonoBehaviour {

	public static FlashcardManager instance = null;

	public List<Flashcard> currentFlashcardList;


	public class Flashcard{
		string question;
		string answer;

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
