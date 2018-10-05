using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TowerController : MonoBehaviour //NOTE: Need to setup the upgrade system for a few more towers (what each level does in its own void [ex. UpgradeStatsMachienGun])
{
    public GameObject targetGameObject;
    public int rotateSpeedDependent;
    public GameObject machineGunBulletPrefab;
    public GameObject sniperBulletPrefab;
    public GameObject grenadeBulletPrefab;

    public int timer;
    public int localCoolDown;
    public bool localCanShoot;

    public GameObject bulletType;
    public GameObject lastBulletInstantiated;
    public float lastTargetPosx;
    public float lastTargetPosy;

    public int localRangeInt;
    public float speed = 1.5f;
    public float speed2;

    public int towerLevel;
    public Sprite towerLevelSprite;
    public List<Sprite> towerSpriteList;

    public string upgradeTextLvl1;
    public string upgradeTextLvl2;
    public string upgradeTextLvl3;
    public string upgradeTextSynergy;

    public bool possibleSynergyX;
    public bool possibleSynergyY;
    public GameObject towerToSynergizeWith;
    public bool SynergyAvailable;
    public int synergizePrice;

    public GameObject synergyTestChild;

    public bool possibleFreezeEnemyX;
    public bool possibleFreezeEnemyY;

    public bool enableTakeExtraDamageFreeze;
    public int explosionSize;
    public GameObject flamethrowerCone;

    public int towerUpgrade1Cost;
    public int towerUpgrade2Cost;
    public int towerUpgrade3Cost;

    public bool canBeEffectedByEngineer;

    public bool canBePlaced;
    public List<GameObject> objsCollidingWithWhileBeingPlaced;

    public bool isSynergy;

    public int disableTimer;
    public bool disableTimerBool;
    public Color disableColor;

    public bool canBeSelected;

    public GameObject bulletAudioSourceChild;
    public AudioClip bulletSound;

    //public List<GameObject> towersToMurder; Plan X

    // Use this for initialization
    void Start()
    {
        if(isSynergy == true)
        {
            GameController.UpgradeButton.SetActive(false);
            GameController.upgradeCostText.SetActive(false);
            GameController.SynergizeButton.SetActive(false);
            GameController.UpgradeText.SetActive(false);
            GameController.aTowerIsSelected = false;
            GameController.TowerStatsParent.SetActive(false);
            WaveGen.callWaveButtonStatic.SetActive(true);
            GameController.towerInfoAndOptionsMenuButtonsState = true;
        }

        disableColor = gameObject.transform.GetComponent<SpriteRenderer>().color;
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 255);
        disableTimer = 300;
        disableTimerBool = false;
        GameController.TowerList.Add(gameObject);
        SynergyAvailable = false;
        canBePlaced = true;
        canBeEffectedByEngineer = true;
        if (gameObject.transform.tag == "Flamethrower")
        {
            flamethrowerCone = gameObject.transform.parent.gameObject.transform.GetChild(3).gameObject;
            flamethrowerCone.transform.GetComponent<FlameScript>().flameDamage = gameObject.transform.GetComponent<EntityStats>().attackDamage;
            flamethrowerCone.transform.GetComponent<FlameScript>().lingeringFlameDamage = gameObject.transform.GetComponent<EntityStats>().lingeringDamage;
            flamethrowerCone.transform.GetComponent<FlameScript>().lingeringFlameTime = gameObject.transform.GetComponent<EntityStats>().lingeringDamageTime;
        }

        localCoolDown = gameObject.transform.GetComponent<EntityStats>().coolDown;
        localCanShoot = gameObject.transform.GetComponent<EntityStats>().canShoot;

        gameObject.transform.GetComponent<EntityStats>().freezeTowerSlowLevel = 0.5F;

        if (gameObject.transform.tag == "MachineGun")
        {
            bulletType = machineGunBulletPrefab;
        }
        if (gameObject.transform.tag == "GrenadeLauncher")
        {
            bulletType = grenadeBulletPrefab;
        }
        localRangeInt = gameObject.transform.GetComponent<EntityStats>().rangeInt;
    }

    // Update is called once per frame
    void Update()
    {
        if(objsCollidingWithWhileBeingPlaced.Count > 0)
        {
            canBePlaced = false;
        }
        else
        {
            canBePlaced = true;
        }
        if(disableTimerBool == true && disableTimer > 0)
        {
            Color temp = gameObject.transform.GetComponent<SpriteRenderer>().color;
            gameObject.transform.GetComponent<SpriteRenderer>().color = disableColor;
            disableTimer -= 1;
        }
        if(disableTimer <= 0 && disableTimerBool == true)
        {
            Enable();
            disableTimerBool = false;
            disableTimer = 300;
        }
        if (targetGameObject != null)
        {
            RotateTower(gameObject.transform.GetComponent<EntityStats>().rotateSpeed, targetGameObject);
        }
        if (disableTimerBool == false)
        {
            Shoot();
        }
        speed2 = speed * Time.deltaTime;
        rotateSpeedDependent = gameObject.transform.GetComponent<EntityStats>().rotateSpeed;
    }

    void FixedUpdate()
    {
        foreach (GameObject possibleTarget in GameController.EnemyList2 )
        {
            if (possibleTarget != null && possibleTarget.transform.position.x <= gameObject.transform.position.x + localRangeInt && possibleTarget.transform.position.x >= gameObject.transform.position.x - localRangeInt && possibleTarget.transform.position.y <= gameObject.transform.position.y + localRangeInt && possibleTarget.transform.position.y >= gameObject.transform.position.y - localRangeInt && targetGameObject == null)
            {
                targetGameObject = possibleTarget;
                localCanShoot = true;
            }
        }
        if (targetGameObject.transform.position.x >= gameObject.transform.position.x + localRangeInt || targetGameObject.transform.position.x <= gameObject.transform.position.x - localRangeInt && targetGameObject.transform.position.y >= gameObject.transform.position.y + localRangeInt || targetGameObject.transform.position.y <= gameObject.transform.position.y - localRangeInt && targetGameObject != null)
        {
            targetGameObject = null;
        } 
    }



    public void RotateTower(int rotateSpeed, GameObject targetObj)
    {
        if(disableTimerBool == false)
        {
            Vector3 vectorToTarget = targetObj.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.parent.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
        }
    }

    public void Shoot()
    {
        if (targetGameObject != null)
        {
            if (timer >= localCoolDown)
            {
                timer = 0;
                if (gameObject.transform.tag != "GrenadeLauncher" && gameObject.transform.tag != "FlamethrowerPrefab" && gameObject.transform.tag != "AEPrefab" && gameObject.transform.tag != "BEPrefab" && gameObject.transform.tag != "CEPrefab" && gameObject.transform.tag != "DEPrefab")
                {
                    GameObject bulletInstantiated = GameObject.Instantiate(bulletType, gameObject.transform.transform.position, gameObject.transform.rotation) as GameObject;
                    lastBulletInstantiated = bulletInstantiated;
                    lastBulletInstantiated.transform.GetComponent<BulletScript>().bulletDamage = gameObject.transform.GetComponent<EntityStats>().attackDamage;
                    lastBulletInstantiated.transform.GetComponent<BulletScript>().burnDamage = gameObject.transform.GetComponent<EntityStats>().lingeringDamage;
                    lastBulletInstantiated.transform.GetComponent<BulletScript>().burnTime = gameObject.transform.GetComponent<EntityStats>().lingeringDamageTime;
                    lastBulletInstantiated.transform.GetComponent<BulletScript>().shouldBurnEnemies = gameObject.transform.GetComponent<EntityStats>().shootsBurningBullets;
                    lastBulletInstantiated.transform.GetComponent<BulletScript>().shouldSlowEnemies = gameObject.transform.GetComponent<EntityStats>().shootsSlowingBullets;
                    lastTargetPosx = targetGameObject.transform.position.x;
                    lastTargetPosy = targetGameObject.transform.position.y;
					bulletAudioSourceChild.transform.GetComponent<AudioSource>().PlayOneShot(bulletSound, GameController.staticSFXVolume);
                }
                else
                {
					if (gameObject.transform.tag == "GrenadeLauncher" || gameObject.transform.tag == "AEPrefab" || gameObject.transform.tag == "BEPrefab" || gameObject.transform.tag == "CEPrefab" || gameObject.transform.tag == "DEPrefab")
                    {
                        lastTargetPosx = targetGameObject.transform.position.x;
                        lastTargetPosy = targetGameObject.transform.position.y;
                        GameObject grenadeInstantiated = GameObject.Instantiate(bulletType, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), gameObject.transform.rotation) as GameObject;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().enemyPosX = (int)lastTargetPosx;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().enemyPosY = (int)lastTargetPosy;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().explosionSizeLocal = explosionSize;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().shouldBurnEnemies = gameObject.transform.GetComponent<EntityStats>().shootsBurningBullets;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().shouldSlowEnemies = gameObject.transform.GetComponent<EntityStats>().shootsSlowingBullets;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().burnDamage = gameObject.transform.GetComponent<EntityStats>().lingeringDamage;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().burnTime = gameObject.transform.GetComponent<EntityStats>().lingeringDamageTime;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().grenadeDamage = gameObject.transform.GetComponent<EntityStats>().attackDamage;
                        grenadeInstantiated.transform.GetComponent<GrenadeScript>().towerShotFrom = gameObject;
                        lastBulletInstantiated = grenadeInstantiated;
                    }
                }
            }
            else
            {
                timer += 1;
            }
        }
        lastBulletInstantiated.GetComponent<Rigidbody2D>().velocity = transform.right * gameObject.transform.GetComponent<EntityStats>().bulletSpeed;
    }

    public void SynergizeAB(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.ABPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }

    public void SynergizeAC(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            GameObject synergySpawned = Instantiate(GameController.ACPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation) as GameObject;
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }
    public void SynergizeAD(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.ADPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }
    public void SynergizeAE(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.AEPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }
    public void SynergizeBC(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.BCPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }
    public void SynergizeBD(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.BDPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }
    public void SynergizeBE(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.BEPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }
    public void SynergizeCD(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.CDPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }
    public void SynergizeCE(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.CEPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }

    public void SynergizeDE(GameObject tower1, GameObject tower2)
    {
        if (GameController.money >= tower1.transform.GetComponent<TowerController>().synergizePrice + tower2.transform.GetComponent<TowerController>().synergizePrice && tower1.transform.tag != tower2.transform.tag)
        {
            int xToInstantiateAtNoDivision = (int)tower1.transform.position.x + (int)tower2.transform.position.x;
            int xToInstantiateAt = xToInstantiateAtNoDivision / 2;
            int yToInstantiateAtNoDivision = (int)tower1.transform.position.y + (int)tower2.transform.position.y;
            int yToInstantiateAt = yToInstantiateAtNoDivision / 2;
            Instantiate(GameController.DEPrefabStatic, new Vector2(xToInstantiateAt, yToInstantiateAt), transform.rotation);
            towerLevel += 1;
            towerToSynergizeWith.transform.GetComponent<TowerController>().towerLevel += 1;
            DestroyImmediate(towerToSynergizeWith.transform.parent.gameObject);
            DestroyImmediate(gameObject.transform.parent.gameObject);
        }
    }

    public void Synergize()
    {
        if (towerLevel == 3 && SynergyAvailable == true)
        {
            if (towerToSynergizeWith.transform.tag == "MachineGun" && gameObject.transform.tag == "Sniper" || towerToSynergizeWith.transform.tag == "Sniper" && gameObject.transform.tag == "MachineGun")
            {
                SynergizeAB(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "MachineGun" && gameObject.transform.tag == "Flamethrower" || towerToSynergizeWith.transform.tag == "Flamethrower" && gameObject.transform.tag == "MachineGun")
            {
                SynergizeAC(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "MachineGun" && gameObject.transform.tag == "FreezeTower" || towerToSynergizeWith.transform.tag == "FreezeTower" && gameObject.transform.tag == "MachineGun")
            {
                SynergizeAD(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "MachineGun" && gameObject.transform.tag == "GrenadeLauncher" || towerToSynergizeWith.transform.tag == "GrenadeLauncher" && gameObject.transform.tag == "MachineGun")
            {
                SynergizeAE(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "Sniper" && gameObject.transform.tag == "Flamethrower" || towerToSynergizeWith.transform.tag == "Flamethrower" && gameObject.transform.tag == "Sniper")
            {
                SynergizeBC(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "Sniper" && gameObject.transform.tag == "FreezeTower" || towerToSynergizeWith.transform.tag == "FreezeTower" && gameObject.transform.tag == "Sniper")
            {
                SynergizeBD(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "Sniper" && gameObject.transform.tag == "GrenadeLauncher" || towerToSynergizeWith.transform.tag == "GrenadeLauncher" && gameObject.transform.tag == "Sniper")
            {
                SynergizeBE(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "Flamethrower" && gameObject.transform.tag == "FreezeTower" || towerToSynergizeWith.transform.tag == "FreezeTower" && gameObject.transform.tag == "Flamethrower")
            {
                SynergizeCD(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "Flamethrower" && gameObject.transform.tag == "GrenadeLauncher" || towerToSynergizeWith.transform.tag == "GrenadeLauncher" && gameObject.transform.tag == "Flamethrower")
            {
                SynergizeCE(towerToSynergizeWith, gameObject);
                return;
            }
            if (towerToSynergizeWith.transform.tag == "FreezeTower" && gameObject.transform.tag == "GrenadeLauncher" || towerToSynergizeWith.transform.tag == "GrenadeLauncher" && gameObject.transform.tag == "FreezeTower")
            {
                SynergizeDE(towerToSynergizeWith, gameObject);
                return;
            }
        }
    }

    public void Upgrade()
    {
        if (gameObject.transform.tag == "MachineGun")
        {
            if (towerLevel == 0 && GameController.money >= towerUpgrade1Cost)
            {
                GameController.money -= towerUpgrade1Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsMachineGun(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 1 && GameController.money >= towerUpgrade2Cost)
            {
                GameController.money -= towerUpgrade2Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsMachineGun(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 2 && GameController.money >= towerUpgrade3Cost)
            {
                GameController.money -= towerUpgrade3Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsMachineGun(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
        }
        if (gameObject.transform.tag == "Sniper")
        {
            if (towerLevel == 0 && GameController.money >= towerUpgrade1Cost)
            {
                GameController.money -= towerUpgrade1Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsSniper(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 1 && GameController.money >= towerUpgrade2Cost)
            {
                GameController.money -= towerUpgrade2Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsSniper(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 2 && GameController.money >= towerUpgrade3Cost)
            {
                GameController.money -= towerUpgrade3Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsSniper(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
        }
        if (gameObject.transform.tag == "Flamethrower")
        {
            if (towerLevel == 0 && GameController.money >= towerUpgrade1Cost)
            {
				towerLevel = towerLevel + 1;
				GameController.setUpgradeLevelText(gameObject);
				GameController.money -= towerUpgrade1Cost;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsFlamethrower(towerLevel);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 1 && GameController.money >= towerUpgrade2Cost)
            {
				towerLevel = towerLevel + 1;
				GameController.setUpgradeLevelText(gameObject);
				GameController.money -= towerUpgrade2Cost;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsFlamethrower(towerLevel);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 2 && GameController.money >= towerUpgrade3Cost)
            {
				towerLevel = towerLevel + 1;
				GameController.setUpgradeLevelText(gameObject);
				GameController.money -= towerUpgrade3Cost;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsFlamethrower(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
        }
        if (gameObject.transform.tag == "EngineerTower")
        {
            if (towerLevel == 0 && GameController.money >= towerUpgrade1Cost)
            {
                GameController.money -= towerUpgrade1Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsEngineer(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 1 && GameController.money >= towerUpgrade2Cost)
            {
                GameController.money -= towerUpgrade2Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsEngineer(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 2 && GameController.money >= towerUpgrade3Cost)
            {
                GameController.money -= towerUpgrade3Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsEngineer(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
        }
        if (gameObject.transform.tag == "FreezeTower")
        {
            if (towerLevel == 0 && GameController.money >= towerUpgrade1Cost)
            {
                GameController.money -= towerUpgrade1Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsFreeze(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 1 && GameController.money >= towerUpgrade2Cost)
            {
                GameController.money -= towerUpgrade2Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsFreeze(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 2 && GameController.money >= towerUpgrade3Cost)
            {
                GameController.money -= towerUpgrade3Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsFreeze(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
        }
        if (gameObject.transform.tag == "GrenadeLauncher")
        {
            if (towerLevel == 0 && GameController.money >= towerUpgrade1Cost)
            {
                GameController.money -= towerUpgrade1Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsGrenade(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 1 && GameController.money >= towerUpgrade2Cost)
            {
                GameController.money -= towerUpgrade2Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsGrenade(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
            if (towerLevel == 2 && GameController.money >= towerUpgrade3Cost)
            {
                GameController.money -= towerUpgrade3Cost;
                towerLevel = towerLevel + 1;
                towerLevelSprite = towerSpriteList[towerLevel - 1];
                gameObject.transform.GetComponent<SpriteRenderer>().sprite = towerLevelSprite;
                UpgradeStatsGrenade(towerLevel);
                GameController.setUpgradeLevelText(gameObject);
                canBeEffectedByEngineer = true;
                return;
            }
        }
        int tempTowerLevel = towerLevel + 1;
        if (towerLevel < 3)
        {
            if (towerLevel == 0)
            {
                GameController.UpgradeText.transform.GetComponent<Text>().text = "Upgrade #" + tempTowerLevel + ": " + upgradeTextLvl1;
            }
            if (towerLevel == 1)
            {
                GameController.UpgradeText.transform.GetComponent<Text>().text = "Upgrade #" + tempTowerLevel + ": " + upgradeTextLvl2;
            }
            if (towerLevel == 2)
            {
                GameController.UpgradeText.transform.GetComponent<Text>().text = "Upgrade #" + tempTowerLevel + ": " + upgradeTextLvl3;
            }
        }
        if (towerLevel == 3 && SynergyAvailable == false)
        {
            GameController.UpgradeText.transform.GetComponent<Text>().text = "Max Upgrades. Synergy Not Avaiable";
        }
        else
        {
            if (towerLevel == 3 && SynergyAvailable == true)
            {
                GameController.UpgradeText.transform.GetComponent<Text>().text = upgradeTextSynergy;
            }
        }
    }

    public void UpgradeStatsMachineGun(int towerLevelLocalInt)
    {
        if (towerLevelLocalInt == 1)
        {
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 8;
        }
        if (towerLevelLocalInt == 2)
        {
            gameObject.transform.GetComponent<EntityStats>().coolDown = 8;
        }
        if (towerLevelLocalInt == 3)
        {
            gameObject.transform.GetComponent<EntityStats>().coolDown = 6;
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 10;
        }
    }
    public void UpgradeStatsSniper(int towerLevelLocalInt)
    {
        if (towerLevelLocalInt == 1)
        {
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 60;
        }
        if (towerLevelLocalInt == 2)
        {
            gameObject.transform.GetComponent<EntityStats>().coolDown = 200;
        }
        if (towerLevelLocalInt == 3)
        {
            gameObject.transform.GetComponent<EntityStats>().coolDown = 150;
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 80;
        }
    }
    public void UpgradeStatsFlamethrower(int towerLevelLocalInt)
    {
        if (towerLevelLocalInt == 1)
        {
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 6;
            gameObject.transform.GetComponent<EntityStats>().lingeringDamage = 4;
        }
        if (towerLevelLocalInt == 2)
        {
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 8;
            gameObject.transform.GetComponent<EntityStats>().lingeringDamage = 5;
        }
        if (towerLevelLocalInt == 3)
        {
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 9;
            gameObject.transform.GetComponent<EntityStats>().lingeringDamage = 6;
        }
        gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject.transform.GetComponent<FlameScript>().flameDamage = gameObject.transform.GetComponent<EntityStats>().attackDamage;
        gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject.transform.GetComponent<FlameScript>().lingeringFlameDamage = gameObject.transform.GetComponent<EntityStats>().lingeringDamage;
    }
    public void UpgradeStatsEngineer(int towerLevelLocalInt)
    {
        if (towerLevelLocalInt == 1)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.transform.parent.gameObject.transform.GetChild(2).transform.position = new Vector2(gameObject.transform.parent.gameObject.transform.GetChild(2).transform.position.x - 0.04f, gameObject.transform.parent.gameObject.transform.GetChild(2).transform.position.y);
        }
        if (towerLevelLocalInt == 2)
        {
            gameObject.transform.parent.gameObject.transform.GetChild(2).transform.position = new Vector2(gameObject.transform.parent.gameObject.transform.GetChild(2).transform.position.x + 0.19f, gameObject.transform.parent.gameObject.transform.GetChild(2).transform.position.y);
        }
        if (towerLevelLocalInt == 3)
        {
            //the 2 above make the sprite larger for the first level (needed more variation) and centers the sprite correctly for both levels
        }
    }
    public void UpgradeStatsFreeze(int towerLevelLocalInt)
    {
        if (towerLevelLocalInt == 1)
        {
            gameObject.transform.GetComponent<EntityStats>().freezeTowerSlowLevel = 0.7f;
        }
        if (towerLevelLocalInt == 2)
        {
            gameObject.transform.GetComponent<EntityStats>().freezeTowerSlowLevel = 0.9f;
        }
        if (towerLevelLocalInt == 3)
        {
            enableTakeExtraDamageFreeze = true;
        }
    }

    public void UpgradeStatsGrenade(int towerLevelLocalInt)
    {
        if (towerLevelLocalInt == 1)
        {
            gameObject.transform.GetComponent<EntityStats>().attackDamage = 30;
        }
        if (towerLevelLocalInt == 2)
        {
            gameObject.transform.GetComponent<EntityStats>().coolDown = 55;
        }
        if (towerLevelLocalInt == 3)
        {
            explosionSize = 2;
        }
    }

    public void Disable()
    {
        disableTimerBool = true;
    }

    public void Enable()
    {
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 255);
    }

    public void placeTower()
    {
        gameObject.transform.GetComponent<EntityStats>().canShoot = true;
        gameObject.transform.GetComponent<TowerController>().localCanShoot = true;
        GameController.setUpgradeLevelText(gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.transform.parent.gameObject.transform.tag == "Tower" || coll.gameObject.transform.tag == "UI" || coll.gameObject.transform.tag == "Path" || coll.gameObject.transform.tag == "PathNoDirection" && coll.gameObject != gameObject && objsCollidingWithWhileBeingPlaced.Contains(coll.gameObject) == false)
        {
            objsCollidingWithWhileBeingPlaced.Add(coll.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.transform.parent.gameObject.transform.tag == "Tower" || coll.gameObject.transform.tag == "UI" || coll.gameObject.transform.tag == "Path" || coll.gameObject.transform.tag == "PathNoDirection" && objsCollidingWithWhileBeingPlaced.Contains(coll.gameObject) == true)
        {
            objsCollidingWithWhileBeingPlaced.Remove(coll.gameObject);
        }
    }
}