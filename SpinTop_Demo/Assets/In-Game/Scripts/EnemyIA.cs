using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    [SerializeField] Vector2 delayChangeDirectionRange = new Vector2(0.5f,1);
    Vector3 currentRandomDirection;
    float changeDirectionDelay = -1;
    float changeDirectionCounter = 0f;

    [Header("Run From Player Movement")]
    [SerializeField] float maxDistanceFromPlayer;

    [Header("Avoid Falling")]
    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;
    [SerializeField] Transform upPos;
    [SerializeField] Transform downPos;
    [SerializeField] LayerMask whatIsArena;
    [SerializeField] float maxDistanceRay = 10f;
    [SerializeField] float delayBeforeHit = 2f;
    bool hasEnteredArena = false;
    bool shoudAvoidFall = true;
    float hitTakenCounter = 0;
    
    float timeCounter = 0f;

    Rigidbody rb;
    Transform player;
    Transform enemySpawner;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().transform;
        enemySpawner = FindObjectOfType<EnemySpawner>().transform;
    }

    void Start() 
    {
        currentRandomDirection = GetRandomDirectionalVector();
    }

    void Update() 
    {

    }

    void FixedUpdate() 
    {
        AvoidFalling();

        if (!hasEnteredArena)
        {
            return;
        }

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
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;
            rb.AddForce(directionToPlayer * speedIA, ForceMode.Force);
        }
        else
        {
            RandomMovement();
        }
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

        if (changeDirectionCounter >= changeDirectionDelay)
        {
            currentRandomDirection = GetRandomDirectionalVector();
            changeDirectionCounter = 0f;
            changeDirectionDelay = Random.Range(delayChangeDirectionRange.x,delayChangeDirectionRange.y);
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
        if (player != null)
        {
            float distanceFromPlayer = Vector3.Distance(player.position, transform.position);

            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;

            if (distanceFromPlayer < maxDistanceFromPlayer)
            {
                rb.AddForce(-directionToPlayer * speedIA, ForceMode.Force);
            }
        }
        else
        {
            RandomMovement();
        }
    }

    void AvoidFalling()
    {
        if (hitTakenCounter > delayBeforeHit)
        {
            shoudAvoidFall = true;
        }
        else
        {
            hitTakenCounter += Time.deltaTime;
        }

        bool hitRight = Physics.Raycast(rightPos.position, -transform.up, maxDistanceRay, whatIsArena);
        bool hitLeft = Physics.Raycast(leftPos.position, -transform.up, maxDistanceRay, whatIsArena);
        bool hitUp = Physics.Raycast(upPos.position, -transform.up, maxDistanceRay, whatIsArena);
        bool hitDown = Physics.Raycast(downPos.position, -transform.up, maxDistanceRay, whatIsArena);

        if (!hitRight && !hitLeft && !hitUp && !hitDown)
        {
            shoudAvoidFall = false;
        }

        if (!hitRight || !hitLeft || !hitUp || !hitDown) 
        {
            if (hasEnteredArena && shoudAvoidFall)
            {
                Vector3 scapeDirection = enemySpawner.position - transform.position;

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero; 
                rb.AddForce(scapeDirection.normalized * 100, ForceMode.Impulse);
            }
        }
        else if(!hasEnteredArena)
        {
            hasEnteredArena = true;
        }
    }

    public void StopAvoidingFallForATime()
    {
        shoudAvoidFall = false;
        hitTakenCounter = 0;
    }
}
