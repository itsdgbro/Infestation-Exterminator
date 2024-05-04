using UnityEngine;
using UnityEngine.Events;

public class Footsteps : MonoBehaviour
{
    // PlayerControls playerControls;
    private PlayerInputHandler playerControls;

    [Range(0, 20f)]
    public float frequency = 10.0f;
    public UnityEvent onFootStep;
    float Sin;
    bool isTriggered = false;

    private void Awake()
    {
        playerControls = PlayerInputHandler.Instance;
    }

    void Update()
    {
        float inputValue = playerControls.CharacterMove.magnitude;
        if (inputValue > 0)
        {
            Debug.Log("123");
            StartFootsteps();
        }
    }

    private void StartFootsteps()
    {
        Sin = Mathf.Sin(Time.time * frequency);
        if (Sin > 0.97f && isTriggered == false)
        {
            Debug.Log("Foot");
            isTriggered = true;
            onFootStep.Invoke();
        }
        else if (isTriggered = true && Sin < -0.97f)
        {
            isTriggered = false;
        }
    }

}
