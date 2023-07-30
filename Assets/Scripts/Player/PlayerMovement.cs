using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SimpleMove(Vector3 direction) 
    {
        Vector3 movement = _agent.speed * Time.deltaTime * direction;
        transform.LookAt(transform.position + movement, Vector3.up);
        _agent.Move(movement);
    }
}
