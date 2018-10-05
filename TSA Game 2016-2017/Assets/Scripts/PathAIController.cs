using UnityEngine;
using System.Collections;

public class PathAIController : MonoBehaviour {

    public int directionToMove; //When enemy goes on this path, it moves according to this int (1 = left, 2 = right, 3 = up, 4 = down)
    public static int directionToMoveStatic;

    public int directionToFlipXWhenMovingOnY; //(1 = left, 2 = right)
	
	// Update is called once per frame
	void Update () {
        directionToMoveStatic = directionToMove;
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == 10)
        {
            //Changes enemy's velocity according to path's direction
            if (directionToMove == 1)
            {
                coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-coll.gameObject.transform.GetComponent<EntityStats>().movementSpeed, 0);
                coll.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            if (directionToMove == 2)
            {
                coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(coll.gameObject.transform.GetComponent<EntityStats>().movementSpeed, 0);
                if (directionToFlipXWhenMovingOnY == 1)
                {
                    coll.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            if (directionToMove == 3)
            {
                coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, coll.gameObject.transform.GetComponent<EntityStats>().movementSpeed);
                if (directionToFlipXWhenMovingOnY == 1)
                {
                    coll.gameObject.GetComponent<SpriteRenderer>().flipX = coll.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                if (directionToFlipXWhenMovingOnY == 2)
                {
                    coll.gameObject.GetComponent<SpriteRenderer>().flipX = coll.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            if (directionToMove == 4)
            {
                coll.gameObject.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -coll.gameObject.transform.GetComponent<EntityStats>().movementSpeed);
                if (directionToFlipXWhenMovingOnY == 1)
                {
                    coll.gameObject.GetComponent<SpriteRenderer>().flipX = coll.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                if (directionToFlipXWhenMovingOnY == 2)
                {
                    coll.gameObject.GetComponent<SpriteRenderer>().flipX = coll.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }

            //Teleports the enemy a little bit to decrease offset on the path \/
            if(coll.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                coll.gameObject.transform.position = new Vector2(coll.gameObject.transform.position.x - .3f, coll.gameObject.transform.position.y);
            }
            if (coll.gameObject.transform.position.x < gameObject.transform.position.x)
            {
                coll.gameObject.transform.position = new Vector2(coll.gameObject.transform.position.x + .3f, coll.gameObject.transform.position.y);
            }
            if (coll.gameObject.transform.position.y > gameObject.transform.position.y)
            {
                coll.gameObject.transform.position = new Vector2(coll.gameObject.transform.position.x, coll.gameObject.transform.position.y - .3f);
            }
            if (coll.gameObject.transform.position.y < gameObject.transform.position.y)
            {
                coll.gameObject.transform.position = new Vector2(coll.gameObject.transform.position.x, coll.gameObject.transform.position.y + .3f);
            }

            //Straight up teleportation (jiterry, but textures arent offset) \/
            //coll.gameObject.transform.position = gameObject.transform.position;

            if (gameObject.transform.tag == "EndingPath")
            {
                Destroy(coll.gameObject);
                GameController.health -= 1;
            }

            coll.gameObject.transform.GetComponent<EnemyController>().canBeFrozen = true;
        }
    }
}
