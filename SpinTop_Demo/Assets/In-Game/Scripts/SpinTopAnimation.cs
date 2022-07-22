using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();

        GameObject FixedPoint = GameObject.Find("Fixed Point");
        if(FixedPoint == null)
            FixedPoint = new GameObject("Fixed Point");

        ConstraintSource c = new ConstraintSource();
        c.sourceTransform = FixedPoint.transform;
        c.weight = 1;
        GetComponentInChildren<RotationConstraint>().AddSource(c);
    }

    void Update()
    {
        Vector3 velocity = rb.velocity;

        velocity.y = 0;
        float magnitude = velocity.magnitude;
        if(magnitude > MaxVelocity)
        {
            velocity = velocity.normalized * MaxVelocity;
            magnitude = MaxVelocity;
        }

        Vector3 vect = velocity;

        vect.x = Mathf.Sign(velocity.z) * Curve.Evaluate(Mathf.Abs(velocity.z)/MaxVelocity) * MaxAngle;
        vect.z = -Mathf.Sign(velocity.x) * Curve.Evaluate(Mathf.Abs(velocity.x)/MaxVelocity) * MaxAngle;

        transform.rotation = Quaternion.Euler(vect);
        SpinTopParent.Rotate(new Vector3(0, (250 + 1000 * magnitude/MaxVelocity) * Time.fixedDeltaTime, 0));
    }
}
