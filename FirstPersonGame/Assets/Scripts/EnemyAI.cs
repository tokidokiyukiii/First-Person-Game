using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    
    //Patrolling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;
    
    [SerializeField] private Transform[] waypoints;
    
    [SerializeField] private Transform[] ffWaypoints;
    [SerializeField] private Transform[] sfWaypoints;
    public Transform currentWaypoint;

    public bool isOnFirst = false;
    public Transform FFWaypoint;

    public bool isOnSecond = true;
    public Transform SFWaypoint;
    
    //States
    public float sightRange;
    public bool playerInSightRange;

    public bool isSeen = false;

    public FirstPersonControls firstPersonControls;

    public bool canEnemyMove = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        ChooseRandomWaypoint();
    }

    void Update()
    {
        if (canEnemyMove)
        {
            agent.enabled = true;
            
            if (firstPersonControls.isInputEnabled)
            {
                //Check for sight and attack range
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

                if (isSeen)
                {
                    agent.isStopped = true; 
                    agent.velocity = Vector3.zero;
                    //agent.ResetPath();
                }
                else
                {
                    agent.isStopped = false;
                    if (!playerInSightRange)
                    {
                        //Change to Patrol Speed and Acceleration
                        agent.speed = 10f;
                        agent.acceleration = 5f;
                        agent.stoppingDistance = 0f;
                
                        Patrol();
                    }
                    else
                    {
                        //Change to Chase Speed and Acceleration
                        agent.speed = 30f;
                        agent.acceleration = 20f;
                        agent.stoppingDistance = 10f;
            
                        ChasePlayer();
                    }
                }
            }
            else if (!firstPersonControls.isInputEnabled)
            {
                agent.isStopped = true; 
                agent.velocity = Vector3.zero;
            }
        }
    }

    private void Patrol()
    {
        // Move towards the current waypoint
        agent.SetDestination(currentWaypoint.position);
        //Debug.Log("Moving to waypoint: " + currentWaypoint.position);

        // Check if the enemy has reached the waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 5f)
        {
            Debug.Log("Reached waypoint: " + currentWaypoint.name);
            ChooseRandomWaypoint(); // Choose a new waypoint once the current one is reached
        }
    }

    private void ChooseRandomWaypoint()
    {
        if (waypoints.Length == 0) 
        {
            Debug.LogWarning("There are no waypoints!");
            return; // Exit if there are no waypoints
        }
        
        //Debug.Log("Total waypoints: " + waypoints.Length);

        if (isOnFirst)
        {
            int randomIndex = UnityEngine.Random.Range(0, ffWaypoints.Length);
            currentWaypoint = ffWaypoints[randomIndex];
        }
        else if (isOnSecond)
        {
            int randomIndex = UnityEngine.Random.Range(0, sfWaypoints.Length);
            currentWaypoint = sfWaypoints[randomIndex];
        }
        
        //int randomIndex = UnityEngine.Random.Range(0, waypoints.Length);
        //currentWaypoint = waypoints[randomIndex];
    }
    
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void MoveFloors(int floor)
    {
        if (canEnemyMove)
        {
            agent.enabled = false;
            
            if (floor == 1 && !isOnFirst)
            {
                isOnFirst = true;
                transform.position = FFWaypoint.position;
                isOnSecond = false;
            }
            else if (floor == 2 && !isOnSecond)
            {
                isOnSecond = true;
                transform.position = SFWaypoint.position;
                isOnFirst = false;
            }
            
            agent.enabled = true;
        }
    }
}
