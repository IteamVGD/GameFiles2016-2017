using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameScript : MonoBehaviour {

    public int flameDamage;
    public int lingeringFlameDamage;
    public int lingeringFlameTime;
    public List<GameObject> EnemiesInCone;
    public int timer;
	public bool freezeFire;

	// Use this for initialization
	void Start () {
        timer = 20;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.GetComponent<AudioSource> ().volume = GameController.staticSFXVolume;
	    if(EnemiesInCone.Count != 0 && timer > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.transform.GetComponent<AudioSource>().enabled = true;
            timer -= 1;
        }
        if(timer <= 0)
        {
            foreach(GameObject obj in EnemiesInCone)
            {
                if (obj == null)
                {
                    EnemiesInCone.Remove(obj);
                }
                else
                {
                    obj.transform.GetComponent<EntityStats>().health -= flameDamage;
                    obj.transform.GetComponent<EnemyController>().burningDamageLocal = lingeringFlameDamage;
                    obj.transform.GetComponent<EnemyController>().burningTimeLocal = lingeringFlameTime;
                    obj.transform.GetComponent<EnemyController>().burningTimerBool = true;
					if (freezeFire == true) 
					{
						obj.transform.GetComponent<EntityStats> ().movementSpeed -= obj.transform.GetComponent<EntityStats> ().movementSpeed / 6;
					}
                }
            }
            timer = 20;
        }
        if(EnemiesInCone.Count == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.GetComponent<AudioSource>().enabled = false;
        }
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        EnemiesInCone.Add(coll.gameObject);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(EnemiesInCone.Contains(coll.gameObject))
        {
            EnemiesInCone.Remove(coll.gameObject);
            coll.gameObject.transform.GetComponent<EntityStats>().movementSpeed = coll.gameObject.transform.GetComponent<EnemyController>().backupSpeed;
        }
    }
}
