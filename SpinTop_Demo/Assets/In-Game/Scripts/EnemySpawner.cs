using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] bool canSpawn;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float spawnHeight = 20f;
    [SerializeField] float startImpulse = 20f;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] float spawnRadius = 10;

    Transform playerTransform;

    void Awake() 
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
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
            int enemyIndex = Random.Range(0, enemies.Count);
            GameObject enemy = enemies[enemyIndex];

            Vector2 spawnPositionBounds = RandomPointOnUnitCircle(spawnRadius);

            Vector3 spawnPositon = new Vector3(spawnPositionBounds.x, spawnHeight, spawnPositionBounds.y);
            
            GameObject instance = Instantiate(enemy, spawnPositon, Quaternion.identity);

            Vector3 directionToCenter = transform.position - instance.transform.position;

            instance.GetComponent<Rigidbody>().AddForce(directionToCenter * startImpulse, ForceMode.Impulse);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    Vector2 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector2(x, y);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
