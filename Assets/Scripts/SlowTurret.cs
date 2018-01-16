using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTurret : MonoBehaviour {

	public float range = 18f;
	public float fireRate = 1f;
	private float fireCountdown = 0f;
	//how much to reduce sspeed by
	public float slowPercent = 0.3f;
	public float power = 0f;
	public ParticleSystem slowParticles;
	private List<GameObject> enemiesInRange;

	// Use this for initialization
	void Start () {
		enemiesInRange = new List<GameObject> ();
		GetComponent<SphereCollider> ().radius = range;
	}
	
	// Update is called once per frame
	void Update () {
		print (enemiesInRange.Count);
		if (enemiesInRange.Count == 0) {//if there are no enemies in range nothing below here will run
			slowParticles.Stop();
			return;
		}
		slowParticles.Play ();


		//=========Firing=========//

		if (fireCountdown <= 0f) {
			Shoot ();
			fireCountdown = 1f / fireRate;
		}
		fireCountdown -= Time.deltaTime;	
	}

	//==========Shoot Variables========//
	private EnemyScript enemy;
	void Shoot(){
		for (int i = 0; i < enemiesInRange.Count; i++) {
			enemy = enemiesInRange [i].GetComponent<EnemyScript> ();
			if (!enemy.isSlowed) {
				enemy.Slow (slowPercent);
				enemy.ReduceHealth (power);
			}
		}
	}




	void OnTriggerEnter(Collider c){
		enemiesInRange.Add (c.gameObject);
	}

	void OnTriggerExit(Collider c){
		c.gameObject.GetComponent<EnemyScript> ().ResetSpeed ();
		enemiesInRange.Remove (c.gameObject);

	}

	//Draws the wirefram that show turret range in editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}
}
