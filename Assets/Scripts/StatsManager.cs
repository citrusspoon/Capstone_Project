using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

	public static StatsManager instance = null;

	public int longestChain = 0;
	public List<Flashcard> mostMissedCards;
	public int mostMissedCardsMaxLength = 5;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}
	// Use this for initialization
	void Start () {
		mostMissedCards = new List<Flashcard> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	/// Updates the longest chain value if currentChain is bigger
	/// </summary>
	/// <param name="currentChain">Current chain.</param>
	public void UpdateLongestChain(int currentChain){
		if (currentChain > longestChain)
			longestChain = currentChain;
	}
	Flashcard temp;
	public void UpdateMostMissedCards(Flashcard toAdd){
		if (!mostMissedCards.Contains (toAdd)) {
			mostMissedCards.Add (toAdd);
		}
		//sorts missed flashcard list
		for (int i = 0; i < mostMissedCards.Count; i++)
			for (int j = 0; j < mostMissedCards.Count - 1; j++)
				if (mostMissedCards[j].misses < mostMissedCards[j+1].misses){
					temp = mostMissedCards[j+1];
					mostMissedCards[j+1] = mostMissedCards[j];
					mostMissedCards[j] = temp;
				}       
		  
		//trims the list down to the designated max length
		while (mostMissedCards.Count > mostMissedCardsMaxLength)
			mostMissedCards.RemoveAt (mostMissedCards.Count-1);
	}
	string s = "";
	public string MostMissedCardsToString(){
		s = "";
		for (int i = 0; i < mostMissedCards.Count; i++)
			s += mostMissedCards [i].term + "\n";
		return s;
	}
}
