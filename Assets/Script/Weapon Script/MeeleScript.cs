using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class MeeleScript : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator animator;

    [Header("References")]
    [SerializeField] private MeleeData meleeData;
    [SerializeField] private Camera fpsCam;

    private bool canAttack = true;
    private bool attackReverse = false;

    [Header("AduioClips")]
    private AudioSource audioSource;
    [SerializeField]private AudioClip attackAudio;

    private void Awake()
    {
        playerControls = new PlayerControls();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleAttackInput();
    }

    private bool ResetAttack() => canAttack = true && this.gameObject.activeSelf;

    private void HandleAttackInput()
    {
        if (playerControls.Movement.Fire.ReadValue<float>() > 0.1f && canAttack )
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {

        if (attackReverse)
        {
            animator.Play("r_attack");

        }
        else
        {
            animator.Play("l_attack");

        }
        audioSource.PlayOneShot(attackAudio);
        attackReverse = !attackReverse;
        canAttack = false;
        Invoke(nameof(ResetAttack), meleeData.fireRate);
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
