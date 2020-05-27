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
    public bool sameLane;
    private IEnumerator stoppedCoroutine;
    private IEnumerator changedCoroutine;
    
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
            if(canChange)
            {
                changeLane(Random.Range(0,2));
                StartCoroutine(changeDelay(changeDelayTime));   
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
