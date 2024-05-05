using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    // private PlayerControls playerControls;
    private PlayerInputHandler playerControls;

    private bool isGrounded = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    public bool IsAiming { get; set; }

    #region Player_Attributes
    [Header("Player Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3.2f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1.5f;
    #endregion

    #region get_set
    public bool GetIsGrounded() => isGrounded;
    public void SetIsGrounded(bool isGrounded) => this.isGrounded = isGrounded;
    public Vector3 GetVelocity() => velocity;
    public bool GetIsSprinting() => isSprinting;
    public void SetIsSprinting(bool isSprinting) => this.isSprinting = isSprinting;
    public bool GetIsCrouching() => isCrouching;
    public void SetIsCrouching(bool isCrouching) => this.isCrouching = isCrouching;
    #endregion


    #region Player_Physics
    private Vector3 velocity;
    private Vector2 characterMove;
    private CharacterController characterController;
    public Transform groundCheck;
    public LayerMask groundLayer;
    #endregion

    [Header("Sounds")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;

    #region Animator
    private Animator animator;
    #endregion


    #region Stamina
    private PlayerStat playerStat;
    #endregion


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerStat = GetComponent<PlayerStat>();
        audioSource = GetComponent<AudioSource>();

        // playerControls = PlayerInputHandler.Instance;

    }

    private void Start()
    {
        playerControls = PlayerInputHandler.Instance;

        // Move action (Does not work on subscription)
        // PlayerInputHandler.Instance.MoveAction.started += Move;
        // PlayerInputHandler.Instance.MoveAction.canceled += Move;

        // Sprint action
        // PlayerInputHandler.Instance.SprintAction.performed += Sprint;
        // PlayerInputHandler.Instance.SprintAction.canceled += Sprint;

        // aim action
        PlayerInputHandler.Instance.AimAction.performed += Aiming;
        PlayerInputHandler.Instance.AimAction.canceled += Aiming;

        // Jump action
        PlayerInputHandler.Instance.JumpAction.started += Jump;

        // crouch action
        PlayerInputHandler.Instance.CrouchAction.started += Crouch;
        PlayerInputHandler.Instance.CrouchAction.canceled += Crouch;
        Gravity();

    }

    void Update()
    {
        Gravity();

        Move();
        // dead
        if (playerStat.IsDead())
            animator.SetTrigger("dead");

    }

    private void Gravity()
    {
        bool wasGrounded = isGrounded;

        // ground check with feet touching Ground Layer
        isGrounded = Physics.CheckSphere(groundCheck.position, characterController.radius * 0.9f, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            if (!wasGrounded)
            {
                // Play landing sound
                audioSource.PlayOneShot(jumpSound);
            }
            // constant pulling value
            velocity.y = -5f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }


    private void Move()
    {
        characterMove = playerControls.MoveAction.ReadValue<Vector2>();
        Vector3 movement = (characterMove.y * transform.forward) + (characterMove.x * transform.right);

        float maxSpeed = moveSpeed; // Default to regular move speed

        // bool check if spring key is pressed
        isSprinting = playerControls.SprintAction.ReadValue<float>() > 0.1f;
        // Check if sprint button is being held down and the player is moving forward
        if (isSprinting && characterMove.y > 0.1f && isCrouching == false && !IsAiming)
        {
            playerStat.DecreaseStamina(); // decrease stamina on sprint
            if (playerStat.CanSprint())
                maxSpeed = sprintSpeed; // Set the speed to sprintSpeed
            else
            {
                maxSpeed = moveSpeed;
                isSprinting = false;
            }
        }
        // if crouching decrease speed by half
        else if (characterMove.y > 0.1f && isCrouching)
            maxSpeed = moveSpeed / 2;
        // if aiming (ads)
        else if (IsAiming)
            maxSpeed = moveSpeed / 1.5f;

        // Update velocity for horizontal movement
        Vector3 horizontalVelocity = movement * maxSpeed;
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;
        characterController.Move(movement * maxSpeed * Time.deltaTime);

    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerStat.DecreaseStamina(10f);
        }
    }

    private void Crouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetBool("isCrouching", true);
        }
        else if (context.canceled)
        {
            animator.SetBool("isCrouching", false);
        }
    }

    public void Aiming(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAiming = true;
        }
        else if (context.canceled)
        {
            IsAiming = false;
        }
    }

    void OnDisable()
    {
        // // Move action
        // PlayerInputHandler.Instance.MoveAction.started += Move;
        // PlayerInputHandler.Instance.MoveAction.canceled += Move;

        // // Sprint action
        // PlayerInputHandler.Instance.SprintAction.performed += Move;
        // PlayerInputHandler.Instance.SprintAction.canceled += Move;

        // aim action
        PlayerInputHandler.Instance.AimAction.performed -= Aiming;
        PlayerInputHandler.Instance.AimAction.canceled += Aiming;

        // crouch
        PlayerInputHandler.Instance.JumpAction.started -= Jump;

        // crouch
        PlayerInputHandler.Instance.CrouchAction.started -= Crouch;
        PlayerInputHandler.Instance.CrouchAction.canceled -= Crouch;

    }
}
