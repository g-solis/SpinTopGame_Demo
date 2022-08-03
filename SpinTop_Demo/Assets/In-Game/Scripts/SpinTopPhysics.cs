using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTopPhysics : MonoBehaviour
{
    [SerializeField] Vector3 spinVelocity = Vector3.zero;
    [SerializeField] float impulseOnHit = 100;

    Rigidbody rb;
    bool dead = false;
    SpinTopAnimation animator = null;

    public void Kill()
    {
        dead = true;
        animator?.PlayDeathAnimation(transform.position);
        SoundManager.PlaySFX(ConstantDatabase.SoundDatabase.SpinTopExplosionSFX,transform.position);
        Destroy(gameObject);
    }

    void Awake() 
    {
        animator = GetComponent<SpinTopAnimation>();
        rb = GetComponentInChildren<Rigidbody>();
    }

    // void FixedUpdate() 
    // {
    //     if(!dead)
    //         rb.AddTorque(spinVelocity, ForceMode.Force);
    // }

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.GetComponent<SpinTopPhysics>() != null && !dead)
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
            animator?.PlaySparkParticles(hitDirection, hitPoint);
            EnemyIA enemy = other.gameObject.GetComponent<EnemyIA>();

            if (enemy != null)
            {
                enemy.StopAvoidingFallForATime();
            }

            // To make that only one of the spintops plays the sound
            if(gameObject.GetInstanceID() > other.gameObject.GetInstanceID())
            {
                SoundManager.PlaySFX(ConstantDatabase.SoundDatabase.SpinTopCollideSFX,transform.position);
            }
        }
    }
}
