using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
	public bool canBeSlowed;
	public bool takeExtraDamage;

    public int burningTimer;
    public bool burningTimerBool;
    public int burningTimerCount;
    public int burningDamageLocal;
    public int burningTimeLocal;

    public int slowingTimer;
    public bool slowingTimerBool;
    public float backupSpeed;

    public int maxHealth;
    public bool hasShield;
    public int shieldsSpawned;
    public GameObject shieldGeneratorThatGaveShield;
    public List<GameObject> enemiesWithShieldFromThisObj;
    public int enemiesWithShieldFromThisObjCount;
    public bool possibleEnemyToAddShieldX;
    public bool possibleEnemyToAddShieldY;

    public bool possibleEnemyToBuffX;
    public bool possibleEnemyToBuffY;
    public List<GameObject> enemiesToBuff;

    public GameObject tower1ToDisable;
    public GameObject tower2ToDisable;
    public int towerToDisableIndex;
    public List<GameObject> possibleTowersToDisable;
    public int possibleTowersToDisableCount;

    public int buffTimer;
    public bool buffTimerBool;
	public bool buffApplied;

    public int destroyTimer;

    public bool canBeFrozen;

    public float velX;
    public float velY;


    // Use this for initialization
    void Start()
    {
		buffApplied = false;
        canBeFrozen = true;
        maxHealth = gameObject.transform.GetComponent<EntityStats>().health;
        backupSpeed = gameObject.transform.GetComponent<EntityStats>().movementSpeed;
        burningTimerCount = 0;
        burningTimer = 100;
        burningTimerBool = false;
		canBeSlowed = true;
		gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.GetComponent<EntityStats>().movementSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
		

        velX = gameObject.transform.GetComponent<Rigidbody2D>().velocity.x;
        velY = gameObject.transform.GetComponent<Rigidbody2D>().velocity.y;
        destroyTimer += 1;
        if(destroyTimer >= 2500 && gameObject.transform.tag == "SprinterEnemy")
        {
            Destroy(gameObject);
        }
        if (destroyTimer >= 5000 && gameObject.transform.tag == "BasicEnemy")
        {
            Destroy(gameObject);
        }
        if (destroyTimer >= 5000 && gameObject.transform.tag == "DisablerEnemy")
        {
            Destroy(gameObject);
        }
        if (destroyTimer >= 5000 && gameObject.transform.tag == "ShieldGeneratorEnemy")
        {
            Destroy(gameObject);
        }
        if (destroyTimer >= 5000 && gameObject.transform.tag == "BasicEnemy")
        {
            Destroy(gameObject);
        }
        if (destroyTimer >= 10000 && gameObject.transform.tag == "PopperEnemy")
        {
            Destroy(gameObject);
        }
        if (destroyTimer >= 10000 && gameObject.transform.tag == "HeavyEnemy")
        {
            Destroy(gameObject);
        }
        if (destroyTimer >= 3500 && gameObject.transform.tag == "FastBasicEnemy")
        {
            Destroy(gameObject);
        }
        enemiesWithShieldFromThisObjCount = enemiesWithShieldFromThisObj.Count;
        foreach(GameObject enemy in enemiesWithShieldFromThisObj)
        {
            if(enemy == null)
            {
                enemiesWithShieldFromThisObj.Remove(enemy);
            }
        }
        if(gameObject.transform.tag == "ShieldGeneratorEnemy" && enemiesWithShieldFromThisObjCount < 5)
        {
            foreach (GameObject enemy in GameController.EnemyList)
            {
                if(enemy.transform.GetComponent<EnemyController>().hasShield == false)
                {
                    if (enemy.transform.position.x <= gameObject.transform.position.x + gameObject.transform.GetComponent<EntityStats>().rangeInt && enemy.transform.position.x >= gameObject.transform.position.x - gameObject.transform.GetComponent<EntityStats>().rangeInt)
                    {
                        possibleEnemyToAddShieldX = true;
                    }
                    else
                    {
                        possibleEnemyToAddShieldX = false;
                    }
                    if (enemy.transform.position.y <= gameObject.transform.position.y + gameObject.transform.GetComponent<EntityStats>().rangeInt && enemy.transform.position.y >= gameObject.transform.position.y - gameObject.transform.GetComponent<EntityStats>().rangeInt)
                    {
                        possibleEnemyToAddShieldY = true;
                    }
                    else
                    {
                        possibleEnemyToAddShieldY = false;
                    }
                    if (possibleEnemyToAddShieldX == true && possibleEnemyToAddShieldY == true && enemiesWithShieldFromThisObj.Contains(enemy) == false)
                    {
                        enemiesWithShieldFromThisObj.Add(enemy);
                        enemy.transform.GetComponent<EntityStats>().health += 50;
                        enemy.transform.GetChild(0).gameObject.SetActive(true);
                        enemy.transform.GetComponent<EnemyController>().shieldGeneratorThatGaveShield = gameObject;
                    }
                }
            }
        }
        if(gameObject.transform.GetComponent<EntityStats>().health > maxHealth)
        {
            hasShield = true;
        }
        if (gameObject.transform.GetComponent<EntityStats>().health <= maxHealth && hasShield == true)
        {
            shieldGeneratorThatGaveShield.GetComponent<EnemyController>().enemiesWithShieldFromThisObj.Remove(gameObject);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (gameObject.GetComponent<EntityStats>().health <= 0)
        {
			//dropMoneyvoid ();
            if (gameObject.transform.tag == "PopperEnemy")
            {
				
                if(GameController.staticPopperAudioSource.GetComponent<AudioSource>().isPlaying == false)
                {
					GameController.staticPopperAudioSource.GetComponent<AudioSource>().PlayOneShot(GameController.staticPopperAudioSource.GetComponent<AudioSource>().clip, GameController.staticSFXVolume);
                }
                
                Destroy(gameObject);
            }
            if(gameObject.transform.tag == "DisablerEnemy")
            {
                possibleTowersToDisable = GameController.TowerList;
                possibleTowersToDisableCount = possibleTowersToDisable.Count;
                towerToDisableIndex = Random.Range(1, possibleTowersToDisableCount);
                tower1ToDisable = possibleTowersToDisable[towerToDisableIndex - 1];
                possibleTowersToDisable.Remove(possibleTowersToDisable[towerToDisableIndex - 1]);
                tower1ToDisable.transform.GetComponent<TowerController>().Disable();
                if (possibleTowersToDisable.Count > 0)
                {
                    possibleTowersToDisableCount = possibleTowersToDisable.Count;
                    towerToDisableIndex = Random.Range(1, possibleTowersToDisableCount);
                    tower2ToDisable = possibleTowersToDisable[towerToDisableIndex];
                    tower2ToDisable.transform.GetComponent<TowerController>().Disable();
                }
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
        if(burningTimerBool == true && burningTimer > 0)
        {
            burningTimer -= 1;
            gameObject.transform.GetComponent<SpriteRenderer>().color = new Vector4(255, 0, 0, 255);
        }
        if(burningTimer <= 0)
        {
            gameObject.transform.GetComponent<EntityStats>().health -= burningDamageLocal;
            burningTimer = 150;
            burningTimerCount += 1;
        }
        if(burningTimerCount >= burningTimeLocal)
        {
            burningTimerBool = false;
            burningTimer = 150;
            burningTimerCount = 0;
            if(hasShield == false)
            {
                gameObject.transform.GetComponent<SpriteRenderer>().color = gameObject.transform.GetComponent<EntityStats>().spriteColor;
            }
        }
       if(slowingTimerBool == true && slowingTimer > 0)
        {
            slowingTimer -= 1;
        }
        if(slowingTimer <= 0)
        {
            slowingTimerBool = false;
            slowingTimer = 500;
            gameObject.transform.GetComponent<EntityStats>().movementSpeed = backupSpeed;
            if(gameObject.transform.GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(backupSpeed, gameObject.transform.GetComponent<Rigidbody2D>().velocity.y);
            }
            if (gameObject.transform.GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-backupSpeed, gameObject.transform.GetComponent<Rigidbody2D>().velocity.y);
            }
            if (gameObject.transform.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.GetComponent<Rigidbody2D>().velocity.x, backupSpeed);
            }
            if (gameObject.transform.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.GetComponent<Rigidbody2D>().velocity.y, -backupSpeed);
            }
        }
    }
	public void dropMoneyvoid(){
		GameController.money += gameObject.transform.GetComponent<EntityStats>().dropMoney;
	}
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
			if (takeExtraDamage == true) {
				gameObject.GetComponent<EntityStats>().health -= coll.gameObject.GetComponent<BulletScript>().bulletDamage + coll.gameObject.GetComponent<BulletScript>().bulletDamage / 5;
			} else {
				gameObject.GetComponent<EntityStats>().health -= coll.gameObject.GetComponent<BulletScript>().bulletDamage;
			}
            if(coll.gameObject.transform.GetComponent<BulletScript>().shouldBurnEnemies == true)
            {
                coll.gameObject.transform.GetComponent<BulletScript>().burnEnemy(gameObject);
            }
           if(coll.gameObject.transform.GetComponent<BulletScript>().shouldSlowEnemies == true)
            {
                slow();
            }
            Destroy(coll.gameObject);
        }
    }

	public void setMovementSpeed(float movementToSubtract)
	{
        if(canBeSlowed == true && gameObject.transform.GetComponent<EntityStats>().movementSpeed >= backupSpeed)
        {
            float tempMovX = gameObject.transform.GetComponent<Rigidbody2D>().velocity.x - movementToSubtract;
            float tempMovY = gameObject.transform.GetComponent<Rigidbody2D>().velocity.y - movementToSubtract;
            if (gameObject.transform.GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(tempMovX, gameObject.transform.GetComponent<Rigidbody2D>().velocity.y);
            }
            if (gameObject.transform.GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.GetComponent<Rigidbody2D>().velocity.x, tempMovY);
            }
            gameObject.transform.GetComponent<EntityStats>().movementSpeed -= movementToSubtract;
            canBeSlowed = false;
        }
    }   

    public void setMovementSpeedBack(float movementToAdd)
    {
        float tempMovX = gameObject.transform.GetComponent<Rigidbody2D>().velocity.x + movementToAdd;
        float tempMovY = gameObject.transform.GetComponent<Rigidbody2D>().velocity.y + movementToAdd;
        if (gameObject.transform.GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(tempMovX, gameObject.transform.GetComponent<Rigidbody2D>().velocity.y);
        }
        if (gameObject.transform.GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.GetComponent<Rigidbody2D>().velocity.x, tempMovY);
        }
        gameObject.transform.GetComponent<EntityStats>().movementSpeed += movementToAdd;
        canBeSlowed = true;
        takeExtraDamage = false;
    }

    public void setTakeExtraDamage()
    {
        takeExtraDamage = true;
    }

    public void slow()
    {
        slowingTimerBool = true;
        slowingTimer = 150;
        gameObject.transform.GetComponent<EntityStats>().movementSpeed = gameObject.transform.GetComponent<EntityStats>().movementSpeed - gameObject.transform.GetComponent<EntityStats>().movementSpeed / 5;
        gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.GetComponent<Rigidbody2D>().velocity.x - gameObject.transform.GetComponent<Rigidbody2D>().velocity.x / 5, gameObject.transform.GetComponent<Rigidbody2D>().velocity.y - gameObject.transform.GetComponent<Rigidbody2D>().velocity.y / 5);
    }

    
}
