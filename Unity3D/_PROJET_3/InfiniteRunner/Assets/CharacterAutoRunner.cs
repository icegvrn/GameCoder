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
    }

    // Update is called once per frame
    void Update()
    {
        float theSpeed = speed * Time.deltaTime;
        Vector3 movement = transform.position;
        movement.z += runningSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Z))
        {
            if (!isJumpStarted)
            {
                currentState = STATE.JUMP;
                isJumpStarted = true;
                runningSpeed = initialRunningSpeed / 2;
                GetComponent<Rigidbody>().AddForce(transform.up * 25, ForceMode.Impulse);
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            movement.x -= theSpeed;
            currentState = STATE.LEFT;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement.x += theSpeed;
            currentState = STATE.RIGHT;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentState = STATE.CROUCH;
            GetComponent<CapsuleCollider>().enabled = false;
            runningSpeed = initialRunningSpeed / 4;
            isCrouched = true;
        }

        else
        {
            if (isJumpStarted && transform.localPosition.y > 6.5f) { GetComponent<Rigidbody>().AddForce(transform.up * -5, ForceMode.Impulse); }

            else if (isJumpStarted && transform.localPosition.y < 0.5f) { Debug.Log("JE STOPE LE JUMP"); isJumpStarted = false; runningSpeed = initialRunningSpeed; }
           
            else if (isJumpStarted)
            {
                runningSpeed = initialRunningSpeed / 2;
            }

            else
            {
                if (isCrouched)
                {
                    GetComponent<CapsuleCollider>().enabled = true;
                    currentState = STATE.RUN;
                    runningSpeed = initialRunningSpeed;
                    isCrouched = false;
                }
               
            }

        }

        transform.position = movement;

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
       
        if (collision.gameObject.tag == "Obstacle")
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
        if (collision.gameObject.tag == "Obstacle")
        {
          
            runningSpeed = 0;
            currentState = STATE.IDLE;
           
           // Debug.Log("----------Toujours en collision j'ai un timer de " + collisionTimer.GetValue());
             if (collisionTimer.GetFloatValue() >= 0.4f)
            {
                runStatsService.UserLife -= 1;
                collisionTimer.Stop();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            isCollide = false;
            collisionTimer.Stop();
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
}
