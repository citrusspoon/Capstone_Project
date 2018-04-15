using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance = null;
	public AudioSource mainMusic;
	public AudioSource menuMusic;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void PlayMainMusic(){
		menuMusic.Stop ();
		mainMusic.Play ();
	}

}
