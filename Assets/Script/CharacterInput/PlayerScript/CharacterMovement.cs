using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    private PlayerControls playerControls;
    private bool isGrounded = false;
    private bool isSprinting = false;
    private bool isCrouching = false;

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

    #region Animator
    private Animator animator;
    #endregion

    #region FootSpets
    private Footsteps footsteps;
    #endregion

    void Awake()
    {
        playerControls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        footsteps = GetComponentInChildren<Footsteps>();
    }

    private void Start()
    {
        Gravity();
    }

    void Update()
    {
        Gravity();
        Move();
        if (!isCrouching)
        {
            Jump();
        }
        Crouch();
    }

    private void Gravity()
    {
        // ground check with feet touching Ground Layer
        isGrounded = Physics.CheckSphere(groundCheck.position, characterController.radius * 0.9f, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            // constant pulling value
            velocity.y = -5f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }



    private void Move()
    {
        characterMove = playerControls.Movement.Move.ReadValue<Vector2>();
        Vector3 movement = (characterMove.y * transform.forward) + (characterMove.x * transform.right);

        float maxSpeed = moveSpeed; // Default to regular move speed

        // bool check if spring key is pressed
        isSprinting = playerControls.Movement.Sprint.ReadValue<float>() > 0.1f;
        // Check if sprint button is being held down and the player is moving forward
        if (isSprinting && characterMove.y > 0.1f && isCrouching == false && !IsAiming())
            maxSpeed = sprintSpeed; // Set the speed to sprintSpeed
        // if crouching decrease speed by half
        else if (characterMove.y > 0.1f && isCrouching)
            maxSpeed = moveSpeed / 2;
        // if aiming (ads)
        else if (IsAiming())
            maxSpeed = moveSpeed / 1.5f;

        // Update velocity for horizontal movement
        Vector3 horizontalVelocity = movement * maxSpeed;
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;

        characterController.Move(movement * maxSpeed * Time.deltaTime);

    }


    private void Jump()
    {
        if (playerControls.Movement.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Crouch()
    {
        isCrouching = playerControls.Movement.Crouch.ReadValue<float>() > 0.1f;

        // decrease the height of the character via animation
        animator.SetBool("isCrouching", isCrouching);
    }

    public bool IsAiming()
    {
        return playerControls.Movement.Aim.ReadValue<float>() > 0.5f;
    }


    #region Enable/Disable
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion
}
