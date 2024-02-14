using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] private Camera playerCamera;

    #region Player_look
    [Header("Mouse Controls")]
    [SerializeField] private float MouseSensitivity = 10f;
    Vector2 mousePosition;
    private float xRoation = 0f;
    private Transform player;
    #endregion
    
    private void Awake()
    {
        player = transform.parent;
        playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            Look();
            
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

    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
