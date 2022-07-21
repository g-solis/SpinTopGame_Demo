using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTopAnimation : MonoBehaviour
{
    [SerializeField]
    Transform SpinTopParent;

    [SerializeField]
    AnimationCurve Curve;

    [SerializeField]
    float MaxVelocity = 500;

    [SerializeField]
    [Range(0.0f,45.0f)]
    float MaxAngle = 45;

    void FixedUpdate()
    {
        Vector3 vect = GetComponentInChildren<Rigidbody>().velocity;
        vect.x = Curve.Evaluate(Mathf.Min(vect.x/MaxVelocity,MaxVelocity)) * MaxAngle;
        vect.y = 0;
        vect.z = Curve.Evaluate(Mathf.Min(vect.z/MaxVelocity,MaxVelocity)) * MaxAngle;
        SpinTopParent.Rotate(Vector3.up * 2);
        transform.rotation = Quaternion.Euler(vect);
    }
}
