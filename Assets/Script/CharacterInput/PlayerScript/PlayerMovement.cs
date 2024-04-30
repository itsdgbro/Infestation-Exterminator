using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerControls;

    [SerializeField]
    private float moveSpeed = 5f;

    private float sprintSpeed = 10f;

    private Vector3 velocity;

    private float gravity = -9.81f;

    private Vector2 characterMove;

    private Vector2 gravity2;

    private float jumpHeight = 2f;

    private CharacterController characterController;

    public Transform ground;

    public float distanceToGround = 0.4f;

    public LayerMask groundLayerMask;

    private bool isGrounded;


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
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
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
        }

        characterController.Move(movement * maxSpeed * Time.deltaTime);

    }


    private void Jump()
    {
        if (playerControls.Movement.Jump.triggered && isGrounded)
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
