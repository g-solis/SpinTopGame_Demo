using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTopPhysics : MonoBehaviour
{
    [SerializeField]
    Vector3 spinVelocity = Vector3.zero;

    [SerializeField]
    float impulseOnHit = 100;

    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    void FixedUpdate() 
    {
        // rb.AddTorque(spinVelocity, ForceMode.Force);
    }

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.GetComponent<SpinTopPhysics>() != null)
        {
            Debug.Log(transform.name);
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            hitDirection.y = 0;

            other.rigidbody.AddForce(hitDirection * impulseOnHit, ForceMode.Impulse);
        }
    }
}
