using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minorEnemy : EnemyMovement
{
    [Header("Assignments")]
    public float threatDistance;
    public float changeDelayTime;

    [Header("State Vars")]
    public bool canChange;

    override public void Start()
    {
        //Calling base enemy movement start function and
        //assigning a random lane and adding to enemy count
        base.Start();
        currentLane = Random.Range(0,lanes.Length); 
        gameManager.currentLanes[currentLane].minorEnemyCount ++;
        
    }
    override public void Update()
    {   
        //Enemy Count update
        gameManager.currentLanes[currentLane].minorEnemyCount --;

        //Behavior for being in the same lane as player
        if(player.GetComponent<MovementController>().currentLane == currentLane)
        {
            if(!pushing)
            {
                pushing = true;
            }
            
            //Since Minor Enemies really only affect score, they will attempt to 
            //avoid being in the player's lane - they do not attack 
            if(distanceToPlayer <= threatDistance && canChange && !changing && !jumping)
            {
                if(!changing)
                {
                    int shifting = Random.Range(0,2);
                    
                    if(shifting == 0)
                    {
                        changeLane(shifting);
                        StartCoroutine(changeDelay(changeDelayTime));  
                    }

                    if(shifting == 1)
                    {
                        changeLane(shifting);
                        StartCoroutine(changeDelay(changeDelayTime));  
                    }
                }
            }
        }

        //Otherwise they are always set to push forward
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
        gameManager.currentLanes[currentLane].minorEnemyCount ++;
        base.Update(); 

        //Depending on the velocity, the run speed is set
        if(pushing && !jumping && !(subjectRb.velocity.z/maxSpeed<0))
        {
            anim.speed = (subjectRb.velocity.z/maxSpeed)*.2f;
        }

        //Otherwise if there is no jumping then there should
        //be no movement
        else
        {
            if(!jumping)
            {
                //Ensures that falling off stage conserves acceleration
                if(!falling)
                {
                    subjectRb.velocity = new Vector3(0,0,0);
                }
                anim.speed = 0;
            } 
        }
        
        //If a left or right dash should be happening, anim state vars change
        if(!animOver)
        {
            if(!leftRight)
            {
                anim.SetInteger("animState",10);
            }
            else
            {
                anim.SetInteger("animState",1);
            }
        }

        //Otherwise depending on the y velocity another animation state is determined
        else
        {
            if((subjectRb.velocity.y == 0 || pushing) && !jumping)
            {
                anim.SetInteger("animState",0);
            }

            else if(subjectRb.velocity.y > 0)
            {
                anim.SetInteger("animState",111);
            }
            else if(subjectRb.velocity.y < -.05)
            {
                anim.SetInteger("animState",100);
            }
        }

        //Hurt animatyion controls
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
    //Delay for being able to change lanes
    protected IEnumerator changeDelay(float delayLength)
    {
        canChange = false;
        yield return new WaitForSeconds(delayLength);
        canChange = true;
        yield return null;
    }

    private void OnDestroy() 
    {
        lanes[currentLane].minorEnemyCount --;
    }
}
