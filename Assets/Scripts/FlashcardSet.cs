using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlashcardSet {

	public Flashcard[] terms;
	public int http_code;
	public int id;
	public string title;
	public bool swapped = false;
	public string jsonString;

	public string ToString(){
		string s = "";
		for (int i = 0; i < terms.Length; i++)
			s += terms [i].ToString (); 
		return s;
	}

	public void SwapFlashcardElements(bool marked){

		if (marked && swapped || !marked && !swapped)
			return;

		string temp;
		for(int i = 0; i < terms.Length; i++){
			temp = terms [i].term;
			terms [i].term = terms [i].definition;
			terms [i].definition = temp;
		}
		swapped = !swapped;
	}
}
