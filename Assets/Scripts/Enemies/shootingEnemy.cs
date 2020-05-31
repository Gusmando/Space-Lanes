using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingEnemy : EnemyMovement
{
    public GunController gunContr; 
    public float threatDistance;
    public bool canChange;
    public float changeDelayTime;
    public float shotDelayTime;
    public float reloadTime;
    public float sameLaneShotDelay;
    public float sameLaneReloadTime;
    public float rayCastLeftOffset;
    public float rayCastRightOffset;
    public bool leftOpen;
    public bool rightOpen;
    public bool sameLane;
    public bool laneSkip;
    public int shifting;
    private IEnumerator stoppedCoroutine;
    private IEnumerator changedCoroutine;
    override public void Start()
    {
        base.Start();
        lanes[currentLane].shootingEnemyCount ++;
    }
    override public void Update()
    {
        if(!gunContr.input)
        {
            gunContr.input = true;
        }

        if(gunContr.currentGun.clipSize <= 0)
        {
            gunContr.reload = true;
        }
        sameLane = player.GetComponent<MovementController>().currentLane == currentLane;
        bool inRange = distanceToPlayer <= threatDistance;
        if(!inRange)
        {
            if(!pushing)
            {
                pushing = true;
                stopped = false;
            }
        }

        else
        {
            if(pushing)
            {
                pushing = false;
            }
        }

        if(sameLane)
        {
            gunContr.reloadTime = sameLaneReloadTime;
            gunContr.currentGun.delayTime = sameLaneShotDelay;
        }
        else
        {
            gunContr.reloadTime = this.reloadTime;
            gunContr.currentGun.delayTime= shotDelayTime;
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

            if(canChange)
            {
                lanes[currentLane].shootingEnemyCount --;
                shifting = Random.Range(0,2);
                
                if(shifting == 0 && leftOpen && currentLane - 1 != gameManager.lowActiveLane)
                {
                    changeLane(shifting);
                }

                if(shifting == 1 && rightOpen && currentLane + 1 != gameManager.lowActiveLane)
                {
                    changeLane(shifting);
                }
                
                lanes[currentLane].shootingEnemyCount ++;
                StartCoroutine(changeDelay(changeDelayTime));   
            }
        }
        lanes[currentLane].shootingEnemyCount ++;
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

    private void OnDestroy() 
    {
        lanes[currentLane].shootingEnemyCount --;
    }
}
