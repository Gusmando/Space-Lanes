using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbingEnemy : EnemyMovement
{
    public GunController gunContr; 
    public float threatDistance;
    public bool sameLane;
    public bool inRange;
    public bool changeStage;
    public int shotStage;
    public float timeDelay;
    public bool manualPos;
    public int initLane;
    override public void Start()
    {
        base.Start();
        if(manualPos)
        {
            currentLane = initLane;
        }

        else
        {
            currentLane = openLane(); 
            gameManager.currentLanes[currentLane].lobbingEnemyCount ++;
        }
        
    }

    override public void Update()
    {
        if(!manualPos)
        {
            gameManager.currentLanes[currentLane].lobbingEnemyCount --;
        }
        else
        {
            lanes = gameManager.largestLanes;
            changed = true;
        }
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
                shotStage = 0;
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

        if(!manualPos)
        {
            gameManager.currentLanes[currentLane].lobbingEnemyCount ++;
        }
        if(!gunContr.reloading && gunContr.input)
        {
            anim.SetBool("shooting",true);
        }
        else
        {
            anim.SetBool("shooting",false);
        }

        if(changeStage && anim.GetBool("shooting"))
        {
            shotStage ++;
            StartCoroutine(shotNimDelay(timeDelay));
        }

        if(shotStage == 5)
        {
            shotStage = 0;
        }

        anim.SetInteger("lobStage",shotStage);

        if(hurt)
        {
            anim.SetBool("hurt",true);
            spotLight.color = red;
        }
        else
        {
            anim.SetBool("hurt",false);
            spotLight.color = white;
        }


    }

    private void OnDestroy() 
    {
        if(!manualPos)
        {
            lanes[currentLane].lobbingEnemyCount --;
        }
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

    protected IEnumerator shotNimDelay(float delayLength)
    {
        changeStage = false;
        yield return new WaitForSeconds(delayLength);
        changeStage = true;;
        yield return null;
    }
}
