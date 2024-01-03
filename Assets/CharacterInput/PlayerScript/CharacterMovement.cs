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

    #region Player_Can
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canUseHeadBob = true;
    #endregion

    #region Get_Set
    public bool getCanMove() => canMove;    
    public void setCanMove(bool moveAble) => canMove = moveAble;
    public bool getCanJump() => canJump;
    public void setCanJump(bool jumpAble) => canJump = jumpAble;
    public bool getCanUseHeadBob() => canUseHeadBob;
    public void setCanuseHeadBob(bool headBobUseAble) => canUseHeadBob = headBobUseAble; 
    public bool getIsGrounded() => isGrounded;
    public void setIsGrounded(bool isGrounded) => this.isGrounded = isGrounded;
    public Vector3 getVelocity() => velocity;
    public bool getIsSprinting() => isSprinting;
    public bool setIsSprinting(bool isSprinting) => this.isSprinting = isSprinting;
    public bool getIsCrouching() => isCrouching;
    public bool setIsCrouching(bool isCrouching) => this.isCrouching = isCrouching;
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
        if (canMove)
        {
            Move();
        }
        if (canJump)
        {
            Jump();
        }
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
        if(playerControls.Movement.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }


    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
