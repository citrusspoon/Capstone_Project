using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

	public static StatsManager instance = null;

	public int longestChain = 0;


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
	/// <summary>
	/// Updates the longest chain value if currentChain is bigger
	/// </summary>
	/// <param name="currentChain">Current chain.</param>
	public void UpdateLongestChain(int currentChain){
		if (currentChain > longestChain)
			longestChain = currentChain;
	}
}
