using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Vector2 moveSpeed;

    [SerializeField] ForceMode forceMode;
    [SerializeField] float dashSpeed = 10;
    [SerializeField] float dashDelay = 0.5f;
    [SerializeField] float dashDelayCooldown = 2;
    [SerializeField] TextMeshPro timerDash;
    [SerializeField] ParticleSystem dashParticles;
    [SerializeField] Vector3 sparkParticlesOffset;
    bool isDashing = false;
    bool canDash = false;
    bool dashCooldown = false;

    Vector3 dashNewPos;
    Vector2 inputDirection;
    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    void Start() 
    {
        timerDash.text = dashDelay.ToString();
    }

    void FixedUpdate() 
    {
        if (isDashing)
        {
            float timer = float.Parse(timerDash.text) - Time.fixedDeltaTime;
            timerDash.text = timer.ToString();
        }
        else
        {
            timerDash.text = dashDelay.ToString();
        }

        if (!isDashing)
        {
            Vector2 resultForce = moveSpeed * inputDirection;
            Vector3 moveVector = new Vector3(resultForce.x, 0f, resultForce.y);
            rb.AddForce(moveVector, forceMode);
        }

        if (canDash)
        {
            canDash = false;
            isDashing = true;

            bool playerIsPressingInput = Mathf.Abs(inputDirection.x) > Mathf.Epsilon || Mathf.Abs(inputDirection.y) > Mathf.Epsilon;
            
            Vector2 resultForce;
            if (playerIsPressingInput)
            {
                resultForce = dashSpeed * inputDirection.normalized;
            }
            else
            {
                resultForce = dashSpeed * transform.right;
            }

            Vector3 dashVector = new Vector3(resultForce.x, 0f, resultForce.y);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(dashVector, ForceMode.Impulse);
            PlayDashParticles();

            StartCoroutine(DashDelay());
        }
    }

    IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(dashDelay);
        isDashing = false;
    }

    IEnumerator DashDelayCoolDown()
    {
        dashCooldown = true;
        yield return new WaitForSeconds(dashDelayCooldown);
        dashCooldown = false;
    }

    void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }
    
    void OnFire(InputValue value)
    {
        if (value.isPressed && !isDashing && !dashCooldown)
        {
            canDash = true;
            StartCoroutine(DashDelayCoolDown());
        }
    }

    void PlayDashParticles()
    {
        if (dashParticles != null)
        {
            ParticleSystem instance = Instantiate(dashParticles, transform.position + sparkParticlesOffset, Quaternion.Euler(inputDirection), transform);
            Destroy(instance.gameObject, dashDelay);
        }
    }
}
