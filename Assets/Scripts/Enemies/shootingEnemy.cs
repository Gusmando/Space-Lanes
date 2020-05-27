using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingEnemy : EnemyMovement
{
    public GunController gunContr; 
    public float threatDistance; 
    public bool stoppedOnce;
    public bool shootOn;
    public bool canChange;
    public float changeDelayTime;
    public float rangeStopMin;
    public float rangeStopMax;
     public float rangePushMin;
    public float rangePushMax;
    public float barrageDelay;
    public float barrageChangeTime;
    public float lowHealth;
    
    
    override public void Update()
    {
        bool sameLane = player.GetComponent<MovementController>().currentLane == currentLane;
        bool inRange = distanceToPlayer <= threatDistance;

        if(!stopped && inRange && sameLane && pushing)
        {
            pushing = false;
            stopped = true;
        }

        if(sameLane)
        {   

            StartCoroutine(changeDelay(changeDelayTime));
            
            if(!gunContr.shooting)
            {
                gunContr.shooting = true;
            }

            if(!pushing && !inRange)
            {
                pushing = true;
            }
        }

        else if(!sameLane)
        {
            if(canChange && health >= lowHealth)
            {
                changeLane(Random.Range(0,2));
                StartCoroutine(changeDelay(changeDelayTime));
            }

            if(!pushing && !inRange && !stopped)
            {
               pushing = true;
            }

            if(health <= lowHealth)
            {
                changeDelayTime = barrageChangeTime;

                if(!gunContr.shooting)
                {
                    gunContr.shooting = true;
                }

                if(canChange)
                {
                    changeLane(Random.Range(0,2));
                    StartCoroutine(changeDelay(changeDelayTime));
                }
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

    protected IEnumerator pushingLength(float delayLength)
    {
        pushing = true;

        yield return new WaitForSeconds(delayLength);

        pushing = false;

        yield return null;
    }
    protected IEnumerator changeDelay(float delayLength)
    {
        canChange = false;

        yield return new WaitForSeconds(delayLength);

        canChange = true;

        yield return null;
    }
}
