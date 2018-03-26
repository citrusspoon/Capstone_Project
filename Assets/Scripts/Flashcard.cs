using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Flashcard {
	
	public string term;
	public string definition;
	public int misses;

	public Flashcard(string t, string d){
		term = t;
		definition = d;
		misses = 0;
	}
	public string ToString(){
		return "Term: " + term + "\nDef: " + definition;
	}
}
