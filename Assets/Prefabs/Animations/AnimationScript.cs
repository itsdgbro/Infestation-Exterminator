using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Set the trigger to start or continue the "shoot" animation
                animator.SetTrigger("shoot");
            }
        }
    }

    // This method is called as an Animation Event to reset the isShooting trigger
    public void ResetShootTrigger()
    {
        animator.ResetTrigger("shoot");
    }
}
