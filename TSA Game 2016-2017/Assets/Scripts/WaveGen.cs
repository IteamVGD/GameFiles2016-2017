using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveGen : MonoBehaviour {

    public int waveNumber;
    public float waveForEntityFloat; //+0.5 For every wave, every 2 waves = +1 = new entity type
    public int waveForEntityInt; //Above in int form
    public GameObject entityToSpawn;
    public List<GameObject> spawnableEntitiesList;
    public List<GameObject> entitiesSpawned;
	public static List<GameObject> entitiesSpawnedStatic;
    public int whileLoopInt;
    public int objAmountToSpawnStat;
    public int spawnedEntitiesInt; //amountOfSpawnedEntities, not linked to spawned entities list or that lists lenght
    public int spawnabledEntitiesListLength;
    public bool generatingWave;

    //Enemy Objs (Prefabs that are in the scene) used for wave spawning
	public GameObject TestObjobject;
    public GameObject basicEnemyObj;
    public GameObject fastBasicEnemyObj;
    public GameObject disablerEnemyObj;
    public GameObject sprinterEnemyObj;
    public GameObject popperEnemyObj;
    public GameObject heavyEnemyObj;
    public GameObject shieldGenratorEnemyObj;

	public GameObject entitySpawned;

    public int spawningTimer;
    public bool spawningTimerToggle;

    public GameObject spawningPath;
    public GameObject endingPath;
    public static GameObject endingPathStatic;

    public static GameObject callWaveButtonStatic;
    public GameObject callWaveButton;

    public static int totalAmountOfEnemiesToSpawn;
    public int nonStaticTotalAmountOfEnemiesToSpawn;

    public int waveMoneyToGive;
    public bool waveMoneyToGiveBool;

    public static bool spawningTimerToggleStatic;
    public static bool spawningTimerToggleStatic2; //Used to set spawningTimerToggle to false after wave is generated, false = sets spawningTimerToggle to false, true = does nothing

    public static int spawningTimerStatic;

    public static int enemiesSpawnedInThisWave;

    // Use this for initialization
    void Start () {
        spawningTimerToggleStatic2 = true;
        spawningTimerStatic = 1;
        waveMoneyToGiveBool = false;
        callWaveButtonStatic = callWaveButton;
        totalAmountOfEnemiesToSpawn = 0;
		entitiesSpawned = new List<GameObject> ();
		entitiesSpawnedStatic = new List<GameObject> ();
        generatingWave = false;
        spawningTimerToggle = false;
        spawningTimer = 0;
        spawningPath = GameObject.FindGameObjectWithTag("StartingPath");
        endingPath = GameObject.FindGameObjectWithTag("EndingPath");
        endingPathStatic = endingPath;
        GameController.waveInProgress = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(spawningTimerStatic == 0)
        {
            spawningTimer = spawningTimerStatic;
            spawningTimerStatic = 1;
        }
        if(spawningTimerToggleStatic2 == false)
        {
            spawningTimerToggle = false;
            spawningTimerToggleStatic2 = true;
        }
        spawningTimerToggleStatic = spawningTimerToggle;
        if(waveNumber - 1 > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", waveNumber);
            GameController.highScore = PlayerPrefs.GetInt("HighScore");
            GameController.highScoreTextObj.GetComponent<Text>().text = "High Score: " + GameController.highScore;
        }
        if(waveMoneyToGiveBool == true && GameController.EnemyList.Count == 0)
        {
            GameController.money += waveMoneyToGive;
            waveMoneyToGiveBool = false;
        }
		entitiesSpawnedStatic = entitiesSpawned;
		spawnabledEntitiesListLength = spawnableEntitiesList.Count;
        nonStaticTotalAmountOfEnemiesToSpawn = totalAmountOfEnemiesToSpawn;
        if(totalAmountOfEnemiesToSpawn < entitiesSpawned.Count && GameController.aTowerIsSelected == false && spawningTimerToggle == true)
        {
            callWaveButton.SetActive(false);
        }
	}

    public void nextWave()
    {
        enemiesSpawnedInThisWave = 0;
        entitiesSpawned.Clear();
        callWaveButton.SetActive(false);
		GameController.waveInProgress = true;
        //Called by Next Wave button, initiates wave loading
        waveNumber += 1;
		setupWaveSettings ();
        waveForEntityFloat += 0.5F;
        waveForEntityInt = (int)waveForEntityFloat;
        
        //Wave Music Controll \/
		if(waveNumber >= 1 && waveNumber <= 3)
		{
				GameController.staticFirstWavesMusic.SetActive (true);
		}
		if(waveNumber >= 4 && waveNumber <= 6)
		{
			GameController.staticFirstWavesMusic.SetActive (false);
			GameController.staticFirstHalfMiddleMusic.SetActive(true);
			
		}
		if(waveNumber >= 7 && waveNumber <= 10)
		{
			GameController.staticFirstHalfMiddleMusic.SetActive(false);
			GameController.staticSecondHalfMiddleMusic.SetActive(true);
			

		}
		if(waveNumber >= 11)
		{
			GameController.staticSecondHalfMiddleMusic.SetActive(false);
			GameController.staticFinalWavesMusic.SetActive(true);
		}
    }

    public void setupNextWave()
    {
        //Runs after nextWave void
        totalAmountOfEnemiesToSpawn = 0;
        foreach(GameObject enemyType in spawnableEntitiesList)
        {
            totalAmountOfEnemiesToSpawn += enemyType.transform.GetComponent<EntityStats>().amountToSpawn;
        }
        nonStaticTotalAmountOfEnemiesToSpawn = totalAmountOfEnemiesToSpawn;
        spawnEntities();
    }

    public void spawnEntities()
    {
        if (spawnedEntitiesInt - 1 < spawnableEntitiesList.Count) //-1 because list starts at 0, not 1
        {
            entityToSpawn = spawnableEntitiesList[spawnedEntitiesInt]; //If the number of entities spawned is less that the amount of entities that can (and will be) spawned, continue running this loop (This loop = spawnEntities() + FixedUpdate()while)
            whileLoopInt = 0;
            objAmountToSpawnStat = entityToSpawn.GetComponent<EntityStats>().amountToSpawn;
            spawnedEntitiesInt += 1; //Allows loop because when this runs again at the end of FixedUpdate while loop, this will stop the loop when spawnedEntitiesInt - 1 is not < spawnableEntitiesListLenght
            generatingWave = true;
        }
    }

    public void FixedUpdate() //Runs at all times (like update, but not linked to framerate)
    {
        if (spawningTimerToggle == true)
        {
            spawningTimer += 1;
        }
        if (whileLoopInt < objAmountToSpawnStat) //Starts running when objAmountToSpawnStat gets a value in spawnEntities
        {
            spawningTimerToggle = true;
            if(spawningTimer >= 70)
            {
                whileLoopInt += 1;
                spawningTimer = 0;
				GameObject instantiatedEntity = GameObject.Instantiate(entityToSpawn, spawningPath.transform.position, transform.rotation) as GameObject	; //Spawns entity
				entitySpawned = instantiatedEntity.gameObject;
				entitiesSpawned.Add(entitySpawned);
                enemiesSpawnedInThisWave += 1;
                waveMoneyToGiveBool = true;
            }
            
        }
        if (whileLoopInt >= objAmountToSpawnStat)
        {
            spawnEntities();
            spawningTimerToggle = false;
            spawningTimer = 0;
            generatingWave = false;
        }
    }

	void setupWaveSettings()
	{
        entityToSpawn = null;
        spawnableEntitiesList.Clear();
        spawningTimer = 0;
        whileLoopInt = 0;
        spawnedEntitiesInt = 0;

        //Beggin Wave Spawning Control \/
        if (waveNumber == 1)
        {
            setBasicEnemySettings(10, 15);
        }
        if (waveNumber == 2)
        {
            GameController.planO.SetActive(false); // Plan O
            setBasicEnemySettings(7, 12);
            setFastBasicEnemySettings(5, 10);
        }
        if (waveNumber == 3)
        {
            setBasicEnemySettings(10, 15);
            setFastBasicEnemySettings(10, 15);
        }
        if (waveNumber == 4)
        {
            setBasicEnemySettings(6, 10);
            setFastBasicEnemySettings(8, 12);
            setSprinterEnemySettings(5, 10);
        }
        if (waveNumber == 5)
        {
            setBasicEnemySettings(6, 10);
            setFastBasicEnemySettings(15, 20);
            setSprinterEnemySettings(12, 15);
        }
        if (waveNumber == 6)
        {
            setBasicEnemySettings(6, 8);
            setFastBasicEnemySettings(6, 8);
            setSprinterEnemySettings(6, 8);
            setHeavyEnemySettings(8, 12);
        }
        if (waveNumber == 7)
        {
            setFastBasicEnemySettings(10, 12);
            setHeavyEnemySettings(8, 12);
            setSprinterEnemySettings(6, 8);
            setHeavyEnemySettings(8, 12);
        }
        if (waveNumber == 8)
        {
            setFastBasicEnemySettings(10, 12);
            setHeavyEnemySettings(8, 12);
            setSprinterEnemySettings(6, 8);
            setHeavyEnemySettings(8, 12);
            setShieldGeneratorEnemySettings(1, 2);
        }
        if (waveNumber == 9)
        {
            setFastBasicEnemySettings(10, 12);
            setHeavyEnemySettings(11, 12);
            setSprinterEnemySettings(10, 20);
            setHeavyEnemySettings(14, 15);
            setShieldGeneratorEnemySettings(4, 8);
        }
        if (waveNumber == 10)
        {
            setHeavyEnemySettings(11, 12);
            setSprinterEnemySettings(10, 20);
            setHeavyEnemySettings(14, 15);
            setShieldGeneratorEnemySettings(4, 8);
            setPopperEnemySettings(4, 6);
        }
        if (waveNumber == 11)
        {
            setHeavyEnemySettings(15, 15);
            setSprinterEnemySettings(20, 20);
            setHeavyEnemySettings(14, 15);
            setShieldGeneratorEnemySettings(6, 8);
            setPopperEnemySettings(4, 6);
        }
        if (waveNumber == 12)
        {
            setPopperEnemySettings(8, 10);
            setBasicEnemySettings(12, 12);
            setFastBasicEnemySettings(20, 20);
            setHeavyEnemySettings(15, 15);
            setSprinterEnemySettings(20, 20);
            setHeavyEnemySettings(14, 15);
            setShieldGeneratorEnemySettings(8, 10);
            setPopperEnemySettings(4, 6);
            setDisablerEnemySettings(1, 2);
        }
        if (waveNumber == 13)
        {
            setDisablerEnemySettings(2, 2);
            setPopperEnemySettings(10, 10);
            setBasicEnemySettings(14, 15);
            setFastBasicEnemySettings(20, 20);
            setHeavyEnemySettings(15, 15);
            setDisablerEnemySettings(1, 2);
            setSprinterEnemySettings(15, 15);
            setHeavyEnemySettings(14, 15);
            setShieldGeneratorEnemySettings(8, 10);
            setPopperEnemySettings(4, 6);
            setDisablerEnemySettings(1, 2);
        }
        //End Wave Spawning Control /\

        setupNextWave();
    }

	void setTestObjSettings(int min, int max)
	{
		TestObjobject.transform.GetComponent<EntityStats> ().amountToSpawnMin = min;
		TestObjobject.transform.GetComponent<EntityStats> ().amountToSpawnMax = max;
        TestObjobject.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(TestObjobject);
    }
    void setBasicEnemySettings(int min, int max)
    {
        basicEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMin = min;
        basicEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMax = max;
        basicEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(basicEnemyObj);
    }
    void setFastBasicEnemySettings(int min, int max)
    {
        fastBasicEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMin = min;
        fastBasicEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMax = max;
        fastBasicEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(fastBasicEnemyObj);
    }
    void setDisablerEnemySettings(int min, int max)
    {
        disablerEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMin = min;
        disablerEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMax = max;
        disablerEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(disablerEnemyObj);
    }
    void setSprinterEnemySettings(int min, int max)
    {
        sprinterEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMin = min;
        sprinterEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMax = max;
        sprinterEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(sprinterEnemyObj);
    }
    void setPopperEnemySettings(int min, int max)
    {
        popperEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMin = min;
        popperEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMax = max;
        popperEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(popperEnemyObj);
    }
    void setHeavyEnemySettings(int min, int max)
    {
        heavyEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMin = min;
        heavyEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMax = max;
        heavyEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(heavyEnemyObj);
    }
    void setShieldGeneratorEnemySettings(int min, int max)
    {
        shieldGenratorEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMin = min;
        shieldGenratorEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnMax = max;
        shieldGenratorEnemyObj.transform.GetComponent<EntityStats>().amountToSpawnRangeSetup();
        spawnableEntitiesList.Add(shieldGenratorEnemyObj);
    }

    public static void setSpawningTimerOff()
    {
        callWaveButtonStatic.SetActive(true);
        spawningTimerStatic = 0;
        spawningTimerToggleStatic = false;
        spawningTimerToggleStatic2 = false;
        totalAmountOfEnemiesToSpawn = 0;
    }
}
