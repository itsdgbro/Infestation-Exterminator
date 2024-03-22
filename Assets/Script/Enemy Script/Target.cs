using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, ISTarget
{
    #region Target Health
    [SerializeField] private float health = 50f;
    public float GetZombieHealth() => health;
    #endregion

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    // GameManager
    [SerializeField]private GameManager gameManager;
    

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        {
            Debug.LogWarning("animator not found");
        }
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    IEnumerator WaitForAnimationAndDestroy(float len)
    {
        // Wait for the duration of the animation
        yield return new WaitForSeconds(len);
       
        // Destroy the GameObject
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {   
            // death
            navMeshAgent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            animator.SetTrigger("dead");
            gameManager.SetZombieAlive(this.gameObject);
        }
        else
        {
            animator.Play("Reaction");
        }
    }
}
