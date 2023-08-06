using UnityEngine;
using UnityEngine.AI;
using static ICharacter;

public class CharacterController : MonoBehaviour, ICharacter
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float runningSpeed = 10f;
    private float initialRunningSpeed = 10f;
    [SerializeField] private Animator animator;
    public STATE currentState { get; set; }
    public bool isJumpStarted { get; set; }
    public bool isCrouched;
    private bool isCollide;
    private float initialVerticalPosition;
    public Transform cameraTransform; // Référence à la caméra

    private NavMeshAgent navMeshAgent; // Référence au NavMeshAgent
    private bool isBlocked = false; // Pour gérer le blocage

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        isJumpStarted = false;
        initialVerticalPosition = transform.position.y;
        initialRunningSpeed = runningSpeed;
        currentState = STATE.IDLE;

        // Récupérer le NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        UpdateAnimator();
    }

    private void HandleInput()
    {
        float theSpeed = speed * Time.deltaTime;
        Vector3 movement = transform.position;

        if (cameraTransform != null)
        {
            // Obtenir la rotation de la souris et l'appliquer au personnage
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up * mouseX);
        }

        // Contrôler le déplacement au clavier
        if (!isBlocked)
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.Z))
            {
                moveDirection += transform.forward;
                currentState = STATE.RUN;
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                currentState = STATE.IDLE;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                moveDirection -= transform.right;
                currentState = STATE.LEFT;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                currentState = STATE.IDLE;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += transform.right;
                currentState = STATE.RIGHT;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                currentState = STATE.IDLE;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection -= transform.forward;
                currentState = STATE.RUN;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                currentState = STATE.IDLE;
            }

            movement += moveDirection.normalized * theSpeed;
        }

        // Vérifier les limites du navmesh
        if (NavMesh.SamplePosition(movement, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
        {
            movement = hit.position;
        }

        transform.position = movement;
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
        isCollide = false;
    }
}