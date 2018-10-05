using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SynergyTestScript : MonoBehaviour {

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.transform.parent.gameObject.transform.tag == "Tower" && gameObject.transform.parent.gameObject.transform.GetChild(0).transform.GetComponent<TowerController>().towerLevel == 3 && coll.transform.GetComponent<TowerController>().towerLevel == 3 && coll.gameObject != gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject && gameObject.transform.tag != "GrenadeLauncher")
        {
            if(coll.gameObject.tag != gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject.transform.tag)
            {
                gameObject.transform.parent.gameObject.transform.GetChild(0).transform.GetComponent<TowerController>().towerToSynergizeWith = coll.gameObject;
                gameObject.transform.parent.gameObject.transform.GetChild(0).transform.GetComponent<TowerController>().SynergyAvailable = true;
            }
        }
    }
    void Update()
    {
        if(gameObject.transform.GetComponent<BoxCollider2D>().enabled == false && UIController.staticTowerBeingSpawned == false)
        {
            gameObject.transform.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    void OnMouseDown()
    {
        if (gameObject.transform.parent.transform.GetChild(0).transform.GetComponent<TowerController>().canBeSelected == true && gameObject.transform.parent.transform.GetChild(0).transform.GetComponent<TowerController>().isSynergy == false)
        {
            GameController.setUpgradeLevelText(gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject);
            WaveGen.callWaveButtonStatic.SetActive(false);
        }
    }
}
