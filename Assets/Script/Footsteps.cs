using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Footsteps : MonoBehaviour
{
    PlayerControls playerControls;
    [Range(0, 20f)]
    public float frequency = 10.0f;
    public UnityEvent onFootStep;
    float Sin;
    bool isTriggered = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Update()
    {
        float inputValue = playerControls.Movement.Move.ReadValue<Vector2>().magnitude;
        if (inputValue > 0)
        {   
            StartFootsteps();
        }
    }

    private void StartFootsteps()
    {
        Sin = Mathf.Sin(Time.time * frequency);
        if (Sin > 0.97f && isTriggered == false)
        {
            isTriggered = true;
            onFootStep.Invoke();
        }
        else if(isTriggered = true && Sin < -0.97f)
        {
            isTriggered = false;
        }
    }

    #region Enable/Disable
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion
}
