using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrol : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform movePos1;
    [SerializeField] private Transform movePos2;

    private Transform currentTarget;

    public float stoppingDistance = 1.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        currentTarget = movePos1;
        agent.SetDestination(currentTarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (currentPos == 0)
        {
            agent.SetDestination(movePos1.position);
        }
        else if (currentPos == 1)
        {
            agent.SetDestination(movePos2.position);
        }
        else if (currentPos == 2)
        {
            agent.SetDestination(movePos1.position);
        }*/

        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            currentTarget = (currentTarget == movePos1) ? movePos2 : movePos1;
            agent.SetDestination(currentTarget.position);
        }
    }
}
