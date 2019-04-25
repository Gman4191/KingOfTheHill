using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    private Vector3 destination;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
    }
    void FixedUpdate()
    {
        destination = player.transform.position;
        agent.destination = destination;
    }
}
