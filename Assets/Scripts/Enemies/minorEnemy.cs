using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minorEnemy : EnemyMovement
{
    public float threatDistance;
    public float rangePushMin;
    public float rangePushMax;
    public float rangeStopMin;
    public float rangeStopMax;
    public float rayCastLeftOffset;
    public float rayCastRightOffset;
    public bool stoppedOnce;
    public bool leftOpen;
    public bool rightOpen;

    override public void Start()
    {
        base.Start();
        
    }
    override public void Update()
    {   
        
        if(player.GetComponent<MovementController>().currentLane == currentLane)
        {

            if(!stopped && (distanceToPlayer <= threatDistance) && !stoppedOnce)
            {
                StartCoroutine(stopDelay(Random.Range(rangeStopMin,rangeStopMax)));
                stoppedOnce = true;
            }
            
            else if(distanceToPlayer >= threatDistance && !stopped && !pushing && stoppedOnce)
            {
                pushing = true;
            }
        }

        else
        {
            if(!pushing)
            {
                pushing = true;
                if(stopped)
                {
                    stopped = false;
                }
            }
        }
        
        base.Update(); 
        lanes[currentLane].minorEnemyCount ++;

    }
    protected IEnumerator stopDelay(float delayLength)
    {
        stopped = true;

        yield return new WaitForSeconds(delayLength);

        stopped = false;

        yield return null;
    }

    protected IEnumerator pushLength(float delayLength)
    {
        pushing = true;

        yield return new WaitForSeconds(delayLength);

        pushing = false;

        yield return null;
    }

    private void OnDestroy() 
    {
        lanes[currentLane].minorEnemyCount --;
    }
}
