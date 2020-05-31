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
            RaycastHit hitLeft;
            Vector3 highObjectleftRight = subject.transform.position + new Vector3(0,5,0);
            Vector3 left = subject.transform.position + new Vector3(rayCastLeftOffset,lanes[currentLane].position.y,0);
            Vector3 leftDirection = left - highObjectleftRight;

            RaycastHit hitRight;
            Vector3 right = subject.transform.position + new Vector3(rayCastRightOffset,lanes[currentLane].position.y,0);
            Vector3 rightDirection = right - highObjectleftRight;

            leftOpen = Physics.Raycast(highObjectleftRight,leftDirection,out hitLeft);
            rightOpen = Physics.Raycast(highObjectleftRight,rightDirection,out hitRight);

            Debug.DrawRay(highObjectleftRight, leftDirection, Color.blue);
            Debug.DrawRay(highObjectleftRight, rightDirection, Color.red);

            if(!stopped && (distanceToPlayer <= threatDistance) && !stoppedOnce)
            {
                StartCoroutine(stopDelay(Random.Range(rangeStopMin,rangeStopMax)));
                stoppedOnce = true;
            }
            
            else if(distanceToPlayer >= threatDistance && !stopped && !pushing)
            {
                StartCoroutine(pushLength(Random.Range(rangePushMin,rangePushMax)));
            }

            if(distanceToPlayer <= threatDistance && (leftOpen || rightOpen) && stoppedOnce)
            {
                lanes[currentLane].minorEnemyCount --;
                if(leftOpen && rightOpen)
                {
                    changeLane(Random.Range(0,2));
                    stoppedOnce = false;
                }
                else if(leftOpen)
                {
                    changeLane(1);
                    stoppedOnce = false;
                }
                else if(rightOpen)
                {
                    changeLane(0);
                    stoppedOnce = false;
                }
                lanes[currentLane].minorEnemyCount ++;
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
