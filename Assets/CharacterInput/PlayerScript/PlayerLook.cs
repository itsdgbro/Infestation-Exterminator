using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private PlayerControls playerControls;

    [SerializeField]
    private float MouseSensitivity = 50f;

    Vector2 mousePosition;
    private float xRoation = 0f;
    private Transform player;

    private void Awake()
    {
        player = transform.parent;

        playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Look();
    }

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
