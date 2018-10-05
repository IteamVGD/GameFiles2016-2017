using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuPathScript : MonoBehaviour {

    public GameObject StartingMenuPath;
    public GameObject EndingMenuPath;
    public GameObject BasicEnemy;
    public GameObject MainMenu;
    public List<GameObject> basicEnemiesSpawned;


    public int spawningtimer;

    // Use this for initialization
    void Start () {
        spawningtimer = 100;
	}
	
	// Update is called once per frame
	void Update () {
        foreach(GameObject obj in basicEnemiesSpawned)
        {
            if(obj == null)
            {
                basicEnemiesSpawned.Remove(obj);
            }
        }
        spawningtimer += 1;
        if(spawningtimer == 150)
        {
            spawnThings();
            spawningtimer = 0;
        }
        if(MainMenu.activeSelf == false)
        {
            foreach(GameObject obj in basicEnemiesSpawned)
            {
                DestroyImmediate(obj.gameObject);
                Destroy(obj);
            }
            gameObject.SetActive(false);
        }
	
	}
    public void spawnThings()
    {
        GameObject instantiatedEntity = GameObject.Instantiate(BasicEnemy, StartingMenuPath.transform.position, transform.rotation) as GameObject;
        instantiatedEntity.SetActive(true);
        instantiatedEntity.transform.GetChild(0).gameObject.SetActive(false);
        basicEnemiesSpawned.Add(instantiatedEntity);
    }

}
