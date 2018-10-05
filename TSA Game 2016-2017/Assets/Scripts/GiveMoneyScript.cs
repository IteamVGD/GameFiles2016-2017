using UnityEngine;
using System.Collections;

public class GiveMoneyScript : MonoBehaviour {

	public bool droppedMoney;

	// Use this for initialization
	void Start () {
		droppedMoney = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		/*if (gameObject.transform.GetComponent<EntityStats> ().health <= 20 && droppedMoney == false) 
		{
			GameController.money += gameObject.transform.GetComponent<EntityStats> ().enemyDropMoney;
			droppedMoney = true;
		}

		if (gameObject.transform.GetComponent<EntityStats> ().health <= 0 ) 
		{
			Destroy(gameObject);
		}*/
	}
}
