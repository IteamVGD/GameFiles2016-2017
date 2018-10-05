using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public GameObject towerSpawnedGlobal;
    public bool towerBeingSpawned;
    public static bool staticTowerBeingSpawned;

    public int test;

    public bool towerBeingSpawnedXCheck;
    public bool towerBeingSpawnedYCheck;
    public GameObject towerInWay;
    public bool towerInWayXCheck;
    public bool towerInWayYCheck;

    public bool testBool;

    public List<GameObject> towerSpawnButtons;

    // Use this for initialization
    void Start()
    {
        towerBeingSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        staticTowerBeingSpawned = towerBeingSpawned;
        if(towerBeingSpawned == true)
        {
            testBool = true;
            towerSpawnedGlobal.transform.position = new Vector3((int)GameController.mousePos.x, (int)GameController.mousePos.y, 1);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && towerBeingSpawned == true)
        {
            if(towerSpawnedGlobal.transform.GetChild(0).gameObject.transform.GetComponent<TowerController>().canBePlaced == true)
            {
                testBool = false;
                //towerSpawnedGlobal.GetComponent<TowerController>().testForSynergy(); Broken, neeeds to be fixed (breaks tower spawning if un-commented)
                towerSpawnedGlobal.transform.GetChild(0).gameObject.GetComponent<TowerController>().placeTower();
                if(towerSpawnedGlobal.transform.GetChild(0).gameObject.transform.tag == "FreezeTower")
                {
                    towerSpawnedGlobal.transform.GetChild(2).gameObject.SetActive(true);
                }
                towerSpawnedGlobal = null;
                enableTowerSpawnButtons();
                towerBeingSpawned = false;
                enableAllSynergyTestChilds();
            }
        }

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (GameController.OptionsMenuParentObj.transform.GetChild(0).gameObject.activeInHierarchy == false)
                {
                    GameController.OptionsMenuParentObj.transform.GetChild(0).gameObject.SetActive(true);
                    GameController.GameStuffParentObj.SetActive(false);
                }
                else
                {
                    GameController.OptionsMenuParentObj.transform.GetChild(0).gameObject.SetActive(false);
                    GameController.GameStuffParentObj.SetActive(true);
                }
            }
        }
    }	

    public void spawnTowerMachineGun()
    {
        if(GameController.money >= 250)
        {
            GameObject towerSpawned = GameObject.Instantiate(GameController.machineGunPrefabStatic, transform.position, transform.rotation) as GameObject;
            disableAllSynergyTestChilds();
            towerSpawnedGlobal = towerSpawned.gameObject;
            towerBeingSpawned = true;
            towerSpawned.transform.GetChild(0).GetComponent<EntityStats>().canShoot = false;
            towerSpawned.transform.GetChild(1).gameObject.SetActive(true);
            GameController.money -= 250;
            disableTowerSpawnButtons();
        }
    }

	public void spawnTowerSniper()
	{
		if(GameController.money >= 600)
		{
            GameObject towerSpawned = GameObject.Instantiate(GameController.sniperPrefabStatic, transform.position, transform.rotation) as GameObject;
            disableAllSynergyTestChilds();
            towerSpawnedGlobal = towerSpawned.gameObject;
			towerBeingSpawned = true;
			towerSpawned.transform.GetChild(0).GetComponent<EntityStats>().canShoot = false;
            towerSpawned.transform.GetChild(1).gameObject.SetActive(true);
            GameController.money -= 600;
            disableTowerSpawnButtons();
            towerBeingSpawned = true;
        }
	}

	public void spawnTowerFlameThrower()
	{
		if(GameController.money >= 350)
		{
            GameObject towerSpawned = GameObject.Instantiate(GameController.FlameThrowerPrefabStatic, transform.position, transform.rotation) as GameObject;
            disableAllSynergyTestChilds();
            towerSpawnedGlobal = towerSpawned.gameObject;
			towerBeingSpawned = true;
			towerSpawned.transform.GetChild(0).GetComponent<EntityStats >().canShoot = false;
            towerSpawned.transform.GetChild(1).gameObject.SetActive(true);
            GameController.money -= 350;
            disableTowerSpawnButtons();
        }
	}

	public void spawnTowerFreeze()
	{
		if(GameController.money >= 400)
		{
            GameObject towerSpawned = GameObject.Instantiate(GameController.FreezePrefabStatic, transform.position, transform.rotation) as GameObject;
            disableAllSynergyTestChilds();
            towerSpawnedGlobal = towerSpawned.gameObject;
			towerBeingSpawned = true;
			towerSpawned.transform.GetChild(0).GetComponent<EntityStats >().canShoot = false;
            towerSpawned.transform.GetChild(1).gameObject.SetActive(true);
            GameController.money -= 400;
            disableTowerSpawnButtons();
        }
	}

	public void spawnTowerEngineer()
	{
		if(GameController.money >= 450)
		{
            GameObject towerSpawned = GameObject.Instantiate(GameController.EngineerTowerPrefabStatic, transform.position, transform.rotation) as GameObject;
            disableAllSynergyTestChilds();
            towerSpawnedGlobal = towerSpawned.gameObject;
			towerBeingSpawned = true;
			towerSpawned.transform.GetChild(0).GetComponent<EntityStats >().canShoot = false;
            towerSpawned.transform.GetChild(1).gameObject.SetActive(true);
            GameController.money -= 450;
            disableTowerSpawnButtons();
        }
	}

	public void spawnTowerGrenadeLauncher()
	{
		if(GameController.money >= 500)
		{
            GameObject towerSpawned = GameObject.Instantiate(GameController.GrenadeLauncherPrefabStatic, transform.position, transform.rotation) as GameObject;
            disableAllSynergyTestChilds();
            towerSpawnedGlobal = towerSpawned.gameObject;
			towerBeingSpawned = true;
			towerSpawned.transform.GetChild(0).GetComponent<EntityStats >().canShoot = false;
            towerSpawned.transform.GetChild(1).gameObject.SetActive(true);
            GameController.money -= 500;
            disableTowerSpawnButtons();
        }
	}

    void disableTowerSpawnButtons()
    {
        foreach(GameObject button in towerSpawnButtons)
        {
            button.SetActive(false);
        }
    }

    void enableTowerSpawnButtons()
    {
        foreach (GameObject button in towerSpawnButtons)
        {
            button.SetActive(true);
        }
    }

    void disableAllSynergyTestChilds()
    {
        foreach(GameObject tower in GameController.TowerList)
        {
            if(tower.transform.GetComponent<TowerController>().synergyTestChild != null)
            {
                tower.transform.GetComponent<TowerController>().synergyTestChild.SetActive(false);
                tower.transform.GetComponent<TowerController>().canBeSelected = false;
                tower.transform.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    void enableAllSynergyTestChilds()
    {
        foreach (GameObject tower in GameController.TowerList)
        {
            tower.transform.GetComponent<TowerController>().synergyTestChild.SetActive(true);
            tower.transform.GetComponent<TowerController>().canBeSelected = true;
            tower.transform.GetComponent<BoxCollider2D>().enabled = false;
            if (tower.transform.tag == "FreezeTower")
            {
                tower.transform.parent.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }
}


