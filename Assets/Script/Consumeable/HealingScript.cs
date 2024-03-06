using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingScript : MonoBehaviour
{
    private Animator animator;
    [Header("Reference to Healing Data")]
    [SerializeField] private HealData data;

    private PlayerStat playerStat;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerStat = GetComponentInParent<PlayerStat>();
        if (playerStat == null)
            Debug.LogWarning("PlayerStat not found");
    }

    // Trigger from animation event
    public void Heal()
    {
        playerStat.RestoreHealth(data.healAmount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerStat.GetHealth() < 100)
            {
                animator.Play("Heal");
            }
        }
    }
}
