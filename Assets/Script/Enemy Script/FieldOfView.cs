using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    [SerializeField] private Transform eyes;

    // Player transform
    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private readonly string isTargetVisible = "isTargetVisible";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
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

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform potentialTarget = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (potentialTarget.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < zombieData.viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, potentialTarget.position);

                // RayCast hits target and there is no obstacles
                if (!(Physics.Raycast(eyes.position, dirToTarget, dstToTarget, zombieData.obstacleMask)))
                {
                    // Set the target only if it's visible
                    target = potentialTarget;

                    // move animation
                    animator.SetBool(isTargetVisible, true);
                    
                    // rotation towards the target
                    float rotationSpeed = 5.0f; // Adjust the speed as needed
                    Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                    // move towards the target
                    agent.SetDestination(target.position);
                }
                else
                {   
                    // target is not visible
                    animator.SetBool(isTargetVisible, false);
                }
            }
        }
    }


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

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
