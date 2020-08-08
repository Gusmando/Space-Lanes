using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEnemy : MonoBehaviour
{
    [Header("Boss Components")]
    public GameObject lHand;
    public GameObject rHand;
    public GameObject head;

    [Header("Invisible Platforms")]
    public GameObject[] path1;
    public GameObject[] path2;
    public GameObject[] path3;

    [Header("Gun Controllers")]
    public GunController hGun;
    public GunController lGun;
    public GunController rGun;

    [Header("Gun Controllers")]
    public Lane[] lanes;
    public Vector3 ogPosL;
    public Vector3 ogPosR;

    [Header("Future Boss Positions")]
    public Transform stage2Trans;
    public Transform stage3Trans;

    [Header("State Variables")]
    public bool fleeing;
    public bool attacking;
    public bool deciding;
    public bool decided;
    public bool paused;
    public bool animating;
    public bool returning;
    public int shootingMode;

    [Header("Component Count")]
    public int compCount;

    [Header("Delay Time Settings")]
    public float decisionTime;
    public float attackTime;
    public float attackDelayTime;
    public float animDelayTime;

    [Header("Component Animators")]
    private Animator lHandAnim;
    private Animator rHandAnim;
    private Animator headAnim; 

    [Header("Circular Movement")]
    public float circAngle;
    public float circSpeedMult;
    public float offsetLength;
    
    [Header("Movement Speeds")]
    public float positionOffset;
    public float returnSpeed;
    public float fleeSpeed;
    public float handSpeed;

    //Variables below used to calculate position based on 
    //current state of boss
    private float circSpeed;
    private Vector3 newPositionLeft;
    private Vector3 newPositionRight;
    private Vector3 fleePosition;
    private Vector3 stage2;
    private Vector3 stage3;

    //Start is called before the first frame update
    void Start()
    {
        //Inital position of each component is based on three lane system, while offsetting the
        //y position to a more visible in-game placement.
        head.transform.position = new Vector3(lanes[lanes.Length/2].position.x,lanes[lanes.Length/2].position.y + positionOffset,head.transform.position.z);
        lHand.transform.position = new Vector3(lanes[lanes.Length/2 + 1].position.x,lanes[lanes.Length/2 + 1].position.y + positionOffset,lHand.transform.position.z);
        rHand.transform.position = new Vector3(lanes[lanes.Length/2 - 1].position.x,lanes[lanes.Length/2 - 1].position.y + positionOffset,rHand.transform.position.z);

        //Setting the animator component using the main Object
        lHandAnim = lHand.GetComponent<Animator>();
        rHandAnim = rHand.GetComponent<Animator>();
        headAnim = head.GetComponent<Animator>();

        //Saving the Original Position for later use when returning to neutral position
        ogPosL = lHand.transform.position;
        ogPosR = rHand.transform.position;
        
        circSpeed = (2*Mathf.PI)/circSpeedMult;

        //Setting these variables now, but changed constantly in runtime
        newPositionLeft = new Vector3(lanes[lanes.Length/2 + 1].position.x,lanes[lanes.Length/2 + 1].position.y + positionOffset,lHand.transform.position.z);
        newPositionRight = new Vector3(lanes[lanes.Length/2 - 1].position.x,lanes[lanes.Length/2 - 1].position.y + positionOffset,rHand.transform.position.z);

        //Setting Stage2 and Stage3 Vectors to the position variables
        stage2 = stage2Trans.position;
        stage3 = stage3Trans.position;
    }

    //Update is called once per frame
    void Update()
    {   
        //Based on current component count, different flee positions are set
        //and platforms are revealed after the death of each component
        switch (compCount)
        {
            //Complete Boss Death
            case 0:
                showPlat(path3);
                break;
            //Stage 3 -- 1 Component Left
            case 1:
                fleePosition = stage3;
                showPlat(path2);
                break;
            //Stage 2 -- 2 Components Left
            case 2:
                fleePosition = stage2;
                showPlat(path1);
                break;
            //Stage 1 -- Full Capacity
            case 3:
                break;
            default:
                break;
        }

        //While fleeing and not at the next stage position
        if(fleeing && transform.position != fleePosition)
        {
            //Calculating steps for movement of Boss
            float step = Time.deltaTime * fleeSpeed;
            float stepHand = Time.deltaTime * handSpeed;

            //Will be used as a target for flipping
            //the hands to 180 degrees
            Quaternion targetRot = new Quaternion();
            targetRot.eulerAngles = new Vector3(0,0,180);
            
            //Settings state variables to the fleeing setting
            //which constitutes a backwards head and rotated
            //hands; no shooting.
            if(head != null)
            {
                head.GetComponent<bossComponent>().vulnerable = false;
                headAnim.SetInteger("animState",110);
                hGun.input = false;
            }
            if(lHand != null)
            {
                lHand.GetComponent<bossComponent>().vulnerable = false;    
                lHandAnim.SetBool("shooting",false);
                lHand.transform.rotation = Quaternion.RotateTowards(lHand.transform.rotation,targetRot,stepHand);
                lGun.input = false;
            }
            if(rHand != null)
            {
                rHand.GetComponent<bossComponent>().vulnerable = false;
                rHandAnim.SetBool("shooting",false);
                rHand.transform.rotation = Quaternion.RotateTowards(rHand.transform.rotation,targetRot,stepHand);
                rGun.input = false;
            }

            //Inching the entire Object to the new position baserd on step calculation
            transform.position = Vector3.MoveTowards(transform.position,fleePosition,step);
        }

        //If not fleeing and at stage position
        else
        {
            fleeing = false;

            //Setting the hands to original rotation
            if(lHand != null)
            {
                lHand.transform.eulerAngles = new Vector3(0,0,0);
            }
            if(rHand != null)
            {
                rHand.transform.eulerAngles = new Vector3(0,0,0);
            }
        }

        //If the next shooting pattern has been <decided>
        //then the attack delay will start giving the player
        //time to react
        if(!fleeing && !deciding && decided && !attacking)
        {
            StartCoroutine(attackingDelay(attackDelayTime));
            paused = true;   
        }

        //If the next shooting pattern has not been determined,
        //then the decision coroutines will begin
        if(!deciding && !attacking && !fleeing && !paused)
        {
            StartCoroutine(decisionDuration(decisionTime));
            decided = true;            
        }

        if(deciding)
        {
            //The angle of the circular motion will be constantly increasing
            circAngle += circSpeed* Time.deltaTime;
            
            //Setting the left and right hand x and y positions using
            //using circular motion formulas
            float tempXL = Mathf.Cos(circAngle)*offsetLength + ogPosL.x;
            float tempYL = Mathf.Sin(circAngle)*offsetLength + ogPosL.y;

            float tempXR = Mathf.Cos(-1*circAngle)*offsetLength + ogPosR.x;
            float tempYR = Mathf.Sin(-1*circAngle)*offsetLength + ogPosR.y;

            //Setting the individual state variables and shooting to off
            //during the decision state fo each component
            if(head != null)
            {
                head.GetComponent<bossComponent>().vulnerable = false;
                hGun.input = false;
            }
            
            if(lHand != null)
            {
                lHand.GetComponent<bossComponent>().vulnerable = false;
                lHandAnim.SetBool("shooting",false);
                lHand.transform.position = new Vector3(tempXL,tempYL,lHand.transform.position.z);
                lGun.input = false;
            }

            if(rHand != null)
            {
                rHand.GetComponent<bossComponent>().vulnerable = false;
                rHandAnim.SetBool("shooting",false);
                rHand.transform.position = new Vector3(tempXR,tempYR,rHand.transform.position.z);
                rGun.input = false;
            }
            
            //Cycling through shooting options with a coroutine
            //delay set up on animation start
            if(!animating)
            {
                shootingMode = Random.Range(1,4);
                StartCoroutine(animationDelay(animDelayTime));
            }
            
            //The head position animation will change based on the current
            //random shooting mode state
            if(head != null)
            {
                switch (shootingMode) 
                {
                    case 1:
                        headAnim.SetInteger("animState",100);
                        break;
                    case 2: 
                        headAnim.SetInteger("animState",000);
                        break;
                    case 3:
                        headAnim.SetInteger("animState",001); 
                        break;
                    default:
                        break;
                }
            }
        }

        else { circAngle = 0;}
        
        //Fluidly returning hands to normal position
        //after decision routines 
        if(!attacking && paused)
        {
            returning = true;
        }

        //Moving the left and right hand based on the step
        //calculation in order to return from circular motion loop
        //Similar logic to the fleeing behavior
        if(lHand != null)
        {
            if(lHand.transform.position != newPositionLeft && returning)
            {
                float step = Time.deltaTime * returnSpeed;
                lHand.transform.rotation = Quaternion.RotateTowards(lHand.transform.rotation,lanes[lanes.Length/2 + 1].rotation,step);
                lHand.transform.position = Vector3.MoveTowards(lHand.transform.position,newPositionLeft,step);
            }
            else
            {
                returning = false;
            }
        }

        if(rHand != null)
        {
            if(rHand.transform.position != newPositionRight && returning)
            {
                float step = Time.deltaTime * returnSpeed;
                rHand.transform.rotation = Quaternion.RotateTowards(rHand.transform.rotation,lanes[lanes.Length/2 - 1].rotation,step);
                rHand.transform.position = Vector3.MoveTowards(rHand.transform.position,newPositionRight,step);
            }
            else
            {
                returning = false;
            }
        }

        //The attacking state is defined below, changing the
        //animations of each component and their shooting modes
        if(!deciding && !fleeing && attacking)
        {
            //Begins the coroutine which keeps attack state active
            if(paused)
            {
                StartCoroutine(attackingDuration(attackTime));
            }

            //These variables must be reset before the next behavior loop
            if(paused)
            {
                paused = false;
            }

            if(decided)
            {
                decided = false;
            }

            //This switch statement wll change the hands and head
            //to their appropriate shooting states based on the
            //previously determined shooting mode
            switch (shootingMode) 
            {
                //Left Hand is vulnerable, other comps shoot
                case 1:
                    if(lHand != null)
                    {
                        lHandAnim.SetBool("shooting",false);
                        lHand.GetComponent<bossComponent>().vulnerable = true;
                        lGun.input = false;
                    }

                    if(rHand != null)
                    {
                        rHandAnim.SetBool("shooting",true);
                        rHand.GetComponent<bossComponent>().vulnerable = false;
                        rGun.input = true;
                    }

                    if(head != null)
                    {
                        headAnim.SetInteger("animState",010);
                        head.GetComponent<bossComponent>().vulnerable = false;
                        hGun.input = true;
                    }
                    break;
                //Head is vulnerable, other comps shoot
                case 2:
                    if(lHand != null)
                    {
                        lHandAnim.SetBool("shooting",true);
                        lHand.GetComponent<bossComponent>().vulnerable = false;
                        lGun.input = true;
                    }

                    if(rHand != null)
                    {
                        rHandAnim.SetBool("shooting",true);
                        rHand.GetComponent<bossComponent>().vulnerable = false;
                        rGun.input = true;
                    }

                    if(head != null)
                    {
                        headAnim.SetInteger("animState",000);
                        head.GetComponent<bossComponent>().vulnerable = true;
                        hGun.input = false;
                    }
                    break;
                //Right hand is vulnerable, other comps shoot
                case 3:
                    if(lHand != null)
                    {
                        lHandAnim.SetBool("shooting",true);
                        lHand.GetComponent<bossComponent>().vulnerable = false;
                        lGun.input = true;
                    }

                    if(rHand != null)
                    {
                        rHandAnim.SetBool("shooting",false);
                        rHand.GetComponent<bossComponent>().vulnerable = true;
                        rGun.input = false;
                    }

                    if(head != null)
                    {
                        headAnim.SetInteger("animState",010);
                        head.GetComponent<bossComponent>().vulnerable = false;
                        hGun.input = true;
                    }
                    break;
                default:
                    break;
            }
        }

    }

    //Below are various delays which can be called as coroutines
    //to ensure that the boss ai is constantly cycling between
    //states 
    private IEnumerator decisionDuration(float delayLength)
    {
        deciding = true;

        yield return new WaitForSeconds(delayLength);

        deciding = false;

        yield return null;
    }

    private IEnumerator attackingDuration(float delayLength)
    {
        attacking = true;

        yield return new WaitForSeconds(delayLength);

        attacking = false;

        yield return null;
    }

    private IEnumerator animationDelay(float delayLength)
    {
        animating = true;

        yield return new WaitForSeconds(delayLength);

        animating = false;

        yield return null;
    }
    private IEnumerator attackingDelay(float delayLength)
    {
        attacking = false;

        yield return new WaitForSeconds(delayLength);

        attacking = true;

        yield return null;
    }

    private void showPlat(GameObject[] hiddenPlats)
    {
        foreach (GameObject x in hiddenPlats)
        {
            x.SetActive(true);
        }
    }
}
