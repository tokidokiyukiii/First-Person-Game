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
            //enemyAI.isOnFirst = true;
            //enemyAI.isOnSecond = false;
            enemyAI.MoveFloors(1);
        }
        else if (isSecond)
        {
            //enemyAI.isOnSecond = true;
            //enemyAI.isOnFirst = false;
            enemyAI.MoveFloors(2);
        }
    }
}
