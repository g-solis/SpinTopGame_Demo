using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    struct Enemy
    {
        public GameObject Prefab;
        public int Weight;
        public int Cost;
        // public Restriction Restriction;
    }

    // [System.Flags]
    // [System.Serializable]
    // enum Restriction
    // {
    //     None = 0,
    //     Holes = 1,
    //     Walls = 2,
    //     Mounts = 4
    // }

    [Header("Pool System")]
    [SerializeField] bool canSpawn;
    [SerializeField] float creditsRegenRate = 2;
    [SerializeField] Vector2 timeBetweenSpawnsRange = new Vector2(5,10);
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    [Header("Spawn Parameters")]
    [SerializeField] float spawnHeight = 20f;
    [SerializeField] float startImpulse = 20f;
    [SerializeField] float spawnRadius = 10;
    
    float credits = 5;

    Transform playerTransform;

    public void IncreaseCreditsRegen(float ammount)
    {
        creditsRegenRate += ammount;
    }

    void Awake() 
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    //TODO: Remove, Debug Only
    void OnDebug1()
    {
        IncreaseCreditsRegen(1);
    }

    void Update()
    {
        credits += creditsRegenRate * Time.deltaTime;
    }

    void OnEnable()
    {
        canSpawn = true;
        StartCoroutine(SpawnEnemies());
    }
    

    void OnDisable()
    {
        canSpawn = false;
        StopAllCoroutines();
    }

    IEnumerator SpawnEnemies()
    {
        while (canSpawn)
        {
            List<int> pool = new List<int>();
            
            for(int i=0;i<enemies.Count;i++)
            {
                pool.AddXTimes(i,enemies[i].Weight);
            }

            Enemy enemy = enemies[pool.Random()];

            while(credits >= enemy.Cost)
            {
                credits -= enemy.Cost;

                Vector2 spawnPositionBounds = RandomUtils.RandomPointOnCircleEdge(spawnRadius);
                Vector3 spawnPositon = new Vector3(spawnPositionBounds.x, spawnHeight, spawnPositionBounds.y);
                Vector3 directionToCenter = transform.position - spawnPositon;

                GameObject instance = Instantiate(enemy.Prefab, spawnPositon, Quaternion.identity);
                instance.GetComponent<Rigidbody>().AddForce(directionToCenter * startImpulse, ForceMode.Impulse);
            }


            yield return new WaitForSeconds(RandomUtils.Vector2Range(timeBetweenSpawnsRange));
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
