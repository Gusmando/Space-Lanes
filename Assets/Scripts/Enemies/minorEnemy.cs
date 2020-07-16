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
    public float changeDelayTime;
    public bool stoppedOnce;
    public bool canChange;

    override public void Start()
    {
        base.Start();
        currentLane = Random.Range(0,lanes.Length); 
        gameManager.currentLanes[currentLane].minorEnemyCount ++;
        
    }
    override public void Update()
    {   
        gameManager.currentLanes[currentLane].minorEnemyCount --;
        if(player.GetComponent<MovementController>().currentLane == currentLane)
        {
            if(!pushing)
            {
                pushing = true;
            }

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

        //Otherwise depending on the y veloicty an another animation state is determined
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
    protected IEnumerator stopDelay(float delayLength)
    {
        stopped = true;

        yield return new WaitForSeconds(delayLength);

        stopped = false;

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
        lanes[currentLane].minorEnemyCount --;
    }
}
