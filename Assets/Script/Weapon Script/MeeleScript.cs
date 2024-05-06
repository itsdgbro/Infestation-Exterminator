using UnityEngine;
using UnityEngine.InputSystem;

public class MeeleScript : MonoBehaviour
{
    private Animator animator;

    [Header("References")]
    [SerializeField] private MeleeData meleeData;
    [SerializeField] private Camera fpsCam;

    private bool canAttack = true;
    private bool attackReverse = false;

    [Header("AduioClips")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip drawAudio;
    [SerializeField] private GameObject attackHolePrefab;

    private RaycastHit hitInfo;

    [Header("Game Manager Reference")]
    [SerializeField] private GameManager gameManager;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // action subscribe
        PlayerInputHandler.Instance.FireAutoAction.started += HandleAttackInput;
        PlayerInputHandler.Instance.FireAutoAction.canceled += HandleAttackInput;
    }

    private bool ResetAttack() => canAttack = true && this.gameObject.activeSelf;

    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (context.started && canAttack && !gameManager.GetIsGamePaused())
        {
            StartAutoFire();
        }
        else if (context.canceled)
        {
            StopAutoFire();
        }
    }

    private void StartAutoFire()
    {
        InvokeRepeating(nameof(PerformAttack), 0f, 1 / (meleeData.fireRate / 60)); // Start automatic firing
    }

    private void StopAutoFire()
    {
        CancelInvoke(nameof(PerformAttack)); // Stop automatic firing
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

    public void PlayDrawSound()
    {
        audioSource.PlayOneShot(drawAudio);
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

    private void OnDisable()
    {
        // action unsubscribe
        PlayerInputHandler.Instance.FireAutoAction.started -= HandleAttackInput;
        PlayerInputHandler.Instance.FireAutoAction.canceled -= HandleAttackInput;
    }
}
