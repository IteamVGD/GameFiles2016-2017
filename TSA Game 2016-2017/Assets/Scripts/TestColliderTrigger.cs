using UnityEngine;
using System.Collections;

public class TestColliderTrigger : MonoBehaviour {

    public bool test;

	// Use this for initialization
	void Start () {
        test = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        test = true;
        Destroy(gameObject);
    }
}
