using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameObject machineGunPrefabStatic;
    public GameObject machineGunPrefab;
	public static GameObject sniperPrefabStatic;
	public GameObject sniperPrefab;
	public static GameObject FlameThrowerPrefabStatic;
	public GameObject FlameThrowerPrefab;
	public static GameObject FreezePrefabStatic;
	public GameObject FreezePrefab;
	public static GameObject EngineerTowerPrefabStatic;
	public GameObject EngineerTowerPrefab;
	public static GameObject GrenadeLauncherPrefabStatic;
	public GameObject GrenadeLauncherTowerPrefab;
    public GameObject testObj;

    //Synergy prefabs follow \/ 2 letters: each represents a tower. | A=machine | B=sniper | C=flame | D=freeze | E=grenade | (Engineer does not synergize)
    public GameObject ABPrefab;
    public GameObject ACPrefab;
    public GameObject ADPrefab;
    public GameObject AEPrefab;
    public GameObject BCPrefab;
    public GameObject BDPrefab;
    public GameObject BEPrefab;
    public GameObject CDPrefab;
    public GameObject CEPrefab;
    public GameObject DEPrefab;
    public static GameObject ABPrefabStatic;
    public static GameObject ACPrefabStatic;
    public static GameObject ADPrefabStatic;
    public static GameObject AEPrefabStatic;
    public static GameObject BCPrefabStatic;
    public static GameObject BDPrefabStatic;
    public static GameObject BEPrefabStatic;
    public static GameObject CDPrefabStatic;
    public static GameObject CEPrefabStatic;
    public static GameObject DEPrefabStatic;

    public GameObject mainCamera;


    public static int entityHealth;
    public static int towerDamage;
    public int entityHealthPublic;
    public int towerDamagePublic;

    public static int money;
	public  int moneyNonStatic;
    public static int health;

    public static Vector3 mousePos;
    public Vector3 nonStaticMousePos;

    public static GameObject GameStuffParentObj;
    public static GameObject OptionsMenuParentObj;
    public GameObject GameStuffParentObjNonStatic;
    public GameObject OptionsMenuParentObjNonStatic;
	public static bool waveInProgress;
	public bool waveInProgressNonStatic;

	public static List<GameObject> TowerList;
	public List<GameObject> TowerListNonStatic;

	public GameObject towerBeingUpdated;
	public GameObject towerBeingUpdatedChild;

    public static List<GameObject> EnemyList;
    public List<GameObject> EnemyListNonStatic;

    public static GameObject borderTopLeft;
    public static GameObject borderBottomRight;
    public GameObject borderTopLeftNonStatic;
    public GameObject borderBottomRightNonStatic;

    public static int cameraMoveSpeed;

    public GameObject nonStaticUpgradeCostText;
    public GameObject nonStaticUpgradeButton;
    public GameObject nonStaticUpgradeText;
    public GameObject nonStaticSynergizeButton;
    public static GameObject SynergizeButton;
    public static GameObject UpgradeButton;
    public static GameObject upgradeLevelText;
    public static GameObject UpgradeText;
    public static GameObject upgradeCostText;

    public static GameObject TowerStatsParent; //Set from non-static counterpart below
    public GameObject nonStaticTowerStatsParent; 

    public static bool aTowerIsSelected;
    public static GameObject selectedTower;
    public GameObject selectedTowernonStatic;
    public bool nonStaticATowerIsSelected;

    public GameObject MoneyStatTxt;
    public GameObject HealthStatTxt;
    public GameObject RoundStatTxt;

    //the 3 below and the voids they are in should be in UIController, but they are here, and im too lazy to move them.
    public GameObject GameUIParentObj;
    public GameObject OptionsScreenUI;
	public GameObject MainMenuUI;
    public GameObject TowerInfoUIParentObj;
	public GameObject Game;

    public GameObject TowerInfoButton;
    public GameObject OptionsMenuButton;
    public static bool towerInfoAndOptionsMenuButtonsState;

	public GameObject FirstWavesMusic;
	public GameObject FirstHalfMiddleMusic;
	public GameObject SecondHalfMiddleMusic;
	public GameObject FinalWavesMusic;

	public static GameObject staticFirstWavesMusic;
	public static GameObject staticFirstHalfMiddleMusic;
	public static GameObject staticSecondHalfMiddleMusic;
	public static GameObject staticFinalWavesMusic;

    public static int highScore; //Player prefs "HighScore" , updated from WaveGen
    public int nonStaticHighScore;
    public int oldHighScore;
    public static GameObject highScoreTextObj;
    public GameObject nonStaticHighScoreTextObj;

    public GameObject PopperAudioSource;
    public static GameObject staticPopperAudioSource;

	public GameObject gunshotAudioSource;
	public static GameObject staticgunshotAudioSource;

    public static GameObject planO; //Plan O = plan to murder basic enemies that are left over by Main Manu
    public GameObject nonStaticPlanO;

    public int towerColliderFlashTimer;
    public bool towerColliderFlashTimerBool;

    public static List<GameObject> EnemyList2;

    public List<GameObject> enemyList2NonStatic;

	public float SFXVolume;
	public static float staticSFXVolume;
	public GameObject SFXSlider;
    //public List<GameObject> towersToMurder; //Plan X

    // Use this for initialization
    void Start () {


        towerColliderFlashTimer = 20;
        towerColliderFlashTimerBool = false;

        planO = nonStaticPlanO;
		staticgunshotAudioSource = gunshotAudioSource;
        staticPopperAudioSource = PopperAudioSource;
        oldHighScore = PlayerPrefs.GetInt("HighScore");
        PlayerPrefs.SetInt("PerRunWaves", 0);
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreTextObj = nonStaticHighScoreTextObj;
        highScoreTextObj.GetComponent<Text>().text = "High Score: " + highScore + " Waves";

        upgradeCostText = nonStaticUpgradeCostText;

        staticFirstWavesMusic = FirstWavesMusic;
		staticFirstHalfMiddleMusic = FirstHalfMiddleMusic;
		staticSecondHalfMiddleMusic = SecondHalfMiddleMusic;
		staticFinalWavesMusic = FinalWavesMusic;

        ABPrefabStatic = ABPrefab;
        ACPrefabStatic = ACPrefab;
        ADPrefabStatic = ADPrefab;
        AEPrefabStatic = AEPrefab;
        BCPrefabStatic = BCPrefab;
        BDPrefabStatic = BDPrefab;
        BEPrefabStatic = BEPrefab;
        CDPrefabStatic = CDPrefab;
        CEPrefabStatic = CEPrefab;
        DEPrefabStatic = DEPrefab;

        SynergizeButton = nonStaticSynergizeButton;
        TowerStatsParent = nonStaticTowerStatsParent;
        towerInfoAndOptionsMenuButtonsState = true;
        machineGunPrefabStatic = machineGunPrefab;
		sniperPrefabStatic = sniperPrefab;
		FlameThrowerPrefabStatic = FlameThrowerPrefab;
		FreezePrefabStatic = FreezePrefab;
		EngineerTowerPrefabStatic = EngineerTowerPrefab;
		GrenadeLauncherPrefabStatic = GrenadeLauncherTowerPrefab;
        money = 5000;

        health = 15;
        cameraMoveSpeed = 4;
        borderTopLeft = borderTopLeftNonStatic;
        borderBottomRight = borderBottomRightNonStatic;
		TowerList = new List<GameObject>();

        GameStuffParentObj = GameObject.Find("GameStuff");
        OptionsMenuParentObj = GameObject.Find("OptionsScreen");
        OptionsMenuParentObjNonStatic = OptionsMenuParentObj;
        GameStuffParentObjNonStatic = GameStuffParentObj;
        UpgradeText = nonStaticUpgradeText;
        UpgradeButton = nonStaticUpgradeButton;
        upgradeLevelText = UpgradeButton.transform.GetChild(0).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.GetComponent<WaveGen>().waveNumber == 13 && GameController.EnemyList.Count == 0 && WaveGen.totalAmountOfEnemiesToSpawn <= WaveGen.enemiesSpawnedInThisWave)
        {
            SceneManager.LoadScene("WinScene");
        }
        moneyNonStatic = money;
		MoneyStatTxt.GetComponent<Text>().text = "Money: " + money;
		SFXVolume = SFXSlider.GetComponent<Slider> ().value;
		staticSFXVolume = SFXVolume;
        enemyList2NonStatic = EnemyList2;
        EnemyList2 = WaveGen.entitiesSpawnedStatic;
        foreach (GameObject obj in EnemyList2)
        {
            if(obj.transform.GetComponent<EntityStats>().health <= 0)
            {
                money += obj.gameObject.transform.GetComponent<EntityStats>().dropMoney;
                DestroyImmediate(obj.gameObject);
            }
        }
        foreach(GameObject obj in EnemyList2)
        {
            if(obj == null)
            {
                EnemyList2.Remove(obj.gameObject);
            }
        }
        selectedTowernonStatic = selectedTower;
		//mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        nonStaticMousePos = mousePos;
        if(towerColliderFlashTimerBool == true && towerColliderFlashTimer > 0)
        {
            towerColliderFlashTimer -= 1;
        }
        if(towerColliderFlashTimer <= 0)
        {
            towerColliderFlashTimerBool = false;
            towerColliderFlashTimer = 2;
            foreach(GameObject tower in TowerList)
            {
                tower.transform.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        foreach(GameObject obj in TowerList)
        {
            if(obj == null)
            {
                TowerList.Remove(obj);
            }
        }
        if(health <= 0)
        {
            PlayerPrefs.SetInt("PerRunWaves", gameObject.transform.GetComponent<WaveGen>().waveNumber - 1); //Used to transfer number of waves beaten in this run to "GameOver" scene, - 1 because it shouldnt count the wave you were in when you died.
            PlayerPrefs.SetInt("OldHighScore", oldHighScore);
            SceneManager.LoadScene("GameOver");
        }
        nonStaticHighScore = highScore;
        if (towerInfoAndOptionsMenuButtonsState == true)
        {
            TowerInfoButton.SetActive(true);
            OptionsMenuButton.SetActive(true);
        }
        else
        {
            TowerInfoButton.SetActive(false);
            OptionsMenuButton.SetActive(false);
        }
        nonStaticATowerIsSelected = aTowerIsSelected;     
		waveInProgressNonStatic = waveInProgress;
		if (WaveGen.entitiesSpawnedStatic.Count == 0) {
			waveInProgress = false;
		}
		TowerListNonStatic = TowerList;
        entityHealthPublic = entityHealth;
        towerDamagePublic = towerDamage;
        RoundStatTxt.GetComponent<Text>().text = "Wave: " + gameObject.transform.GetComponent<WaveGen>().waveNumber;
        MoneyStatTxt.GetComponent<Text>().text = "Money: " + money;
        HealthStatTxt.GetComponent<Text>().text = "Health: " + health;
    }

	void SetTowerStats(int towerIndex)
	{
		towerBeingUpdated = TowerList [towerIndex].gameObject;
		towerBeingUpdatedChild = towerBeingUpdated.gameObject.transform.GetChild (0).gameObject;
		if (towerBeingUpdatedChild.transform.tag == "MachineGun") {
			towerBeingUpdatedChild.transform.GetComponent<EntityStats> ().attackDamage = 10;
			towerBeingUpdatedChild.transform.GetComponent<EntityStats> ().coolDown = 3;
		}
	}

    public static void setUpgradeLevelText(GameObject tower)
    {
        if(tower.transform.GetComponent<TowerController>().towerLevel <= 3)
        {
            if(selectedTower != null)
            {
                selectedTower.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            WaveGen.callWaveButtonStatic.SetActive(false);
            aTowerIsSelected = true;
            selectedTower = tower;
            selectedTower.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            towerInfoAndOptionsMenuButtonsState = false;
            if(tower.transform.GetComponent<TowerController>().SynergyAvailable == true)
            {
                UpgradeButton.SetActive(false);
                upgradeCostText.SetActive(false);
                SynergizeButton.SetActive(true);
            }
            else
            {
                UpgradeButton.SetActive(true);
                upgradeCostText.SetActive(true);
                SynergizeButton.SetActive(false);
            }
            setUpgradeText(tower);
        }
    }

    void FixedUpdate()
    {
        if (selectedTower != null && Input.GetKeyDown(KeyCode.Escape))
        {
            UpgradeButton.SetActive(false);
            upgradeCostText.SetActive(false);
            SynergizeButton.SetActive(false);
            selectedTower.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            UpgradeText.SetActive(false);
            aTowerIsSelected = false;
            TowerStatsParent.SetActive(false);
            if(WaveGen.totalAmountOfEnemiesToSpawn <= WaveGen.enemiesSpawnedInThisWave && aTowerIsSelected == false && WaveGen.spawningTimerToggleStatic == true)
            {
                WaveGen.setSpawningTimerOff();
            }
            if(WaveGen.totalAmountOfEnemiesToSpawn > WaveGen.enemiesSpawnedInThisWave)
            {
                WaveGen.callWaveButtonStatic.SetActive(false);
            }
            towerInfoAndOptionsMenuButtonsState = true;
            selectedTower = null;
        }
        if(selectedTower != null)
        {
            OptionsMenuButton.SetActive(false);
            TowerInfoButton.SetActive(false);
            WaveGen.callWaveButtonStatic.SetActive(false);
        }
        if(selectedTower == null && WaveGen.totalAmountOfEnemiesToSpawn <= WaveGen.enemiesSpawnedInThisWave)
        {
            OptionsMenuButton.SetActive(true);
            TowerInfoButton.SetActive(true);
            WaveGen.callWaveButtonStatic.SetActive(true);
        }
    }

    public static void setUpgradeText(GameObject tower)
    {
		UpgradeText.SetActive (true);
		int tempTowerLevel = tower.transform.GetComponent<TowerController> ().towerLevel + 1;
        if(tower.transform.GetComponent<TowerController>().towerLevel == 0)
        {
            UpgradeText.transform.GetComponent<Text>().text = "Upgrade #" + tempTowerLevel + ": " + tower.transform.GetComponent<TowerController>().upgradeTextLvl1;
            upgradeCostText.transform.GetComponent<Text>().text = "Cost: " + tower.transform.GetComponent<TowerController>().towerUpgrade1Cost;

        }
        if (tower.transform.GetComponent<TowerController>().towerLevel == 1)
        {
            UpgradeText.transform.GetComponent<Text>().text = "Upgrade #" + tempTowerLevel + ": " + tower.transform.GetComponent<TowerController>().upgradeTextLvl2;
            upgradeCostText.transform.GetComponent<Text>().text = "Cost: " + tower.transform.GetComponent<TowerController>().towerUpgrade2Cost;
        }
        if (tower.transform.GetComponent<TowerController>().towerLevel == 2)
        {
            UpgradeText.transform.GetComponent<Text>().text = "Upgrade #" + tempTowerLevel + ": " + tower.transform.GetComponent<TowerController>().upgradeTextLvl3;
            upgradeCostText.transform.GetComponent<Text>().text = "Cost: " + tower.transform.GetComponent<TowerController>().towerUpgrade3Cost;
        }
        if (tower.transform.GetComponent<TowerController>().towerLevel == 3 && tower.transform.GetComponent<TowerController>().SynergyAvailable == false)
        {
            UpgradeText.transform.GetComponent<Text>().text = "Max Upgrades. Synergy Not Avaiable";
            UpgradeButton.SetActive(false);
            upgradeCostText.SetActive(false);
            SynergizeButton.SetActive(false);
        }
        if (tower.transform.GetComponent<TowerController>().towerLevel == 3 && tower.transform.GetComponent<TowerController>().SynergyAvailable == true)
        {
            UpgradeText.transform.GetComponent<Text>().text = "Synergize Towers: " + tower.transform.GetComponent<TowerController>().upgradeTextSynergy;
        }
        setTowerStatsTexts(tower);
    }

    public static void setTowerStatsTexts(GameObject tower)
    {
        TowerStatsParent.transform.GetChild(0).gameObject.transform.GetComponent<Text>().text = "Current Level: " + tower.transform.GetComponent<TowerController>().towerLevel;
        TowerStatsParent.transform.GetChild(1).gameObject.transform.GetComponent<Text>().text = "Damage: " + tower.transform.GetComponent<EntityStats>().attackDamage;
        TowerStatsParent.transform.GetChild(2).gameObject.transform.GetComponent<Text>().text = "Range: " + tower.transform.GetComponent<EntityStats>().rangeInt;
        TowerStatsParent.transform.GetChild(3).gameObject.transform.GetComponent<Text>().text = "Cooldown: " + tower.transform.GetComponent<EntityStats>().coolDown;
        TowerStatsParent.SetActive(true);
    }


	public void redirectUpgrade()
	{
        foreach (GameObject tower in TowerList)
        {
            tower.transform.GetComponent<BoxCollider2D>().enabled = true;
        }
        towerColliderFlashTimer = 20;
        towerColliderFlashTimerBool = true;
        selectedTower.transform.GetComponent<TowerController> ().Upgrade ();
	}

    public void redirectSynergize()
    {
        selectedTower.transform.GetComponent<TowerController>().Synergize();
        UpgradeButton.SetActive(false);
        upgradeCostText.SetActive(false);
        SynergizeButton.SetActive(false);
        selectedTower.transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        UpgradeText.SetActive(false);
        aTowerIsSelected = false;
        TowerStatsParent.SetActive(false);
        WaveGen.callWaveButtonStatic.SetActive(true);
        towerInfoAndOptionsMenuButtonsState = true;
    }

    public void toggleOptionsMenuOn()
    {
        OptionsScreenUI.SetActive(true);
        mainCamera.SetActive(false);

       
    }

    public void toggleOptionsMenuOff()
    {
		
        OptionsScreenUI.SetActive(false);
        GameUIParentObj.SetActive(true);
        mainCamera.SetActive(true);
    }

	public void toggleMainMenuOn()
	{
		MainMenuUI.SetActive(true);
		GameUIParentObj.SetActive(false);
		OptionsScreenUI.SetActive(false);
		Game.SetActive(false);
	}

	public void toggleMainMenuOff()
	{
		MainMenuUI.SetActive(false);
		Game.SetActive(true);
		GameUIParentObj.SetActive(true);
		
		}

    public void toggleTowerInfoMenuOn()
    {
        TowerInfoUIParentObj.SetActive(true);
    }

    public void toggleTowerInfoMenuOff()
    {
        TowerInfoUIParentObj.SetActive(false);
    }
    
    public void resetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreTextObj.GetComponent<Text>().text = "High Score: " + highScore;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
