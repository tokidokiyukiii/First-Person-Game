using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackFloor : MonoBehaviour
{
    public bool isFirst;
    public bool isSecond;
    
    public EnemyAI enemyAI;
    
    private void OnTriggerEnter(Collider other)
    {
        if (isFirst)
        {
            enemyAI.MoveFloors(1);
        }
        else if (isSecond)
        {
            enemyAI.MoveFloors(2);
        }

        if (enemyAI.thoughtCount.thoughtCount < 10)
        {
            if (isFirst)
                enemyAI.isOnSameFloor = false;
        }
        else
        {
            enemyAI.isOnSameFloor = true;
        }
    }
}
