using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [SerializeField]
    float speedIAForward;

    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() 
    {
        rb.AddForce(transform.forward * speedIAForward, ForceMode.Force);
    }
}
