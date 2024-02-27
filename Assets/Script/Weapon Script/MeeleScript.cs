using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private GameObject attackHolePrefab;
    private RaycastHit hitInfo;

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
        if (playerControls.Movement.Fire.ReadValue<float>() > 0.1f && canAttack)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {

        if (attackReverse)
        {
            animator.Play("r_attack");
            Attack();
        }
        else
        {
            animator.Play("l_attack");
            Attack();
        }
        audioSource.PlayOneShot(attackAudio);
        attackReverse = !attackReverse;
        canAttack = false;
        Invoke(nameof(ResetAttack), meleeData.fireRate);
    }

    private void Attack()
    {
        Vector3 rayOrigin = fpsCam.transform.position;
        Vector3 rayDirection = fpsCam.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, meleeData.maxDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            ISTarget isTarget = hitObject.GetComponentInParent<ISTarget>();
            isTarget?.TakeDamage(meleeData.damage);
        }
    }

    public void AttackHoleEffect()
    {
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Target"))
            {
                Debug.Log("HIT ENEMY");
            }
            else if (hitInfo.collider.CompareTag("Environment"))
            {
                GameObject impact = Instantiate(attackHolePrefab, hitInfo.point, Quaternion.identity);
                impact.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);

                impact.transform.Translate(hitInfo.normal * 0.02f, Space.World);

                Destroy(impact, 2f);
            }
            else
            {
                return;
            }
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
