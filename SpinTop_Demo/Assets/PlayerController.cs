using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] 
    Vector2 moveSpeed = Vector2.zero;

    [SerializeField]
    ForceMode forceMode;
    
    Vector2 inputDirection = Vector2.zero;
    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() 
    {
        Vector2 resultForce = moveSpeed * inputDirection;
        Vector3 moveVector = new Vector3(resultForce.x, 0f, resultForce.y);
        rb.AddForce(moveVector, forceMode);
    }

    void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }
}
