using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbingEnemy : EnemyMovement
{
    public GunController gunContr; 
    public float threatDistance;
    public float reloadTime;
    public bool sameLane;
    public bool canShoot;
    public bool inRange;
    override public void Start()
    {
        base.Start();
    }

    override public void Update()
    {
        sameLane = GameObject.FindWithTag("Player").GetComponent<MovementController>().currentLane == currentLane;
        bool inRange = distanceToPlayer <= threatDistance;
        if(sameLane && inRange)
        {
            if(canShoot)
            {
                if(!gunContr.input && !hurt)
                {
                    gunContr.input = true;
                }
            }
        }

        else
        {
            if(gunContr.input)
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
    }   

    protected IEnumerator shootDelay(float delayLength)
    {
        canShoot = false;

        yield return new WaitForSeconds(delayLength);

        canShoot = true;

        yield return null;
    }
}
