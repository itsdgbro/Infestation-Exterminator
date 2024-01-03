using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private PlayerControls playerControls;
    private CharacterMovement characterMovement;

    #region Player_look
    [Header("Mouse Controls")]
    [SerializeField] private float MouseSensitivity = 10f;
    Vector2 mousePosition;
    private float xRoation = 0f;
    private Transform player;
    #endregion

    #region Camera_Bob
    [Header("HeadBob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 1f;
    [SerializeField] private float crouchBobSpeed=  8f;
    [SerializeField] private float crouchBobAmount = 0.25f;
    private float defaultYPos = 0;
    private float timer;
    #endregion


    private void Awake()
    {
        player = transform.parent;

        playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Start()
    {
        characterMovement = GetComponentInParent<CharacterMovement>();
    }

    private void Update()
    {
        CursorLockAndUnlock();
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }
    }


    // mouse cursor hide and unhide
   private void CursorLockAndUnlock()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                if (characterMovement != null)
                {
                    characterMovement.setCanMove(false);
                    characterMovement.setCanJump(false);
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                characterMovement.setCanMove(true);
                characterMovement.setCanJump(true);
            }
        }
    }

    // mouse look
    private void Look()
    {
        mousePosition = playerControls.Movement.Look.ReadValue<Vector2>();

        float mouseX = mousePosition.x * MouseSensitivity * Time.deltaTime;
        float mouseY = mousePosition.y * MouseSensitivity * Time.deltaTime;

        xRoation -= mouseY;
        xRoation = Mathf.Clamp(xRoation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRoation, 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }

    // subtle head(camera) movement
    private void HeadBob()
    {

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
