using UnityEngine;
using System.Collections;

public class EntityStats : MonoBehaviour {

    public int amountToSpawn;
    public int amountToSpawnMin;
    public int amountToSpawnMax;
    public int price;
    public string TowerTypeString; //Tower type & enemy type are the child's tag
    public int TowerTypeInt;
    public static string TowerTypeStatic;
    public string TowerGenre;
    public string EnemyType;
    public static string EnemyTypeStatic;

	public int bulletSpeed;
	public int coolDown;
    public int rotateSpeed;
    public int attackDamage;
    public int lingeringDamage;
    public int lingeringDamageTime;
    public bool shootsBurningBullets;
    public bool shootsSlowingBullets;
	public float movementSpeed;
	public int rangeInt;
	public float freezeTowerSlowLevel;
	public int health;
    public int dropMoney;
	public int enemyDropMoney;

	public bool canShoot;

	public GameObject LastBulletCollided;

    public Color spriteColor;

    // Use this for initialization
    void Start () {
		enemyDropMoney = gameObject.transform.GetComponent<EntityStats> ().dropMoney;
        spriteColor = gameObject.transform.GetComponent<SpriteRenderer>().color;
        rotateSpeed = 5; //Default rotate speed
		/*if (gameObject.transform.parent.transform.tag == "Tower" && gameObject.transform.GetComponent<TowerController> ().targetGameObject != null) {
			canShoot = true;
			// && GameController.waveInProgress == true
		}*/

        EnemyType = gameObject.transform.tag;
        TowerTypeString = gameObject.transform.tag;
        if(TowerTypeInt > 0 && TowerTypeInt <= 3)
        {
            TowerGenre = "Damage";
        }
        if (TowerTypeInt > 3 && TowerTypeInt <= 5)
        {
            TowerGenre = "Defense";
        }
        if (TowerTypeInt == 6)
        {
            TowerGenre = "Support";
        }
    }
	
	// Update is called once per frame
	void Update () {
        TowerTypeStatic = TowerTypeString;
        EnemyTypeStatic = EnemyType;
	}

	public void amountToSpawnRangeSetup()
	{
		amountToSpawn = Random.Range(amountToSpawnMin, amountToSpawnMax);
	}
}