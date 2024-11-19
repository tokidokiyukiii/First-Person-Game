using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = System.Random;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform player;
    [SerializeField] private Transform playerWaypoint;

    public LayerMask whatIsGround, whatIsPlayer;
    
    //Patrolling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;
    
    [SerializeField] private Transform[] ffWaypoints;
    [SerializeField] private Transform[] sfWaypoints;
    public Transform currentWaypoint;
    public Transform finalWaypoint;

    public bool isOnFirst = false;
    public Transform FFWaypoint;

    public bool isOnSecond = true;
    public Transform SFWaypoint;
    
    //States
    public float sightRange;
    public bool playerInSightRange;

    public float attackRange;
    public bool playerInAttackRange;

    public bool isSeen = false;

    public FirstPersonControls firstPersonControls;
    public ThoughtCount thoughtCount;

    public bool canEnemyMove = false;
    public bool isKeyActive = false;

    public AudioSource audioSource;
    private Coroutine soundCoroutine = null;

    public AudioClip hummingOne;
    public AudioClip hummingTwo;
    public AudioClip laughingSpawn;

    public GameObject chasingSound;

    public bool isOnSameFloor = true;
    public bool isInBedroom;
    public bool isPlaying;

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
                    //if (isPlaying)
                      //  StopCoroutine(PlayRandomSound());
                    agent.isStopped = true; 
                    agent.velocity = Vector3.zero;
                    //agent.ResetPath();
                    chasingSound.SetActive(false);
                }
                else
                {
                    agent.isStopped = false;
                    if ((playerInSightRange && !firstPersonControls.isInAttic && isOnSameFloor && !isInBedroom) || isKeyActive)
                    {
                        //if (isPlaying)
                          //  StopCoroutine(PlayRandomSound());
                        
                        //Change to Chase Speed and Acceleration
                        agent.speed = 30f;
                        agent.acceleration = 20f;
                        agent.stoppingDistance = 10f;
                        
                        chasingSound.SetActive(true);
                        ChasePlayer();

                        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
                        if (playerInAttackRange)
                        {
                            CharacterController controller = player.GetComponent<CharacterController>();
                            controller.enabled = false;
                            player.position = playerWaypoint.position;
                            controller.enabled = true;
                            //firstPersonControls.isFirst = true;
                            
                            MoveFloors(1);
                            //StartCoroutine(MoveFloors(1));
                            if (thoughtCount.thoughtCount <= 10 && player.position == playerWaypoint.position)
                            {
                                isOnSameFloor = false;
                            }
                        }
                    }
                    else if (!playerInSightRange)
                    {
                        //Change to Patrol Speed and Acceleration
                        agent.speed = 10f;
                        agent.acceleration = 5f;
                        agent.stoppingDistance = 0f;
                        
                        chasingSound.SetActive(false);
                        Patrol();
                    }
                }
            }
            else if (!firstPersonControls.isInputEnabled)
            {
                chasingSound.SetActive(false);
                agent.isStopped = true; 
                agent.velocity = Vector3.zero;
            }
            
            if (!isPlaying && !isSeen && !playerInSightRange && soundCoroutine == null)
            {
                PlayHumming();
            }
            else if (isSeen || !firstPersonControls.isGameplay || playerInSightRange)
            {
                StopHumming();
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
        StartCoroutine(RespawnDelay(floor));
    }

    private IEnumerator RespawnDelay(int floor)
    {
        yield return new WaitForSeconds(10f);
        
        if (canEnemyMove && thoughtCount.thoughtCount >= 10)
        {
            agent.enabled = false;
            
            if (floor == 1 && !isOnFirst)
            {
                isOnSecond = false;
                isOnFirst = true;
                transform.position = FFWaypoint.position;
                ChooseRandomWaypoint();
            }
            else if (floor == 2 && !isOnSecond)
            {
                isOnFirst = false;
                isOnSecond = true;
                transform.position = SFWaypoint.position;
                ChooseRandomWaypoint();
            }
            
            agent.enabled = true;
        }
    }

    public void PlayHumming()
    {
        if (soundCoroutine == null)
            StartCoroutine(PlayRandomSound());
    }

    public void StopHumming()
    {
        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
            soundCoroutine = null;
            isPlaying = false;
        }
    }
    
    private IEnumerator PlayRandomSound()
    {
        while (firstPersonControls.isGameplay && !isSeen && !playerInSightRange)// || isOnSameFloor)
        {
            // Wait for a random interval between minInterval and maxInterval
            isPlaying = true;
            
            float waitTime = UnityEngine.Random.Range(20f, 120f);
            yield return new WaitForSeconds(waitTime);

            int audioNum = UnityEngine.Random.Range(0, 2);
            AudioClip selectedClip = (audioNum == 0) ? hummingOne : hummingTwo;
            audioSource.PlayOneShot(selectedClip);
            
            yield return new WaitForSeconds(selectedClip.length);
        }
        
        soundCoroutine = null;
        isPlaying = false;
    }
    
    public void LastEnemyPos()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        transform.tag = "EnemyInteract";
        transform.position = finalWaypoint.position;
        Debug.Log("Moving enemy to position " + finalWaypoint);
    }
}
