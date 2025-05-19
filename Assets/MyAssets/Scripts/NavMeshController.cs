using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    [SerializeField]private NavMeshAgent agent;
    [SerializeField]private Transform target;

    private void Update()
    {
        agent.SetDestination(target.position);
    }
}
