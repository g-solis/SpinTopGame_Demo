using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] float speedIA;
    [SerializeField] bool shouldFollowPlayer = false;
    [SerializeField] bool shouldRotateAround = false;
    [SerializeField] bool shouldBeRandom = false;
    [SerializeField] bool shouldRunFromPlayer = false;

    [Header("Circular Movement")]
    [SerializeField] float circleWidth;
    [SerializeField] float circleHeight;

    [Header("Random Movement")]
    [SerializeField] float delayChangeDirection = 1f;
    Vector3 currentRandomDirection;
    float changeDirectionCounter = 0f;

    [Header("Run From Player Movement")]
    [SerializeField] float maxDistanceFromPlayer;
    
    float timeCounter = 0f;

    Rigidbody rb;
    Transform player;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    void Start() 
    {
        currentRandomDirection = GetRandomDirectionalVector();
    }

    void FixedUpdate() 
    {
        if (shouldFollowPlayer)
        {
            FollowPlayer();
        }
        else if (shouldRotateAround)
        {
            RotateAroundTheArena();
        }
        else if (shouldBeRandom)
        {
            RandomMovement();
        }
        else if (shouldRunFromPlayer)
        {
            RunFromPlayer();
        }
    }

    void FollowPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        rb.AddForce(directionToPlayer * speedIA, ForceMode.Force);
    }

    void RotateAroundTheArena()
    {
        float period = (2 * Mathf.PI); //period = 2PI/|B| where f(x) = ASin(Bx) + C

        if (timeCounter >= period)
        {
            timeCounter = 0;
        }

        timeCounter += Time.fixedDeltaTime;

        float x = Mathf.Cos(timeCounter) * circleWidth;
        float y = 0f;
        float z = Mathf.Sin(timeCounter) * circleHeight;

        Vector3 circularDirection = new Vector3(x, y, z);

        rb.AddForce(circularDirection * speedIA, ForceMode.Force);
    }

    void RandomMovement()
    {
        changeDirectionCounter += Time.fixedDeltaTime;

        if (changeDirectionCounter >= delayChangeDirection)
        {
            currentRandomDirection = GetRandomDirectionalVector();
            changeDirectionCounter = 0f;
        }

        rb.AddForce(currentRandomDirection * speedIA, ForceMode.Force);
    }

    Vector3 GetRandomDirectionalVector()
    {
        Vector2 randomPointOnCircle = Random.insideUnitCircle.normalized;
        float randomX = randomPointOnCircle.x;
        float randomZ = randomPointOnCircle.y;
        float randomY = 0f;

        return new Vector3(randomX, randomY, randomZ);
    }

    void RunFromPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (distanceFromPlayer < maxDistanceFromPlayer)
        {
            rb.AddForce(-directionToPlayer * speedIA, ForceMode.Force);
        }
    }
}
