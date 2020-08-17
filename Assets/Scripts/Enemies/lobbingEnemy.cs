using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbingEnemy : EnemyMovement
{
    [Header("Assignments")]
    public GunController gunContr; 
    public float threatDistance;
    public float timeDelay;

    [Header("State Variables")]
    public bool sameLane;
    public bool inRange;
    public bool changeStage;
    public bool manualPos;
    public int shotStage;
    public int initLane;
    override public void Start()
    {
        //Running the main enemy movement 
        //script start function
        base.Start();

        //Lobbed enemies can be manually
        //placed on platforms without using
        //a spawner.
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
        //Manual positioned enemies will not effect
        //the counted total on each lane
        if(!manualPos)
        {
            gameManager.currentLanes[currentLane].lobbingEnemyCount --;
        }
        else
        {
            //Manually Positioned enemies auto assign to 
            //the largest lane array for reference
            lanes = gameManager.largestLanes;
            changed = true;
        }

        //Keeping track of the player and enemy lane allows 
        //more specified and dynamic ai response
        sameLane = GameObject.FindWithTag("Player").GetComponent<MovementController>().currentLane == currentLane;
        inRange = distanceToPlayer <= threatDistance;

        //The enemy will shoot a barrage when the player 
        //is on the same lane; animation and gun input changes
        //depending on player information
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

        //Reload automatically when gun clip runs to 0
        if(gunContr.currentGun.clipSize <= 0)
        {
            gunContr.reload = true;
        }
        else
        {
            gunContr.reload = false;
        }

        //Base Enemy Movement script must run
        base.Update();

        //Enemy Count Update
        if(!manualPos)
        {
            gameManager.currentLanes[currentLane].lobbingEnemyCount ++;
        }

        //Animation Control for Shooting
        if(!gunContr.reloading && gunContr.input)
        {
            anim.SetBool("shooting",true);
        }
        else
        {
            anim.SetBool("shooting",false);
        }

        //shotStage is updated on a timer to show
        //the gradual mouth opening over time 
        if(changeStage && anim.GetBool("shooting"))
        {
            shotStage ++;
            StartCoroutine(shotNimDelay(timeDelay));
        }

        //When the number hits 5, the shotstage variable
        //is reset for the upcoming cycle
        if(shotStage == 5)
        {
            shotStage = 0;
        }

        anim.SetInteger("lobStage",shotStage);

        //Hurt animation stagr check and set
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

    //On death/destruction fo enemy
    //count is updated
    private void OnDestroy() 
    {
        if(!manualPos)
        {
            lanes[currentLane].lobbingEnemyCount --;
        }
    } 

    //Helper function used to check for open lanes
    //returning the correct one easily
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

    //Shot animation delay
    protected IEnumerator shotNimDelay(float delayLength)
    {
        changeStage = false;
        yield return new WaitForSeconds(delayLength);
        changeStage = true;;
        yield return null;
    }
}
