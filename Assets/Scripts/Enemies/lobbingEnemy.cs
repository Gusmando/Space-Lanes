using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbingEnemy : EnemyMovement
{
    public GunController gunContr; 
    public float threatDistance;
    public bool sameLane;
    public bool inRange;
    override public void Start()
    {
        base.Start();
        currentLane = openLane(); 
        gameManager.currentLanes[currentLane].lobbingEnemyCount ++;
    }

    override public void Update()
    {
        gameManager.currentLanes[currentLane].lobbingEnemyCount --;
        sameLane = GameObject.FindWithTag("Player").GetComponent<MovementController>().currentLane == currentLane;
        inRange = distanceToPlayer <= threatDistance;
        if(sameLane && inRange)
        {
            if(!gunContr.input && !hurt)
            {
                gunContr.input = true;
            }
        }

        else
        {
            if(gunContr.input && gunContr.reloading)
            {
                gunContr.input = false;
            }
        }

        if(gunContr.currentGun.clipSize <= 0)
        {
            gunContr.reload = true;
        }
        else
        {
            gunContr.reload = false;
        }

        base.Update();
        gameManager.currentLanes[currentLane].lobbingEnemyCount ++;
    }

    private void OnDestroy() 
    {
        lanes[currentLane].lobbingEnemyCount --;
    } 

    private int openLane()
    {
        int openLane = 0;
        for(int i = 0; i< gameManager.currentLanes.Length;i ++)
        {
            if(gameManager.currentLanes[i].lobbingEnemyCount == 0)
            {
                openLane = i;
            }
        }

        return openLane;
    }
}
