using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

	public static ResourceManager instance = null;

	public float playerHealth = 100f;
	public float maxPlayerHealth = 100f;
	public float mana = 100f;
	public float maxMana = 100f;
	public Slider healthBar;
	public Slider manaBar;
	public Text healthText;
	public Text manaText;



	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		ChangeHealth (0);
		ChangeMana (0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ChangeHealth(float delta){
		playerHealth += delta;
		healthBar.value = playerHealth;
		healthText.text = "Health (" + playerHealth+ "/" + maxPlayerHealth + ")";
	}
	public void ChangeMana(float delta){
		mana += delta;
		if (mana > maxMana)
			mana = maxMana;
		manaBar.value = mana;
		manaText.text = "Mana (" + mana + "/" + maxMana + ")";
	}
	public void changeMaxMana(float delta){
		maxMana += delta;
		manaBar.maxValue = maxMana;
		manaText.text = "Mana (" + mana + "/" + maxMana + ")";
	}
}
