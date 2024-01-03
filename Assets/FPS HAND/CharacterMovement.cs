using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{   
    private PlayerControls playerControls;

    [SerializeField]
    private float moveSpeed = 5f;

    public float getSpeed()
    {
        return moveSpeed;
    }

    private float sprintSpeed = 10f;

    private Vector3 velocity;

    private float gravity = -9.81f;

    private Vector2 characterMove;

    [SerializeField]
    private float jumpHeight = 2f;

    private CharacterController characterController;

    private bool isGrounded = false;

    public Transform groundCheck;
    public LayerMask groundLayer;

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
        }

        characterController.Move(movement * maxSpeed * Time.deltaTime);

        Debug.Log(maxSpeed);
    }


    private void Jump()
    {
        if(playerControls.Movement.Jump.triggered && isGrounded)
        {
            Debug.Log("Jump");
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
