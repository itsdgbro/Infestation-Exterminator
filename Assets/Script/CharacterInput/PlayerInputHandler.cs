using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMovement = "Movement";
    [SerializeField] private string actionInteractive = "Interactive";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string crouch = "Crouch";
    [SerializeField] private string fire = "Fire";
    [SerializeField] private string reload = "Reload";
    [SerializeField] private string aim = "Aim";
    [SerializeField] private string heal = "Heal";
    [SerializeField] private string toggleTips = "ToggleTips";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string flashLight = "FlashLight";


    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction fireAction;
    private InputAction reloadAction;
    private InputAction aimAction;
    private InputAction healAction;
    private InputAction toggleTipsAction;
    private InputAction interactAction;
    private InputAction flashLightAction;

    // "Getter and Setter"
    public Vector2 CharacterMove { get; private set; }
    public Vector2 CharacterLook { get; private set; }
    public float SprintValue { get; private set; }
    public bool JumpTriggered { get; private set; }
    public float CrouchTriggered { get; private set; }
    public float FireAutoTriggered { get; private set; }
    public bool FireTriggered { get; private set; }
    public bool Reload { get; private set; }
    public float AimTriggered { get; private set; }
    public bool HealTriggered { get; private set; }
    public bool ToggleTips { get; private set; }
    public bool Interact { get; private set; }
    public bool FlashLight { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            playerControls.LoadBindingOverridesFromJson(rebinds);


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMovement).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMovement).FindAction(look);
        sprintAction = playerControls.FindActionMap(actionMovement).FindAction(sprint);
        jumpAction = playerControls.FindActionMap(actionMovement).FindAction(jump);
        crouchAction = playerControls.FindActionMap(actionMovement).FindAction(crouch);
        fireAction = playerControls.FindActionMap(actionMovement).FindAction(fire);
        reloadAction = playerControls.FindActionMap(actionMovement).FindAction(reload);
        aimAction = playerControls.FindActionMap(actionMovement).FindAction(aim);
        healAction = playerControls.FindActionMap(actionMovement).FindAction(heal);
        toggleTipsAction = playerControls.FindActionMap(actionInteractive).FindAction(toggleTips);
        interactAction = playerControls.FindActionMap(actionInteractive).FindAction(interact);
        flashLightAction = playerControls.FindActionMap(actionInteractive).FindAction(flashLight);

        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        moveAction.performed += context => CharacterMove = context.ReadValue<Vector2>();
        moveAction.canceled += context => CharacterMove = Vector2.zero;

        lookAction.performed += context => CharacterLook = context.ReadValue<Vector2>();
        lookAction.canceled += context => CharacterLook = Vector2.zero;

        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => SprintValue = 0f;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;

        crouchAction.performed += context => CrouchTriggered = context.ReadValue<float>();
        crouchAction.canceled += context => CrouchTriggered = 0f;

        fireAction.performed += context =>
        {
            FireTriggered = true;
            FireAutoTriggered = context.ReadValue<float>();
        };
        fireAction.canceled += context =>
        {
            FireTriggered = false;
            FireAutoTriggered = 0f;
        };

        reloadAction.performed += context => Reload = true;
        reloadAction.canceled += context => Reload = false;

        aimAction.performed += context => AimTriggered = context.ReadValue<float>();
        aimAction.canceled += context => AimTriggered = 0f;

        healAction.performed += context => HealTriggered = true;
        healAction.canceled += context => HealTriggered = false;

        toggleTipsAction.performed += context => ToggleTips = true;
        toggleTipsAction.canceled += context => ToggleTips = false;

        interactAction.performed += context => Interact = true;
        interactAction.canceled += context => Interact = false;

        flashLightAction.performed += context => FlashLight = true;
        flashLightAction.canceled += context => FlashLight = false;

    }

    private void OnEnable()
    {
        playerControls.Enable();

    }


    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void Disable()
    {
        OnDisable();
    }

    public void Enable()
    {
        OnEnable();
    }
}


