using UnityEngine;
using System.Collections;

public class PlanOController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 10)
        {
           Destroy(coll.gameObject);
        }
    }
}
