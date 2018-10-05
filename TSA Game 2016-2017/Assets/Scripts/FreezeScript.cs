using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreezeScript : MonoBehaviour {

    public float slowLevel;
    public float newVelX;
    public float newVelY;

    public int timer;

    public List<GameObject> enemiesSlowed;

	// Use this for initialization
	void Start () {
        slowLevel = gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject.transform.GetComponent<EntityStats>().freezeTowerSlowLevel;
        timer = 50;
    }

    // Update is called once per frame
    void Update()
    {
		gameObject.transform.GetComponent<AudioSource> ().volume = GameController.staticSFXVolume;
        foreach (GameObject coll in enemiesSlowed)
        {
            if(enemiesSlowed.Count > 0)
            {
                gameObject.transform.parent.gameObject.transform.GetChild(4).gameObject.transform.GetComponent<AudioSource>().enabled = true;
            }
            if (coll.gameObject.transform.GetComponent<EnemyController>().canBeFrozen == true)
            {
                if (coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    newVelX = coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x;
                    float newNewVelX = newVelX - slowLevel;
                    coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(newNewVelX, coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y);
                }
                if (coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y > 0)
                {
                    newVelY = coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y;
                    float newNewVelY = newVelY - slowLevel;
                    coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x, newNewVelY);
                }
                if (coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x < 0)
                {
                    newVelX = coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x;
                    float newNewVelX = newVelX + slowLevel;
                    coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(newNewVelX, coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y);
                }
                if (coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y < 0)
                {
                    newVelY = coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y;
                    float newNewVelY = newVelY + slowLevel;
                    coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x, newNewVelY);
                }
                coll.gameObject.transform.GetComponent<EnemyController>().canBeFrozen = false;
            }
        }

        if (timer <= 0 && timer >= -3 && UIController.staticTowerBeingSpawned == false)
        {
            enemiesSlowed.Clear();
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
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 10 && enemiesSlowed.Contains(coll.gameObject) == false)
        {
            enemiesSlowed.Add(coll.gameObject);
        }
    }

    void OnMouseDown()
    {
        if (gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject.transform.GetComponent<TowerController>().canBeSelected == true && gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject.transform.GetComponent<TowerController>().isSynergy == false)
        {
            GameController.setUpgradeLevelText(gameObject);
            WaveGen.callWaveButtonStatic.SetActive(false);
        }
    }

    /*void speedUpEnemies()
    {
        foreach (GameObject coll in enemiesSlowed)
        {
                if (coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    float newNewVelX = coll.gameObject.transform.GetComponent<EnemyController>().backupSpeed;
                    coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(newNewVelX, coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y);
                }
                if (coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.y > 0)
                {
                    float newNewVelY = coll.gameObject.transform.GetComponent<EnemyController>().backupSpeed;
                    coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity.x, newNewVelY);
                }
                coll.gameObject.transform.GetComponent<EnemyController>().canBeFrozen = true;
        }
        enemiesSlowed.Clear();
    }*/
}
