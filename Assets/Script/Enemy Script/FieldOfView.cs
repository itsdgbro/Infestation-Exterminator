using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;

#if UNITY_EDITOR
#endif

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    [SerializeField] private Transform eyes;
    [SerializeField]private GameObject referenceForEnemyAttack;

    // Player transform
    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private readonly string isTargetVisible = "isTargetVisible"; // animator parameter
    private readonly string attackParameter = "attack";

    EnemyVelocityController velocityController;
    private bool isTargetVisibleRaycast;

    private Target zombieHealth;

    // Attack script ref
    private EnemyAttack enemyAttack;

    // zombie FOV range
    [Range(0, 360)]
    private float viewAngle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        velocityController = GetComponentInChildren<EnemyVelocityController>();
        zombieHealth = GetComponent<Target>();
        enemyAttack = referenceForEnemyAttack.GetComponent<EnemyAttack>();

        // to ignore collision between enemies
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in zombies)
        {
            // Get the colliders of the target GameObject
            CapsuleCollider[] targetColliders = target.GetComponents<CapsuleCollider>();

            // Get the collider of the zombie GameObject
            CapsuleCollider zombieCollider = GetComponent<CapsuleCollider>();

            // Ignore collisions between each collider of the target and the zombie collider
            foreach (CapsuleCollider targetCollider in targetColliders)
            {
                if (targetCollider != null && zombieCollider != null)
                {
                    Physics.IgnoreCollision(targetCollider, zombieCollider, true);
                }
            }
        }
    }


    private void Update()
    {

        FindVisibleTargets();
        ToggleFOV();
        enemyAttack.canAttack =  !velocityController.IsAttackAnimationPlaying();
    }

    void FindVisibleTargets()
    {
        // Radius of the game object
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, zombieData.viewRadius, zombieData.targetMask);
        GameObject player = GameObject.FindWithTag("Player");

        target = null; // Reset the target

        // default target is not visible
        animator.SetBool(isTargetVisible, false);

        if (zombieHealth.isZombieArgo && player!=null && !zombieHealth.GetIsDead())
        {
            float rotationSpeed = agent.angularSpeed;
            Quaternion targetRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            agent.SetDestination(player.transform.position);
            animator.SetBool(isTargetVisible, true);
        }

        foreach (var potentialTargetCollider in targetsInViewRadius)
        {
            Debug.Log("AA");
            Transform potentialTarget = potentialTargetCollider.transform;
            Vector3 dirToTarget = (potentialTarget.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, potentialTarget.position);
                // RayCast hits target and there is no obstacles
                isTargetVisibleRaycast = !(Physics.Raycast(eyes.position, dirToTarget, dstToTarget, zombieData.obstacleMask));
                // Debug.Log(isTargetVisibleRaycast + " visile");

                // Debug.Log(gameObject.name + " " + dstToTarget);
                // target is visible
                if (isTargetVisibleRaycast && !zombieHealth.GetIsDead())
                {
                    // Rotation towards the target
                    float rotationSpeed = agent.angularSpeed;
                    Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                    target = potentialTarget;

                    // Move towards the target
                    agent.SetDestination(target.position);
                    animator.SetBool(isTargetVisible, true);
                    if (!velocityController.IsAttackAnimationPlaying() && enemyAttack.canAttack)
                    {
                        AttackTrigger(dstToTarget);
                    }
                }
                else
                {
                    animator.SetBool(isTargetVisible, false);
                    zombieHealth.isZombieArgo = false;
                }

            }
        }
    }

    void AttackTrigger(float distance)
    {

        if (distance <= zombieData.maxRange)
        {
            // attack animation
            animator.Play(attackParameter);
            StartCoroutine(ResetCooldown());
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(zombieData.attackCooldown);
    }

    private void ToggleFOV()
    {
        float defaultFOV = 165f;
        float allFOV = 360f;

        viewAngle = isTargetVisibleRaycast ? allFOV : defaultFOV;

    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(eyes.transform.position, Vector3.up, Vector3.forward, 360, zombieData.viewRadius);
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);
        Handles.DrawLine(eyes.transform.position, eyes.transform.position + viewAngleA * zombieData.viewRadius);
        Handles.DrawLine(eyes.transform.position, eyes.transform.position + viewAngleB * zombieData.viewRadius);

        Handles.color = Color.red;
        if (target != null)
        {
            Handles.DrawLine(eyes.transform.position, target.position);
        }
    }
#endif
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
