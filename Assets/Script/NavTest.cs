using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour
{

    [SerializeField] Transform target;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        agent.SetDestination(target.position);
    }
}
