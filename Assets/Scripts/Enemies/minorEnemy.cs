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
    public bool stoppedOnce;
    override public void Update()
    {   
        if(player.GetComponent<MovementController>().currentLane == currentLane)
        {
            if(!stopped && distanceToPlayer <= threatDistance && !stoppedOnce && !pushing)
            {
                StartCoroutine(stopDelay(Random.Range(rangeStopMin,rangeStopMax)));
                stoppedOnce = true;
            }
            
            else if(distanceToPlayer >= threatDistance && !stopped && !pushing)
            {
                StartCoroutine(pushLength(Random.Range(rangePushMin,rangePushMax)));
            }

            if(stoppedOnce && !stopped && distanceToPlayer <= threatDistance)
            {
                changeLane(Random.Range(0,2));
                stoppedOnce = false;
            }
        }

        else
        {
            if(!stopped && !pushing)
            {
                StartCoroutine(pushLength(Random.Range(rangePushMin,rangePushMax)));
            }
        }
        
        base.Update(); 

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

}
