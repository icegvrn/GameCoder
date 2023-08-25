using UnityEngine;
using UnityEngineInternal;

public class CharacterAutoRunner : MonoBehaviour
{

    [Header("Configuration du playable character")]
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float lateralSpeed = 10f;
    [SerializeField] private Animator animator;
    private CapsuleCollider head;
    private float initialRunningSpeed;

    // State du personnage
    private enum CHARACTER_STATE { IDLE, RUN, LEFT, RIGHT, JUMP, CROUCH, DEAD }
    private CHARACTER_STATE currentState;
    private bool isJumpStarted;
    private bool isCrouched;
    private bool isLateral;
    private bool isCollide;

    // Collisions
    private CustomTimer collisionTimer;
    private float playerHeight;
    private const float CrouchOffset = 0.2f;

    //Physics
    private Rigidbody rb;
    private const float SlopeGravityFactor = 1f / 125f;

    // Services liés
    private IInputService input;
    private IRunningGameService runStatsService;


    /// <summary>
    /// Initialisation du character : les services qu'il utilise, son état et stockage de certains composants pour les collisions.
    /// </summary>
    private void Start()
    {
        InitializeServices();
        InitializeCharacterState();
        InitializePhysicAndCollisionComponents();
    }

    /// <summary>
    /// Initilisation du service d'input pour prendre les input joueur et du RunningGameService pour pouvoir modifier la vie de joueur à l'impact.
    /// </summary>
    void InitializeServices()
    {
        input = ServiceLocator.Instance.GetService<IInputService>();
        runStatsService = ServiceLocator.Instance.GetService<IRunningGameService>();
    }

    /// <summary>
    /// Initialisation de l'état de base du character : par défaut, il court à la vitesse qui a été paramétrée.
    /// </summary>
    void InitializeCharacterState()
    {
        isJumpStarted = false;
        currentState = CHARACTER_STATE.RUN;
        initialRunningSpeed = runningSpeed;
    }

    /// <summary>
    /// Initalisation des éléments qui serviront pour la physique et les collisions : le rigidbody, le collider et le timer de collision (utilisé pour enlever de la vie après un certain temps au contact d'un obstacle).
    /// </summary>
    void InitializePhysicAndCollisionComponents()
    {
        rb = GetComponent<Rigidbody>();
        head = GetComponent<CapsuleCollider>();
        playerHeight = GetComponent<BoxCollider>().bounds.size.y;
        collisionTimer = new CustomTimer();
        collisionTimer.Init();
    }

    private void Update()
    {
        UpdateMovements();
        UpdateStates();
        UpdateCollisions();
        UpdateRun();
        UpdateAnimator();
    }

    /// <summary>
    /// Méthode appelant les méthodes permettant de mettre à jour les mouvements du personnage : avant, côté, en hauteur.
    /// </summary>
    void UpdateMovements()
    {
        HandleHorizontalMovement();
        HandleVerticalMovement();
        HandleForwardMovement();
    }

    /// <summary>
    /// Méthode appelant celles permettant de vérifier si le personnage est en train de sauter ou de s'accroupir.
    /// </summary>
    void UpdateStates()
    {
        HandleJumping();
        HandleCrouching();
    }

    /// <summary>
    /// Méthode appelant celles mettant à jour les éléments de collision : le timer de collision s'il est lancé et le clamp vis à vis du sol.
    /// </summary>
    void UpdateCollisions()
    {
        collisionTimer.Update();
        ClampPositionFromFloor();
    }

    /// <summary>
    /// Méthode permettant de déterminer la course automatique du personnage s'il n'est dans aucune des actions sauter, s'accroupir, aller sur le côté ou être en collision.
    /// </summary>
    void UpdateRun()
    {
        if (!isJumpStarted && !isCrouched && !isCollide & !isLateral)
        {
            currentState = CHARACTER_STATE.RUN;
            runningSpeed = initialRunningSpeed;
        }

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
    }

    /// <summary>
    /// Méthode permettant de mettre à jour l'animator du character en fonction de son état actuel, via un booléen portant le même nom que le CHARACTER_STATE.
    /// </summary>
    private void UpdateAnimator()
    {
        animator.SetBool(CHARACTER_STATE.IDLE.ToString(), currentState == CHARACTER_STATE.IDLE);
        animator.SetBool(CHARACTER_STATE.RUN.ToString(), currentState == CHARACTER_STATE.RUN);
        animator.SetBool(CHARACTER_STATE.LEFT.ToString(), currentState == CHARACTER_STATE.LEFT);
        animator.SetBool(CHARACTER_STATE.RIGHT.ToString(), currentState == CHARACTER_STATE.RIGHT);
        animator.SetBool(CHARACTER_STATE.JUMP.ToString(), currentState == CHARACTER_STATE.JUMP);
        animator.SetBool(CHARACTER_STATE.CROUCH.ToString(), currentState == CHARACTER_STATE.CROUCH);
        animator.SetBool(CHARACTER_STATE.DEAD.ToString(), currentState == CHARACTER_STATE.DEAD);
    }


