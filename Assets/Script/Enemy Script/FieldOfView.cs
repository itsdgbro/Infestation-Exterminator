using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float viewRadius;
    [Range(0, 360)]
    [SerializeField] private float viewAngle;


    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Transform eyes;
    
    // Player transform
    private Transform target;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        target = null; // Reset the target

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform potentialTarget = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (potentialTarget.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, potentialTarget.position);

                if (!(Physics.Raycast(eyes.position, dirToTarget, dstToTarget, obstacleMask)))
                {
                    // Set the target only if it's visible
                    target = potentialTarget;

                    // player detected
                    // look at player
                    transform.rotation = Quaternion.LookRotation(dirToTarget);
                    agent.SetDestination(target.position);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(eyes.transform.position, Vector3.up, Vector3.forward, 360, viewRadius);
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);
        Handles.DrawLine(eyes.transform.position, eyes.transform.position + viewAngleA * viewRadius);
        Handles.DrawLine(eyes.transform.position, eyes.transform.position + viewAngleB * viewRadius);

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
