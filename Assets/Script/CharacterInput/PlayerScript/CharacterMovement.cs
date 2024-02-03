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
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
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

    void Awake()
    {
        playerControls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        Gravity();
        Move();
        Jump();

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

        // Check if sprint button is being held down and the player is moving forward
        if (playerControls.Movement.Sprint.ReadValue<float>() > 0.1f && characterMove.y > 0.1f)
        {
            maxSpeed = sprintSpeed; // Set the speed to sprintSpeed
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

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
