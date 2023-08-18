using UnityEngine;

public class CharacterAutoRunner : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private InputService input;

    private const float SlopeGravityFactor = 1f / 125f;
    private const float CrouchOffset = 0.2f;

    private bool isJumpStarted;
    private bool isCrouched;
    private bool isCollide;
    private float initialRunningSpeed;
    private float playerHeight;

    private CustomTimer collisionTimer;
    private RunStatsService runStatsService;

    private enum STATE
    {
        IDLE,
        RUN,
        LEFT,
        RIGHT,
        JUMP,
        CROUCH,
        DEAD
    }

    private STATE currentState;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        input = ServiceLocator.Instance.GetService<InputService>();

        isJumpStarted = false;
        initialRunningSpeed = runningSpeed;
        currentState = STATE.RUN;

        collisionTimer = new CustomTimer();
        collisionTimer.Init();

        runStatsService = ServiceLocator.Instance.GetService<RunStatsService>();

        playerHeight = GetComponent<BoxCollider>().bounds.size.y;
    }

    private void Update()
    {
      
        collisionTimer.Update();

      
       HandleForwardMovement();
        HandleHorizontalMovement();
        
        
        HandleJumping();
        HandleVerticalMovement();

        HandleCrouching();
        
        HandleAnimator();

        HandleCollisionExit();
    
        
        
    }

    private void HandleHorizontalMovement()
    {
        float horizontalInput = input.GetHorizontalAxis();

        if (!isCrouched && !isJumpStarted)
        {
            if (horizontalInput < 0)
            {
                currentState = STATE.LEFT;
            }
            else if (horizontalInput > 0)
            {
                currentState = STATE.RIGHT;
            }
        }

        Vector3 horizontalMovement = new Vector3(horizontalInput * speed * Time.fixedDeltaTime, 0, 0);
        rb.AddForce(horizontalMovement, ForceMode.VelocityChange);
    }

    private void HandleForwardMovement()
    {
        if (!isCollide)
        {
            Vector3 forwardMovement = new Vector3(0, 0, runningSpeed * Time.fixedDeltaTime);
            rb.AddForce(forwardMovement, ForceMode.VelocityChange);
        }
    }

    private void HandleJumping()
    {
        if (input.GetKey(InputService.ActionKey.up))
        {
            if (!isJumpStarted)
            {
                rb.AddForce(transform.up * 900, ForceMode.Impulse);
                isJumpStarted = true;
            }
            runningSpeed = initialRunningSpeed * 0.4f;
            currentState = STATE.JUMP;
        }

        if (transform.localPosition.y < 0f)
        {
            if (isJumpStarted)
            {
                isJumpStarted = false;
                currentState = STATE.RUN;
            }
        }

             
    }

    private void HandleCrouching()
    {
        if (input.GetKey(InputService.ActionKey.down))
        {
            if (!isCrouched)
            {
                GetComponent<CapsuleCollider>().center = new Vector3(
                    GetComponent<CapsuleCollider>().center.x,
                    GetComponent<CapsuleCollider>().center.y - CrouchOffset,
                    GetComponent<CapsuleCollider>().center.z
                );
                currentState = STATE.CROUCH;
                isCrouched = true;
                isCollide = false;
            }
        }
        else if (input.GetKeyUp(InputService.ActionKey.down))
        {
            if (isCrouched)
            {
                GetComponent<CapsuleCollider>().center = new Vector3(
                    GetComponent<CapsuleCollider>().center.x,
                    GetComponent<CapsuleCollider>().center.y + CrouchOffset,
                    GetComponent<CapsuleCollider>().center.z
                );

                isCrouched = false;
            }
        }
    }

 

    private void HandleVerticalMovement()
    {
        if (OnSlope())
        {
            Vector3 slopeGravity = Vector3.down * (Physics.gravity.magnitude * SlopeGravityFactor);
           rb.AddForce(slopeGravity, ForceMode.Acceleration);
        }
        if (transform.localPosition.y > 6f)
        {
            rb.AddForce(transform.up * -300, ForceMode.Impulse);
        }
    }

    private void HandleCollisionExit()
    {
        if (transform.position.y < -0.08f)
        {
            Vector3 clamp = transform.position;
            clamp.y = 0;
            transform.position = clamp;
        }
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);

        if (!isJumpStarted && !isCrouched && !isCollide)
        {
            currentState = STATE.RUN;
            runningSpeed = initialRunningSpeed;
        }
    }

    private void HandleAnimator()
    {
        animator.SetBool(STATE.IDLE.ToString(), currentState == STATE.IDLE);
        animator.SetBool(STATE.RUN.ToString(), currentState == STATE.RUN);
        animator.SetBool(STATE.LEFT.ToString(), currentState == STATE.LEFT);
        animator.SetBool(STATE.RIGHT.ToString(), currentState == STATE.RIGHT);
        animator.SetBool(STATE.JUMP.ToString(), currentState == STATE.JUMP);
        animator.SetBool(STATE.CROUCH.ToString(), currentState == STATE.CROUCH);
        animator.SetBool(STATE.DEAD.ToString(), currentState == STATE.DEAD);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, playerHeight / 2 + 0.5f))
        {
            float slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            return slopeAngle > 5 && slopeAngle < 45;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeadZone"))
        {
            isCollide = true;
            runningSpeed = 0;
            currentState = STATE.IDLE;

            if (!collisionTimer.TimerIsStarted)
            {
                collisionTimer.Start();
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeadZone"))
        {
            isCollide = true;
            runningSpeed = 0;
            currentState = STATE.IDLE;

            if (collisionTimer.GetFloatValue() >= 0.4f)
            {
                runStatsService.UserLife -= 1;
                collisionTimer.Stop();
            }


        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeadZone"))
        {
            isCollide = false;
            collisionTimer.Stop();
            runningSpeed = initialRunningSpeed;
            Vector3 exitDirection = -collision.contacts[0].normal;
            float exitDistance = 0.1f;
            transform.position += exitDirection * exitDistance;
        }
   
    }

    public void ResetCharacter()
    {
        gameObject.SetActive(true);
        runningSpeed = initialRunningSpeed;
        transform.localPosition = Vector3.zero;
        enabled = true;
        isJumpStarted = false;
        isCrouched = false;
        isCollide = false;
    }
}