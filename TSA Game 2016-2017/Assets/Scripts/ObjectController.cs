using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectController : MonoBehaviour {

    public string objTag;
    public bool towerFollowsMouse;
    public int towerType; //1-6, set by in inspector
    public GameObject towerToSpawn;
    public List<GameObject> towerTypeList;
    public bool spawningTower;

    // Use this for initialization
    void Start () {
        objTag = gameObject.tag;
	}

    public void OnMouseDown()
    {
        if(objTag == "Tower")
        { 
            selectTower(towerType);
        }
    }

    public void selectTower(int type)
    {
        towerToSpawn = towerTypeList[towerType - 1]; //-1 because list goes from 0-5 but int goes from 1-6
        if(gameObject.GetComponent<EntityStats>().price <= GameController.money)
        {
            Transform instantiatedEntity = GameObject.Instantiate(towerToSpawn, transform.position, transform.rotation) as Transform; //Spawns tower
        }
    }

    public void FixedUpdate()
    {
        if(spawningTower == true)
        {

        }
    }
}
