using UnityEngine;
using UnityEngine.AI;
using static ICharacter;


/// /// <summary>
/// Controle du personnage lorsqu'il n'est pas en run. Il peut se déplacer librement tant qu'il y a un navmesh.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterController : MonoBehaviour, ICharacter
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Animator animator;
   
    private IInputService input;
    public STATE CurrentState { get; set; }

    private NavMeshAgent navMeshAgent;


    void Start()
    {
        InitCharacter();
        InitNavMesh();
    }

    void Update()
    {
        MoveCharacter();
        UpdateAnimator();
    }

    void InitCharacter()
    {
        Cursor.visible = false;
        input = ServiceLocator.Instance.GetService<IInputService>();
        CurrentState = STATE.IDLE;
    }

    void InitNavMesh()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }


    private void MoveCharacter()
    {
        Vector3 moveDirection = Vector3.zero;
        Vector3 movement = transform.position;

        float theSpeed = speed * Time.deltaTime;
        

            if (input.GetKey(InputService.ActionKey.up))
            {
                moveDirection += transform.forward;
                CurrentState = STATE.RUN;
            }
            else if (input.GetKeyUp(InputService.ActionKey.up))
            {
                CurrentState = STATE.IDLE;
            }
            if (input.GetKey(InputService.ActionKey.left))
            {
                moveDirection -= transform.right;
                CurrentState = STATE.LEFT;
            }
            else if (input.GetKeyUp(InputService.ActionKey.left))
            {
                CurrentState = STATE.IDLE;
            }
            if (input.GetKey(InputService.ActionKey.right))
            {
                moveDirection += transform.right;
                CurrentState = STATE.RIGHT;
            }
            else if (input.GetKeyUp(InputService.ActionKey.right))
            {
                CurrentState = STATE.IDLE;
            }
            if (input.GetKey(InputService.ActionKey.down))
            {
                moveDirection -= transform.forward;
                CurrentState = STATE.RUN;
            }
            else if (input.GetKeyUp(InputService.ActionKey.down))
            {
                CurrentState = STATE.IDLE;
            }

            movement += moveDirection.normalized * theSpeed;
        

        // Méthode Unity qui prend le point le plus proche accessible du navmesh
        if (NavMesh.SamplePosition(movement, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
        {
            movement = hit.position;
        }

        transform.position = movement;
    }

    void UpdateAnimator()
    {
        animator.SetBool(STATE.IDLE.ToString(), CurrentState == STATE.IDLE);
        animator.SetBool(STATE.RUN.ToString(), CurrentState == STATE.RUN);
        animator.SetBool(STATE.LEFT.ToString(), CurrentState == STATE.LEFT);
        animator.SetBool(STATE.RIGHT.ToString(), CurrentState == STATE.RIGHT);
        animator.SetBool(STATE.BOOSTED.ToString(), CurrentState == STATE.BOOSTED);
        animator.SetBool(STATE.JUMP.ToString(), CurrentState == STATE.JUMP);
        animator.SetBool(STATE.CROUCH.ToString(), CurrentState == STATE.CROUCH);
        animator.SetBool(STATE.DEAD.ToString(), CurrentState == STATE.DEAD);
    }

}