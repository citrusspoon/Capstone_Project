using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScript : MonoBehaviour {

	// User Inputs
	public float amplitude = 0.5f;
	public float frequency = 1f;
	public bool floating = true;
	public LogoType type;
	private SpriteRenderer spriteRenderer;
	public Sprite[] sprites = new Sprite[3];
	public float timeBetweenSprites = 1f;
	private float countdown = 0f;

	// Position Storage Variables
	Vector3 posOffset = new Vector3 ();
	Vector3 tempPos = new Vector3 ();

	// Use this for initialization
	void Start () {
		// Store the starting position & rotation of the object
		posOffset = transform.position;
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		// Float up/down with a Sin()
		if (floating && type != LogoType.Import) {
			tempPos = posOffset;
			tempPos.z += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

			transform.position = tempPos;
		}

		if (type == LogoType.Import) {

			if (countdown <= 0f) {
				Animate ();
				countdown = timeBetweenSprites;
			}
			countdown -= Time.deltaTime;
		}

	}
	int spriteIndex = 0;

	void Animate()
	{
		spriteRenderer.sprite = sprites[spriteIndex++];
		if (spriteIndex > 2)
			spriteIndex = 0;	
	}
}
