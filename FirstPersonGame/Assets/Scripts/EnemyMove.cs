using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private bool hasEnemyMoved = false;
    public Transform enemy;

    public float targetDistance;
    public float movingSpeed;
    public float moveDuration = 2f;

    public Vector3 targetDirection = new Vector3(0, 0, 1);
    private Vector3 targetPosition;

    public EnemyAI enemyAI;

    public bool isLastScare;
    public Transform newPos;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasEnemyMoved && !enemyAI.canEnemyMove)
            {
                targetPosition = enemy.position + targetDirection.normalized * targetDistance;

                StartCoroutine(MoveEnemy());
                hasEnemyMoved = true;
            }
        }
    }
    

    IEnumerator MoveEnemy()
    {
        Vector3 startPosition = enemy.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Lerp between start and target positions based on elapsed time
            enemy.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches the exact target position at the end
        enemy.position = targetPosition;
        
        Debug.Log("Moving Enemy to new position");
        enemy.position = newPos.position;
        enemy.rotation = newPos.rotation;
        
        if (isLastScare)
            enemy.gameObject.SetActive(false);
            
        //targetPosition = enemy.position + targetDirection.normalized * targetDistance;
    }
}
