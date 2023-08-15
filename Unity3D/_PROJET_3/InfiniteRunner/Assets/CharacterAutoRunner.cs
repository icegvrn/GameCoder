using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;
using static ICharacter;

public class CharacterAutoRunner : MonoBehaviour, ICharacter
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float runningSpeed = 10f;
    private float initialRunningSpeed = 10f;
    [SerializeField]
    private Animator animator;
    public STATE currentState { get; set; }
    public bool isJumpStarted { get; set; }
    public bool isCrouched;
    private bool isCollide;
    private float initialVerticalPosition;
    private CustomTimer collisionTimer;
    private RunStatsService runStatsService;
    bool collisionStarted;

    private bool canExitCollision = true;

    RaycastHit slopeHit;
    float playerHeight;
    Vector3 slopeMoveDirection;
    Vector3 headInitialCenter;
    // Start is called before the first frame update
    void Start()
    {
        isJumpStarted = false;
        initialVerticalPosition = transform.position.y;
        initialRunningSpeed = runningSpeed;
        currentState = STATE.RUN;
        collisionTimer = new CustomTimer();
        collisionTimer.Init();
        runStatsService = ServiceLocator.Instance.GetService<RunStatsService>();
        playerHeight = GetComponent<BoxCollider>().bounds.size.y;
        headInitialCenter = GetComponent<CapsuleCollider>().center;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Contrôle du joueur pour aller à gauche ou à droite
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput < 0)
        {
            currentState = STATE.LEFT;
        }
        else if (horizontalInput > 0)
        {
            currentState = STATE.RIGHT;
        }

        Vector3 horizontalMovement = new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
        rb.AddForce(horizontalMovement, ForceMode.VelocityChange);

   
        Vector3 forwardMovement = new Vector3(0, 0, runningSpeed * Time.deltaTime);
        rb.AddForce(forwardMovement, ForceMode.VelocityChange);

        if (OnSlope()) 
        {
            float slopeGravityFactor = 10f; 
            Vector3 slopeGravity = Vector3.down * (Physics.gravity.magnitude * slopeGravityFactor);
            rb.AddForce(slopeGravity, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (!isJumpStarted)
            {
                currentState = STATE.JUMP;
                isJumpStarted = true;
                GetComponent<Rigidbody>().AddForce(transform.up * 800, ForceMode.Impulse);
            }
        }
    
        else if (Input.GetKey(KeyCode.S))
        {
            currentState = STATE.CROUCH;
            GetComponent<CapsuleCollider>().center = new Vector3(GetComponent<CapsuleCollider>().center.x, GetComponent<CapsuleCollider>().center.y - 0.2f, GetComponent<CapsuleCollider>().center.z);
            isCrouched = false;
        }

        else
        {
            if (isJumpStarted && transform.localPosition.y > 6.5f) { GetComponent<Rigidbody>().AddForce(transform.up * -5, ForceMode.Impulse); }

            else if (isJumpStarted && transform.localPosition.y < 0.5f) { Debug.Log("JE STOPE LE JUMP"); isJumpStarted = false; runningSpeed = initialRunningSpeed; }

            else if (isJumpStarted)
            {
                runningSpeed = initialRunningSpeed * 0.4f;
            }

            else
            {
                if (isCrouched)
                {
                    GetComponent<CapsuleCollider>().center = headInitialCenter;
                    currentState = STATE.RUN;
                    runningSpeed = initialRunningSpeed;
                    isCrouched = false;
                }

            }

        }
   
       


      

        UpdateAnimator();


        collisionTimer.Update();

     
    }

    void FixedUpdate()
    {
        if (!isJumpStarted && !isCrouched && !isCollide)
        {
            currentState = STATE.RUN;
            runningSpeed = initialRunningSpeed;
        }

    }
    void UpdateAnimator()
    {
        animator.SetBool(STATE.IDLE.ToString(), currentState == STATE.IDLE);
        animator.SetBool(STATE.RUN.ToString(), currentState == STATE.RUN);
        animator.SetBool(STATE.LEFT.ToString(), currentState == STATE.LEFT);
        animator.SetBool(STATE.RIGHT.ToString(), currentState == STATE.RIGHT);
        animator.SetBool(STATE.BOOSTED.ToString(), currentState == STATE.BOOSTED);
        animator.SetBool(STATE.JUMP.ToString(), currentState == STATE.JUMP);
        animator.SetBool(STATE.CROUCH.ToString(), currentState == STATE.CROUCH);
        animator.SetBool(STATE.DEAD.ToString(), currentState == STATE.DEAD);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "DeadZone")
        {
            isCollide = true;
            runningSpeed = 0;
            currentState = STATE.IDLE;

            if (!collisionTimer.TimerIsStarted)
            {
                collisionTimer.Start();
            }
            if (collision.gameObject.tag == "DeadZone")
            {
                canExitCollision = false;
            }
           
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "DeadZone")
        {
            isCollide = true;
            runningSpeed = 0;
            currentState = STATE.IDLE;

            if (collisionTimer.GetFloatValue() >= 0.4f)
            {
                runStatsService.UserLife -= 1;
                collisionTimer.Stop();
                canExitCollision = true;
            }

            if (collision.gameObject.tag == "DeadZone")
            {
                collisionTimer.Start();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "DeadZone")
        {
            isCollide = false;
            collisionTimer.Stop();
            Vector3 exitDirection = -collision.contacts[0].normal;
            float exitDistance = 0.1f; 
            transform.position += exitDirection * exitDistance;
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        runningSpeed = initialRunningSpeed;
        gameObject.transform.localPosition = Vector3.zero;
        enabled = true;
        isJumpStarted = false;
        isCrouched = false;
        isCollide = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            Debug.Log("JE SUIS EN PENTE SLOPE");
            float slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            return slopeAngle > 0 && slopeAngle < 45; // Ajustez l'angle comme nécessaire
        }
        return false;
    }

}