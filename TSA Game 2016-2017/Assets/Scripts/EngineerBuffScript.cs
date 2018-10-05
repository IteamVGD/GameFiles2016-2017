using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EngineerBuffScript : MonoBehaviour {

    public List<GameObject> TowersInRange;


    public int timer;
	public bool firstEngineerBuff;
	public bool secondEngineerBuff;
	public bool thirdEngineerBuff;
	public bool fourthEngineerBuff;

  

    void Start()
    {
        timer = 50;
		firstEngineerBuff = true;
		secondEngineerBuff = true;
		thirdEngineerBuff = true;
		fourthEngineerBuff = true;
    }

    void Update()
    {
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
        foreach (GameObject obj in TowersInRange)
        {
            if(obj == null)
            {
                TowersInRange.Remove(obj);
            }
           
        }
    }

    void FixedUpdate()
    {
		foreach (GameObject tower in TowersInRange) 
		{
			if (tower == null) 
			{
				TowersInRange.Remove (tower);
			}
		}
            foreach (GameObject tower in TowersInRange)
            {
                if (tower.transform.GetComponent<TowerController>().canBeEffectedByEngineer == true)
                {
				if (gameObject.transform.parent.transform.GetChild(0).gameObject.transform.GetComponent<TowerController>().towerLevel >= 0 && firstEngineerBuff == true)
                    {
						tower.transform.GetComponent<EntityStats> ().attackDamage += tower.transform.GetComponent<EntityStats>().attackDamage / 10; //lvl 0 = starting lvl = 10% more damage
						firstEngineerBuff = false;
                    }
				if (gameObject.transform.parent.transform.GetChild(0).gameObject.transform.GetComponent<TowerController>().towerLevel >= 1 && secondEngineerBuff == true) //lvl 1 = 20% more damage
                    {
                        tower.transform.GetComponent<EntityStats>().attackDamage += tower.transform.GetComponent<EntityStats>().attackDamage / 5;
						secondEngineerBuff = false;
                    }
				if (gameObject.transform.parent.transform.GetChild(0).gameObject.transform.GetComponent<TowerController>().towerLevel >= 2 && thirdEngineerBuff == true) //lvl 2 = 20% cheaper upgrades
                    {
                        tower.gameObject.transform.GetComponent<TowerController>().towerUpgrade1Cost -= tower.gameObject.transform.GetComponent<TowerController>().towerUpgrade1Cost / 5; //20% cheaper
                        tower.gameObject.transform.GetComponent<TowerController>().towerUpgrade2Cost -= tower.gameObject.transform.GetComponent<TowerController>().towerUpgrade2Cost / 5;  // \/
                        tower.gameObject.transform.GetComponent<TowerController>().towerUpgrade3Cost -= tower.gameObject.transform.GetComponent<TowerController>().towerUpgrade3Cost / 5; //  \/
						thirdEngineerBuff = false;
                    }
				if (gameObject.transform.parent.transform.GetChild(0).gameObject.transform.GetComponent<TowerController>().towerLevel >= 3 && fourthEngineerBuff == true) //lvl 3 = 30% faster fire rate
                    {
                        tower.transform.GetComponent<EntityStats>().coolDown -= tower.transform.GetComponent<EntityStats>().coolDown / 3;
                        tower.transform.GetComponent<TowerController>().localCoolDown = tower.transform.GetComponent<EntityStats>().coolDown;
						fourthEngineerBuff = false;

                    }

                }
            }
        }
    void OnMouseDown()
    {
        if (gameObject.transform.parent.transform.GetChild(0).transform.GetComponent<TowerController>().canBeSelected == true && gameObject.transform.parent.transform.GetChild(0).transform.GetComponent<TowerController>().isSynergy == false)
        {
            GameController.setUpgradeLevelText(gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject);
            WaveGen.callWaveButtonStatic.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if(TowersInRange.Contains(coll.gameObject) == false && coll.gameObject.layer == 8)
        {
            TowersInRange.Add(coll.gameObject);
			coll.gameObject.transform.GetComponent<TowerController> ().canBeEffectedByEngineer = true;
        }
    }
}
