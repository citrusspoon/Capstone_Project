using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Flashcard {
	
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
