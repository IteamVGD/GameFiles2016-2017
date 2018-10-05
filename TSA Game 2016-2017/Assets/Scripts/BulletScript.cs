using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public int bulletDamage;
    public bool shouldBurnEnemies;
    public int burnDamage;
    public int burnTime;
    public bool shouldSlowEnemies;

    void Update()
    {
        if (gameObject.transform.position.x < GameController.borderTopLeft.transform.position.x)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.x > GameController.borderBottomRight.transform.position.x)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.y > GameController.borderTopLeft.transform.position.y)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.y < GameController.borderBottomRight.transform.position.y)
        {
            Destroy(gameObject);
        }
    }

    public void burnEnemy(GameObject enemy)
    {
        enemy.transform.GetComponent<EnemyController>().burningDamageLocal = burnDamage;
        enemy.transform.GetComponent<EnemyController>().burningTimeLocal = burnTime;
        enemy.transform.GetComponent<EnemyController>().burningTimerBool = true;
    }
}
