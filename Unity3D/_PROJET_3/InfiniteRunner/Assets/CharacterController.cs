using UnityEngine;
using UnityEngine.Playables;
using static ICharacter;

public class CharacterController : MonoBehaviour, ICharacter
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float runningSpeed = 10f;
    private float initialRunningSpeed = 10f;
    [SerializeField]
    private Animator animator;
    public STATE currentState { get; set; }
    public bool isJumpStarted {get; set; }
    public bool isCrouched;
    private float initialVerticalPosition;

// Start is called before the first frame update
void Start()
    {
        isJumpStarted = false;
        initialVerticalPosition = transform.position.y;
        initialRunningSpeed = runningSpeed;
        currentState = STATE.RUN;
    }

    // Update is called once per frame
    void Update()
    {
        float theSpeed = speed*Time.deltaTime;
        Vector3 movement = transform.position;
        movement.z += runningSpeed * Time.deltaTime;
        

        if (Input.GetKey(KeyCode.Z))
        {
           
           if (!isJumpStarted)
            {   currentState = STATE.JUMP;
                isJumpStarted = true;
                runningSpeed = initialRunningSpeed / 2;
                GetComponent<Rigidbody>().AddForce(transform.up * 25, ForceMode.Impulse);
            }
        }
        else
        {
            if (isJumpStarted && transform.localPosition.y > 6.5f) { GetComponent<Rigidbody>().AddForce(transform.up * -5, ForceMode.Impulse); } 

            else if (isJumpStarted && transform.localPosition.y < 0.5f ) { Debug.Log("JE STOPE LE JUMP"); isJumpStarted = false; runningSpeed = initialRunningSpeed; }
            else if (isJumpStarted)
            {
                runningSpeed = initialRunningSpeed / 2;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                movement.x -= theSpeed;
                currentState = STATE.LEFT;
            }


            if (Input.GetKey(KeyCode.D))
            {
                movement.x += theSpeed;
                currentState = STATE.RIGHT;
            }

            if (Input.GetKey(KeyCode.S))
            {
                currentState = STATE.CROUCH;
                GetComponent<CapsuleCollider>().enabled = false;
                runningSpeed = initialRunningSpeed/4;
                isCrouched = true;
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
            runningSpeed = 0;
            currentState = STATE.IDLE;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            runningSpeed = 0;
            currentState = STATE.IDLE;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        runningSpeed = initialRunningSpeed;
        currentState = STATE.RUN;
    }
}