using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutorunDebug : MonoBehaviour
{
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private bool isJumpStarted;
    private bool isCrouched;

    private enum STATE
    {
        IDLE,
        RUN,
        JUMP,
        CROUCH
    }

    private STATE currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentState = STATE.RUN;
    }

    private void Update()
    {
        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isJumpStarted)
            {
                currentState = STATE.JUMP;
                isJumpStarted = true;
                rb.AddForce(transform.up * 900, ForceMode.Impulse);
            }
        }
        else if (Input.GetKeyUp(KeyCode.S) || transform.position.y < 0.01f)
        {
            isJumpStarted = false;
        }

                // Handle crouch input
                if (Input.GetKeyDown(KeyCode.S))
        {
            isCrouched = true;
            currentState = STATE.CROUCH;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            isCrouched = false;
        }

                if (!isCrouched && !isJumpStarted)
        {
            currentState = STATE.RUN;
        }

    }

    private void FixedUpdate()
    {
        HandleForwardMovement();
      //  HandleJumping();
       // HandleCrouching();
        HandleAnimator();
    }

    private void HandleForwardMovement()
    {
        Vector3 forwardMovement = new Vector3(0, 0, runningSpeed * Time.deltaTime);
        rb.AddForce(forwardMovement, ForceMode.VelocityChange);
    }

    private void HandleJumping()
    {
        if (isJumpStarted && transform.localPosition.y < 0.1f)
        {
            isJumpStarted = false;
        }
    }

    private void HandleCrouching()
    {
        // Handle crouching logic here
    }

    private void HandleAnimator()
    {
        animator.SetBool(STATE.RUN.ToString(), currentState == STATE.RUN);
        animator.SetBool(STATE.JUMP.ToString(), currentState == STATE.JUMP);
        animator.SetBool(STATE.CROUCH.ToString(), currentState == STATE.CROUCH);
    }
}
