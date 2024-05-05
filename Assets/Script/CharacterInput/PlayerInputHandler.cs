using System;
using System.Collections.Generic;
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


    public InputAction MoveAction { get; private set; }
    public InputAction LookAction { get; private set; }
    public InputAction SprintAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction CrouchAction { get; private set; }
    public InputAction FireAction { get; private set; }
    public InputAction ReloadAction { get; private set; }
    public InputAction AimAction { get; private set; }
    public InputAction HealAction { get; private set; }
    public InputAction ToggleTipsAction { get; private set; }
    public InputAction InteractAction { get; private set; }
    public InputAction FlashLightAction { get; private set; }

    // [SerializeField] private InputActionReference toggleTipsAction;

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
    public bool ToggleTipsValue { get; private set; }
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

        MoveAction = playerControls.FindActionMap(actionMovement).FindAction(move);
        LookAction = playerControls.FindActionMap(actionMovement).FindAction(look);
        SprintAction = playerControls.FindActionMap(actionMovement).FindAction(sprint);
        JumpAction = playerControls.FindActionMap(actionMovement).FindAction(jump);
        CrouchAction = playerControls.FindActionMap(actionMovement).FindAction(crouch);
        FireAction = playerControls.FindActionMap(actionMovement).FindAction(fire);
        ReloadAction = playerControls.FindActionMap(actionMovement).FindAction(reload);
        AimAction = playerControls.FindActionMap(actionMovement).FindAction(aim);
        HealAction = playerControls.FindActionMap(actionMovement).FindAction(heal);
        ToggleTipsAction = playerControls.FindActionMap(actionInteractive).FindAction(toggleTips);
        InteractAction = playerControls.FindActionMap(actionInteractive).FindAction(interact);
        FlashLightAction = playerControls.FindActionMap(actionInteractive).FindAction(flashLight);

        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        MoveAction.performed += context => CharacterMove = context.ReadValue<Vector2>();
        MoveAction.canceled += context => CharacterMove = Vector2.zero;

        LookAction.performed += context => CharacterLook = context.ReadValue<Vector2>();
        LookAction.canceled += context => CharacterLook = Vector2.zero;

        SprintAction.performed += context => SprintValue = context.ReadValue<float>();
        SprintAction.canceled += context => SprintValue = 0f;

        JumpAction.performed += context => JumpTriggered = true;
        JumpAction.canceled += context => JumpTriggered = false;

        CrouchAction.performed += context => CrouchTriggered = context.ReadValue<float>();
        CrouchAction.canceled += context => CrouchTriggered = 0f;

        FireAction.performed += context =>
        {
            FireTriggered = true;
            FireAutoTriggered = context.ReadValue<float>();
        };
        FireAction.canceled += context =>
        {
            FireTriggered = false;
            FireAutoTriggered = 0f;
        };

        ReloadAction.performed += context => Reload = true;
        ReloadAction.canceled += context => Reload = false;

        AimAction.performed += context => AimTriggered = context.ReadValue<float>();
        AimAction.canceled += context => AimTriggered = 0f;

        HealAction.performed += context => HealTriggered = true;
        HealAction.canceled += context => HealTriggered = false;

        // toggleTipsAction.performed += context => ToggleTips = true;
        // toggleTipsAction.canceled += context => ToggleTips = false;

        InteractAction.performed += context => Interact = true;
        InteractAction.canceled += context => Interact = false;

        FlashLightAction.performed += context => FlashLight = true;
        FlashLightAction.canceled += context => FlashLight = false;

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


