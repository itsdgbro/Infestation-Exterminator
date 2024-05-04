using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // private PlayerControls playerControls;
    private PlayerInputHandler playerControls;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
    }

    private void Start()
    {
        playerControls = PlayerInputHandler.Instance;
    }

    private void Update()
    {
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        if (Time.timeScale > 0)
        {
            Look();

        }
    }

    // mouse look
    private void Look()
    {
        mousePosition = playerControls.CharacterLook;

        float mouseX = mousePosition.x * MouseSensitivity * Time.deltaTime;
        float mouseY = mousePosition.y * MouseSensitivity * Time.deltaTime;

        xRoation -= mouseY;
        xRoation = Mathf.Clamp(xRoation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRoation, 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }

}
