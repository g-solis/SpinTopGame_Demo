using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTopPhysics : MonoBehaviour
{
    [SerializeField] Vector3 spinVelocity = Vector3.zero;
    [SerializeField] float impulseOnHit = 100;
    [SerializeField] ParticleSystem sparkParticles;

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
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            hitDirection.y = 0;

            Vector3 hitPoint = transform.position;
            if (other.contactCount > 0)
            {
                hitPoint = other.contacts[0].point;
            }

            other.rigidbody.AddForce(hitDirection * impulseOnHit, ForceMode.Impulse);
            PlaySparkParticles(hitDirection, hitPoint);
        }
    }

    void PlaySparkParticles(Vector3 hitDirection, Vector3 hitPosition)
    {
        if (sparkParticles != null)
        {
            ParticleSystem instance = Instantiate(sparkParticles, hitPosition, Quaternion.Euler(hitDirection));
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }
}
