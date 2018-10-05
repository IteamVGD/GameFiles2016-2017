using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrenadeScript : MonoBehaviour {

    public int explodingTimer;
    public bool atEnemyPos;
    public int enemyPosX;
    public int enemyPosY;
    public int currentPosXInt;
    public int currentPosYInt;
    public GameObject explodingAnimObj;
    public int grenadeDamage;
    public int towerLevel;
    public List<GameObject> EnemiesToDamage;
    public bool possibleEnemyX;
    public bool possibleEnemyY;
    public int explosionSizeLocal;
	public bool smallGrenade;

    public Vector2 grenadeVelocity;
    public bool isMovingToTarget;

    public bool shouldBurnEnemies;
    public bool shouldSlowEnemies;
    public int burnTime;
    public int burnDamage;

    public GameObject bulletPrefab;

    public GameObject towerShotFrom;

    //public int suicideTimer; //Plan HardR

    // Use this for initialization
    void Start () {
        //suicideTimer = 750;
        isMovingToTarget = true;
        if(explosionSizeLocal == 2)
        {
            gameObject.transform.GetComponent<BoxCollider2D>().size = new Vector2(8.5f, 8.5f);
        }
        atEnemyPos = false;
	}
	
	// Update is called once per frame
	void Update () {
        /*if(suicideTimer > 0)
        {
            suicideTimer -= 1;
        }
        else
        {
            DestroyImmediate(gameObject);
        }*/
        if(towerShotFrom == null)
        {
            Destroy(gameObject);
        }
        grenadeVelocity = gameObject.transform.GetComponent<Rigidbody2D>().velocity;
        currentPosXInt = (int)gameObject.transform.position.x;
        currentPosYInt = (int)gameObject.transform.position.y;
        if(currentPosXInt + 1 >= enemyPosX && currentPosXInt - 1 <= enemyPosX && currentPosYInt + 1 >= enemyPosY && currentPosYInt - 1 <= enemyPosY)
        {
            atEnemyPos = true;
        }
	    if(atEnemyPos == true)
        {
            isMovingToTarget = false;
            gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            if (explodingTimer >= 75)
            {
                foreach(GameObject obj in EnemiesToDamage)
                {
                    if(obj == null)
                    {
                        EnemiesToDamage.Remove(obj);
                    }
                    else
                    {
                        if (shouldBurnEnemies == true)
                        {
                            obj.gameObject.transform.GetComponent<EnemyController>().burningTimeLocal = burnTime;
                            obj.gameObject.transform.GetComponent<EnemyController>().burningDamageLocal = burnDamage;
                            obj.gameObject.transform.GetComponent<EnemyController>().burningTimerBool = true;
                        }
                        if (shouldSlowEnemies == true)
                        {
                            obj.transform.GetComponent<EnemyController>().slow();
                        }
                        obj.transform.GetComponent<EntityStats>().health -= grenadeDamage;
                    }
                }
				towerShotFrom.transform.GetComponent<AudioSource>().PlayOneShot(towerShotFrom.transform.GetComponent<AudioSource>().clip, GameController.staticSFXVolume);
                Destroy(gameObject);  
            }
            else
            {
                explodingTimer += 1;
            }
        }

		if (smallGrenade == true) 
		{
			if(atEnemyPos == true)
			{
				isMovingToTarget = false;
				gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
				if (explodingTimer >= 20)
				{
					foreach(GameObject obj in EnemiesToDamage)
					{
						if(obj == null)
						{
							EnemiesToDamage.Remove(obj);
						}
						else
						{
                            if (shouldSlowEnemies == true)
                            {
                                obj.transform.GetComponent<EnemyController>().slow();
                            }
                            obj.transform.GetComponent<EntityStats>().health -= grenadeDamage;
                        }
					}
					towerShotFrom.transform.GetComponent<AudioSource>().PlayOneShot(towerShotFrom.transform.GetComponent<AudioSource>().clip, GameController.staticSFXVolume);
                    Destroy(gameObject);  
				}
				else
				{
					explodingTimer += 1;
				}
			}
		}
	}
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == 10)
        {
            EnemiesToDamage.Add(coll.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 10)
        {
            EnemiesToDamage.Remove(coll.gameObject);
        }
    }
}
