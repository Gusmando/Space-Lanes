using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingEnemy : EnemyMovement
{
    [Header("Assignments")]
    public GunController gunContr; 
    public float threatDistance;
    public float changeDelayTime;
    public float sameLaneShotDelay;
    public float sameLaneReloadTime;
    
    [Header("State Variables")]
    public bool canChange;
    public bool sameLane;
    public int shifting;

    override public void Start()
    {
        //Calling base enemy movement start function and
        //assigning a random lane and adding to enemy count
        base.Start();
        currentLane = Random.Range(0,lanes.Length); 
        gameManager.currentLanes[currentLane].shootingEnemyCount ++;
    }
    override public void Update()
    {
        //Enemy Count Update
        gameManager.currentLanes[currentLane].shootingEnemyCount --;
        
        //Shooting enemies are always shooting unless they are
        //in the hurt state or currently reloading
        if(!gunContr.input && !hurt)
        {
            gunContr.input = true;
        }
        else
        {
            gunContr.input = false;
        }

        if(gunContr.currentGun.clipSize <= 0)
        {
            gunContr.reload = true;
        }
        else
        {
            gunContr.reload = false;
        }

        //Player and enemy relationship will change ai states
        sameLane = GameObject.FindWithTag("Player").GetComponent<MovementController>().currentLane == currentLane;
        bool inRange = distanceToPlayer <= threatDistance;

        //When not in range shooting enemies will always
        //work to move forward
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

        //When in the same lane the shooting enemies fire 
        //rate will speed up due to the situation
        if(sameLane)
        {
            gunContr.reloadTime = sameLaneReloadTime;
            gunContr.currentGun.delayTime = sameLaneShotDelay;
        }

        //Otherwise the shooting enemy is constantly lane
        //switching to find the player
        else
        {
            if(canChange && !noGap && !changing && !jumping)
            {
                lanes[currentLane].shootingEnemyCount --;
                shifting = Random.Range(0,2);
                
                if(shifting == 0 && currentLane - 1 != gameManager.lowActiveLane)
                {
                    changeLane(shifting);
                }

                if(shifting == 1 && currentLane + 1 != gameManager.lowActiveLane)
                {
                    changeLane(shifting);
                }
                
                lanes[currentLane].shootingEnemyCount ++;
                StartCoroutine(changeDelay(changeDelayTime));   
            }
        }
        base.Update();

        //Enemy Count Update
        gameManager.currentLanes[currentLane].shootingEnemyCount ++;

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

        if(!gunContr.reloading)
        {
            anim.SetBool("shooting",true);
        }
        else
        {
            anim.SetBool("shooting",false);
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
