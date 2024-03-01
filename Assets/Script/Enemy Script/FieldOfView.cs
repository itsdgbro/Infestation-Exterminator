using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

#if UNITY_EDITOR
#endif

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    [SerializeField] private Transform eyes;

    // Player transform
    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private readonly string isTargetVisible = "isTargetVisible";
    EnemyVelocityController velocityController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        velocityController = GetComponentInChildren<EnemyVelocityController>();
    }

    private void Update()
    {
        FindVisibleTargets();
    }

    void FindVisibleTargets()
    {
        // Radius of the game object
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, zombieData.viewRadius, zombieData.targetMask);

        target = null; // Reset the target

        // default target is not visible
        animator.SetBool(isTargetVisible, false);

        foreach (var potentialTargetCollider in targetsInViewRadius)
        {
            Transform potentialTarget = potentialTargetCollider.transform;
            Vector3 dirToTarget = (potentialTarget.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < zombieData.viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, potentialTarget.position);
                // RayCast hits target and there is no obstacles
                bool isTargetVisibleRaycast = !(Physics.Raycast(eyes.position, dirToTarget, dstToTarget, zombieData.obstacleMask));

                if (isTargetVisibleRaycast && !velocityController.GetIsAttacking())
                {
                    // Set the target only if it's visible
                    target = potentialTarget;

                    // move animation
                    animator.SetBool(isTargetVisible, true);

                    // rotation towards the target
                    float rotationSpeed = 5.0f; // Adjust the speed as needed
                    Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                    agent.SetDestination(target.position);
                    AttackTrigger(dstToTarget);
                }
                else if (velocityController.GetIsAttacking() && isTargetVisibleRaycast)
                {
                    animator.SetBool(isTargetVisible, true);
                }
                else
                {
                    // target is not visible
                    animator.SetBool(isTargetVisible, false);
                }
            }
        }
    }

    void AttackTrigger(float distance)
    {

        if (distance <= zombieData.maxRange)
        {
            PerfromAttack();
        }
    }

    private void PerfromAttack()
    {
        // perform attack
        animator.SetTrigger("attack");
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision");
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(eyes.transform.position, Vector3.up, Vector3.forward, 360, zombieData.viewRadius);
        Vector3 viewAngleA = DirFromAngle(-zombieData.viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(zombieData.viewAngle / 2, false);
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
