using System;
using UnityEngine;
using UnityEngine.Events;

public class HeadBob : MonoBehaviour
{
    // PlayerControls playerControls;
    private PlayerInputHandler playerControls;

    private CharacterMovement characterMovement;


    [Range(10f, 100f)]
    public float Smooth = 10.0f;

    [Header("Walk Settings")]
    [SerializeField, Range(1f, 20f)]
    private float walkBobSpeed = 8f;

    [SerializeField, Range(0.001f, 0.01f)]
    private float walkBobAmount = 0.0065f;

    [Header("Sprint Settings")]
    [SerializeField, Range(1f, 20f)]
    private float sprintBobSpeed = 12f;

    [SerializeField, Range(0.001f, 0.01f)]
    private float sprintBobAmount = 0.009f;

    [Header("Crouch Settings")]
    [SerializeField, Range(1f, 20f)]
    private float crouchBobSpeed = 3f;

    [SerializeField, Range(0.001f, 0.01f)]
    private float crouchBobAmount = 0.0065f;



    Vector3 StartPos;
    float Sin;
    bool isTriggered;
    public UnityEvent onFootStep;

    private void Awake()
    {
        if (!TryGetComponent<CharacterMovement>(out characterMovement))
        {
            characterMovement = transform.parent.parent.GetComponent<CharacterMovement>();
        }
    }

    private void Start()
    {
        StartPos = transform.localPosition;
        playerControls = PlayerInputHandler.Instance;
    }

    private void Update()
    {
        CheckHeadBobTrigger();
        if (characterMovement.IsAiming())
        {
            transform.localPosition = StartPos;
        }
        else
        {
            StopHeadbob();
        }

    }


    private void CheckHeadBobTrigger()
    {
        float inputValue = playerControls.CharacterMove.magnitude;
        if (inputValue > 0 && characterMovement.GetIsGrounded() && !characterMovement.IsAiming())
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        float bobSpeed = walkBobSpeed;
        float bobAmount = walkBobAmount;

        if (characterMovement.GetIsCrouching())
        {
            bobSpeed = crouchBobSpeed;
            bobAmount = crouchBobAmount;
        }
        else if (characterMovement.GetIsSprinting())
        {
            bobSpeed = sprintBobSpeed;
            bobAmount = sprintBobAmount;
        }

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * bobSpeed) * bobAmount, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * bobSpeed / 2f) * bobAmount, Smooth * Time.deltaTime);
        transform.localPosition += pos;

        Sin = Mathf.Sin(Time.time * bobSpeed);
        if (Sin > 0.97f && isTriggered == false)
        {
            isTriggered = true;
            onFootStep.Invoke();
        }
        else if (isTriggered = true && Sin < -0.97f)
        {
            isTriggered = false;
        }

        return pos;
    }
    private void StopHeadbob()
    {
        if (transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }
}