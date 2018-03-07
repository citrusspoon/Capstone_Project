using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlashcardSet {

	public Flashcard[] terms;
	public int http_code;

	public string ToString(){
		string s = "";
		for (int i = 0; i < terms.Length; i++)
			s += terms [i].ToString (); 
		return s;
	}
}
