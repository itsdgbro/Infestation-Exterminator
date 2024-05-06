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


    public InputAction MoveAction { get; set; }
    public InputAction LookAction { get; set; }
    public InputAction SprintAction { get; set; }
    public InputAction JumpAction { get; set; }
    public InputAction CrouchAction { get; set; }
    public InputAction FireAction { get; set; }
    public InputAction FireAutoAction { get; set; }
    public InputAction ReloadAction { get; set; }
    public InputAction AimAction { get; set; }
    public InputAction HealAction { get; set; }
    public InputAction ToggleTipsAction { get; set; }
    public InputAction InteractAction { get; set; }
    public InputAction FlashLightAction { get; set; }

    // [SerializeField] private InputActionReference toggleTipsAction;

    // "Getter and Setter"
    public Vector2 CharacterMove { get; private set; }
    public Vector2 CharacterLook { get; private set; }
    public float SprintValue { get; private set; }
    public bool JumpTriggered { get; private set; }
    public float CrouchTriggered { get; private set; }
    public bool FireTriggered { get; private set; }
    public float FireAutoTriggered { get; private set; }
    public bool Reload { get; private set; }
    public float AimTriggered { get; private set; }
    public bool HealTriggered { get; private set; }
    public bool ToggleTipsValue { get; private set; }
    public bool Interact { get; private set; }
    public bool FlashLight { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            playerControls.LoadBindingOverridesFromJson(rebinds);

        MoveAction = playerControls.FindActionMap(actionMovement).FindAction(move);
        LookAction = playerControls.FindActionMap(actionMovement).FindAction(look);
        SprintAction = playerControls.FindActionMap(actionMovement).FindAction(sprint);
        JumpAction = playerControls.FindActionMap(actionMovement).FindAction(jump);
        CrouchAction = playerControls.FindActionMap(actionMovement).FindAction(crouch);
        FireAction = playerControls.FindActionMap(actionMovement).FindAction(fire);
        FireAutoAction = playerControls.FindActionMap(actionMovement).FindAction(fire);
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

        SprintAction.performed += context => SprintValue = context.ReadValue<float>();
        SprintAction.canceled += context => SprintValue = 0f;

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


