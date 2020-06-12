using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbingEnemy : EnemyMovement
{
    public GunController gunContr; 
    public float threatDistance;
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
    }   
}
