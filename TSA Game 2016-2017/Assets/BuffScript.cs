using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffScript : MonoBehaviour {
	
	public List<GameObject> EnemiesInRange;
	public int timer;

	// Use this for initialization
	void Start () {
		timer = 50;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer <= 0 && timer >= -3)
		{

			gameObject.transform.GetComponent<CircleCollider2D>().enabled = true;
		}
		if (timer > -5)
		{
			timer -= 1;
		}
		else
		{
			gameObject.transform.GetComponent<CircleCollider2D>().enabled = false;
			timer = 50;
		}
		foreach (GameObject obj in EnemiesInRange)
		{
			if(obj == null)
			{
				EnemiesInRange.Remove(obj);
			}

		}
	
	}

	public void buff()
	{
		foreach (GameObject enemy in EnemiesInRange) 
		{
			enemy.gameObject.transform.GetComponent<EntityStats> ().movementSpeed = enemy.gameObject.transform.GetComponent<EntityStats> ().movementSpeed + 1;
		}

	}
		

	void OnTriggerStay2D(Collider2D coll)
	{
		if(EnemiesInRange.Contains(coll.gameObject) == false && coll.gameObject.layer == 10)
		{
			EnemiesInRange.Add(coll.gameObject);

		}
	}
}
