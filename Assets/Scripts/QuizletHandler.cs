using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuizletHandler : MonoBehaviour {

	private const string API_KEY = "Hd5x4wJFkAuDBMXb2YgyzC";
	private const string CLIENT_ID = "6KTW6QMZ6M";
	private string currentSetID;
	private string currentURL;
	public GameObject loadingIcon;
	public TextMeshPro testText;
	public static QuizletHandler instance = null;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		//Request ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UpdateSetID(Text id){
		currentSetID = id.text;
		UpdateURL ();
	}
	private void UpdateURL(){
		currentURL = "https://api.quizlet.com/2.0/sets/" + currentSetID + "?client_id=" + CLIENT_ID;
		Request (currentURL);
	}
	private const string testURL = "https://api.quizlet.com/2.0/sets/125969193?client_id=6KTW6QMZ6M";

	public void Request(string reqURL){
		loadingIcon.SetActive (true);
		WWW request = new WWW (reqURL);
		StartCoroutine (OnResponse(request));
	}
	IEnumerator OnResponse(WWW req){
		yield return req;
		print ("done");
		loadingIcon.SetActive (false);
		FlashcardSet f = JsonUtility.FromJson<FlashcardSet> (req.text);
		testText.text = f.ToString();
		//testText.text = req.text;
	}
}
/*
		Application: 
		FlashcardTD

		Your Quizlet Client ID (used for public and user access):
		6KTW6QMZ6M

		Your Secret Key (used for user authentication only):
		Hd5x4wJFkAuDBMXb2YgyzC (reset)
		
		Your Redirect URI:
		flashcardtd://after_oauth (double-click to edit) 
*/