using UnityEngine;
using System.Collections;

public class PlaceTestScript : MonoBehaviour {
    public GameObject objCollidingWith;

    void OnTriggerStay2D(Collider2D coll)
    {
        objCollidingWith = coll.gameObject;
        if (coll.gameObject.transform.parent.gameObject.transform.tag == "Tower" || coll.gameObject.transform.tag == "UI" || coll.gameObject.transform.tag == "Path" || coll.gameObject.transform.tag == "PathNoDirection" && coll.gameObject != gameObject)
        {
            gameObject.transform.parent.gameObject.transform.GetChild(0).transform.GetComponent<TowerController>().canBePlaced = false;
        }
        else
        {
            gameObject.transform.parent.gameObject.transform.GetChild(0).transform.GetComponent<TowerController>().canBePlaced = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        objCollidingWith = null;
        if (coll.gameObject.transform.parent.gameObject.transform.tag == "Tower" || coll.gameObject.transform.tag == "UI" || coll.gameObject.transform.tag == "Path" || coll.gameObject.transform.tag == "PathNoDirection")
        {
            gameObject.transform.parent.gameObject.transform.GetChild(0).transform.GetComponent<TowerController>().canBePlaced = true;
        }
    }
}