    /// <summary>
    /// Permet de contrôler l'input utilisateur gauche-droite et d'appliquer une force correspondante au rigidbody du personnage.
    /// </summary>
    private void HandleHorizontalMovement()
    {
        float horizontalInput = input.GetHorizontalAxis();

        isLateral = false;

        if (!isCrouched && !isJumpStarted)
        {
            if (horizontalInput < 0)
            {
                isLateral = true;
                currentState = CHARACTER_STATE.LEFT;
            }
            else if (horizontalInput > 0)
            {
                isLateral = true;
                currentState = CHARACTER_STATE.RIGHT;
            }
        }

        Vector3 horizontalMovement = new Vector3(horizontalInput * lateralSpeed * Time.deltaTime, 0, 0);
        rb.AddForce(horizontalMovement, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Methode permettant de faire avancer automatiquement le personnage s'il n'est pas en collision. 
    /// </summary>
    private void HandleForwardMovement()
    {
        if (!isCollide)
        {
            Vector3 forwardMovement = new Vector3(0, 0, runningSpeed * Time.deltaTime);
            rb.AddForce(forwardMovement, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Méthode gérant le saut du personnage : vérifie s'il saute selon input de l'utilisateur et annule le saut s'il retouche le sol.
    /// </summary>
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
            currentState = CHARACTER_STATE.JUMP;
        }

        if (transform.localPosition.y <= 0.001f)
        {
            if (isJumpStarted)
            {
                isJumpStarted = false;
                currentState = CHARACTER_STATE.RUN;
            }
        }
    }

    /// <summary>
    /// Méthode gérant l'accroupissement du personnage : vérifie s'il est accroupi selon input de l'utilisateur
    /// et déplace le capsuleCollider (avec une valeur de crouchOffset) représentant la tête du personnage pour lui permettre de passer sous des objets plus bas.
    /// </summary>
    private void HandleCrouching()
    {
        if (input.GetKey(InputService.ActionKey.down))
        {
            if (!isCrouched)
            {
                head.center = new Vector3(
                    head.center.x,
                     head.center.y - CrouchOffset,
                     head.center.z
                 );
                currentState = CHARACTER_STATE.CROUCH;
                isCrouched = true;
                isCollide = false;
            }
        }
        else if (input.GetKeyUp(InputService.ActionKey.down))
        {
            if (isCrouched)
            {
                head.center = new Vector3(
                   head.center.x,
                   head.center.y + CrouchOffset,
                   head.center.z
                );

                isCrouched = false;
            }
        }
    }

    /// <summary>
    /// Gère le mouvement vertical : applique une force opposée vers le bas quand le personnage monte sur un slope pour ne pas qu'il reste trop longtemps dans l'air.
    /// Applique une force opposée vers le bas quand le personnage dépasse une certaine hauteur pour une gravité plus rapide et un effet de saut plus intéressant.
    /// </summary>
    private void HandleVerticalMovement()
    {
        if (OnSlope())
        {
            Vector3 slopeGravity = Vector3.down * (Physics.gravity.magnitude * SlopeGravityFactor);
            rb.AddForce(slopeGravity, ForceMode.Acceleration);
        }
        if (transform.localPosition.y > 6f)
        {
            rb.AddForce(transform.up * -200, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Permet de maintenir le personnage au-dessus du sol si jamais il est amené à aller plus bas que celui-ci par collision notamment.
    /// </summary>
    private void ClampPositionFromFloor()
    {
        if (transform.position.y < 0f)
        {
            Vector3 clamp = transform.position;
            clamp.y = 0f;
            transform.position = clamp;
        }
    }


    /// <summary>
    /// Méthode permettant de dire à l'aide d'un Raycast si le personnage se trouve sur une pente.
    /// </summary>
    /// <returns></returns>
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, playerHeight / 2 + 0.5f))
        {
            float slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            return slopeAngle > 10 && slopeAngle < 45;
        }
        return false;
    }

    /// <summary>
    /// Check la collision avec un Obstacle ou une "DeadZone". Si c'est le cas la vitesse passe à 0 et le personnage est remis à IDLE. 
    /// Un timer collision est lancé : si le personnage reste en contact trop longtemps il perdra de la vie.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeadZone"))
        {
            isCollide = true;
            rb.velocity = Vector3.zero;
            runningSpeed = 0;
            currentState = CHARACTER_STATE.IDLE;

            if (!collisionTimer.TimerIsStarted)
            {
                collisionTimer.Start();
            }
        }
    }

    /// <summary>
    /// Si le personnage est resté trop longtemps en contact avec un obstacle ou une deadzone, il perd une vie et le timer de collision se stop.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeadZone"))
        {
            isCollide = true;
            rb.velocity = Vector3.zero;

            if (collisionTimer.GetFloatValue() >= 0.4f)
            {
                runStatsService.UserLife -= 1;
                collisionTimer.Stop();
                currentState = CHARACTER_STATE.IDLE;
            }
      

        }
    }


    /// <summary>
    /// QUand le personnage quitte une collision avec un obstacle ou une deadzone, on stop le timer pour ne pas qu'il perde de la vie.
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("DeadZone"))
        {
            isCollide = false;
            collisionTimer.Stop();
            runningSpeed = initialRunningSpeed;

            if (transform.position.y > 0)
            {
                Vector3 newPosition = transform.position;
                newPosition.y = 0;
                transform.position = newPosition;
            }
        }

    }

    /// <summary>
    /// Méthode publique permettant de rétablir depuis l'extérieur l'état initial du personnage.
    /// </summary>
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