using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private PlayerControls playerControls;
    private CharacterMovement characterMovement;
    [SerializeField] private Camera playerCamera;

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
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;
    #endregion


    private void Awake()
    {
        player = transform.parent;
        playerControls = new PlayerControls();
        characterMovement = GetComponentInParent<CharacterMovement>();
        defaultYPos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            Look();
            HeadBob();
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
        if (!characterMovement.getIsGrounded()) return;
        if (Mathf.Abs(characterMovement.getVelocity().x) > 0.1f || Mathf.Abs(characterMovement.getVelocity().z) > 0.1f)
        {
            timer += Time.deltaTime * (characterMovement.getIsCrouching() ? crouchBobSpeed : characterMovement.getIsSprinting() ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (characterMovement.getIsCrouching() ? crouchBobAmount : characterMovement.getIsSprinting() ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
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
