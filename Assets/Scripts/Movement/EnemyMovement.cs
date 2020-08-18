using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The enemy movement class will handle a generalized movement
//pattern for all enemies, changing based on the inspector set
//variables and individual implementations
public class EnemyMovement : MonoBehaviour
{
    [Header("Assignments")]
    public GameObject subject;
    public Rigidbody subjectRb;
    public GameObject player;
    public GameManager gameManager;
    public Light spotLight;
    public Color white;
    public Color red;

    [Header("State Vars")]
    public int currentLane;
    public float health;
    public float distanceToPlayer;
    public bool changed;
    public bool pushing;
    public bool jumping;
    public bool falling;
    public bool stopped;
    public bool jump;
    public bool hurt;
    public bool changing;
    public bool noGap;
    public bool leftOpens;
    public bool rightOpens;
    public bool jumpable;
    
    [Header("Lane Assignment (Left to Right)")]
    public Lane[] lanes;
    public float laneDiff;
    
    [Header("Physic Scalars")]
    public float speed;
    public float maxSpeed;
    public float jumpMult;
    public float rayCastOffset;
    public float jumpCastOffset;
    public float laneChangeSpeed;
    public float leftRightOffset;
    
    [Header("Force Vectors")]
    public Vector3 gravityForce;
    private Vector3 pushForce;
    private Vector3 jumpForce;
    public Vector3 newPosition;
    
    [Header("Animation Control")]
    public Animator anim;
    public bool animOver;
    public float laneChangeDelay;
    public bool leftRight;
    public float hurtDelayTime;

    virtual public void Start()
    {
        //Finding and Assigning Objects
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player");

        //Initial Conditions
        lanes = gameManager.currentLanes;
        changed = true;
        subjectRb = subject.GetComponent<Rigidbody>();
        animOver = true;
    }

    virtual public void Update()
    {
        //Jump is set first and state vard (jumping, falling)
        //set accordingly
        if(jump && !jumping && !falling)
        {
            jumping = true;
            falling = false;
            subjectRb.velocity = new Vector3 (0,0,0);
            subjectRb.AddForce(jumpForce,ForceMode.Impulse);
            jump = false;
        }

        //If at some point the enemy lane is changed
        if(changed)
        {
            //If within a certain y range, then new position auto
            //sets to the y position of the next lane
            if((!jumping && !falling) || (jumping && !falling) || (falling && lanes[currentLane].position.y - subject.transform.position.y < laneDiff))
            {
                newPosition = new Vector3(lanes[currentLane].position.x,lanes[currentLane].position.y,subject.transform.position.z);
            }

            //Otherwise current y position is maintained
            else
            {
                newPosition = new Vector3(lanes[currentLane].position.x,subject.transform.position.y,subject.transform.position.z);
            }
            
            //Keeping movement in the y and z axis, x is frozen as no strafe movement
            subjectRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            changed = false;
            changing = true;
        }

        //If the enemy has not arrrived at the new position
        if(subject.transform.position != newPosition && changing)
        {
            //Calculating steps for the transition speed
            //and then moving the enmy to the new position
            //on that step formula
            float step = Time.deltaTime * laneChangeSpeed;
            subject.transform.rotation = Quaternion.RotateTowards(subject.transform.rotation,lanes[currentLane].rotation,step);
            subject.transform.position = Vector3.MoveTowards(subject.transform.position,newPosition,step);
        }
        else
        {
            changing = false;
        }

        //Enemy destroyed at 0 health
        if(health <= 0)
        {
            Destroy(subject);
        }
        
    }

    virtual protected void FixedUpdate() 
    {
        //Constantly checking distance to the player object
        //and recalculating the push and jump force of the player
        distanceToPlayer = Vector3.Distance(player.transform.position,subject.transform.position);
        pushForce = new Vector3(0,0,10*speed);
        jumpForce = new Vector3(0,jumpMult,0);

        //Falling when y velocity gets past the small threshold
        if(subjectRb.velocity.y < -.05)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }

        //Pushing behavior moves enemy towards the player 
        //in the -z direction
        if(pushing && !stopped)
        {
            //Raycast in front of enemy
            RaycastHit hit;
            Vector3 highObject = subject.transform.position + new Vector3(0,8,0);
            Vector3 inFront = subject.transform.position + new Vector3(0,0,rayCastOffset);
            Vector3 direction = inFront - highObject;

            //Raycast to the left of enemy
            RaycastHit leftHit;
            Vector3 leftVec = subject.transform.position + new Vector3(-1*leftRightOffset,0,0);
            Vector3 leftDirections = leftVec - highObject;

            //Raycast to the right of enemy
            RaycastHit rightHit;
            Vector3 rightVec = subject.transform.position + new Vector3(leftRightOffset,0,0);
            Vector3 rightDirections = rightVec - highObject;

            //Raycast in front of enemy (witthin jump range)
            RaycastHit jumpHit;
            Vector3 jumpVec = subject.transform.position + new Vector3(0,0,jumpCastOffset);
            Vector3 jumpDirections = jumpVec - highObject;
            
            //Setting booleans based on raycasts
            leftOpens = Physics.Raycast(highObject,leftDirections,out leftHit,10);
            rightOpens = Physics.Raycast(highObject,rightDirections,out rightHit,10);
            jumpable = Physics.Raycast(highObject,jumpDirections,out jumpHit,25);
            noGap = Physics.Raycast(highObject,direction,out hit,20);

            //When the left and right is open and the gap is not jumpable
            if(leftOpens && rightOpens && !noGap && !changing && !jumpable && !changing && !jumping)
            {
                //Change randomly right or left
                int changeDirect = Random.Range(0,2);
                if(changeDirect == 0 && (currentLane - 1) >= 0)
                {
                    changeLane(changeDirect);
                }
                else if(changeDirect == 1 && (currentLane + 1) <= (lanes.Length-1))
                {
                    changeLane(changeDirect);
                }
            }

            //If ONLY left is open and the gap is not jumpable
            else if(leftOpens && !noGap && !changing && !jumpable&& !changing && !jumping)
            {
                if((currentLane - 1) >= 0)
                {
                    changeLane(0);
                }
            }

            //If ONLY right is open and the gap is not jumpable
            else if(rightOpens && !noGap && !changing && !jumpable&& !changing && !jumping)
            {
                if((currentLane + 1) <= (lanes.Length-1))
                {
                    changeLane(1);
                }
            }

            //If the gap is jumpable
            else if(!noGap && !jump && !jumping && !falling && jumpable)
            {
                jump = true;
            }

            //Accelerate as long as top speed is not hit
            if(subjectRb.velocity.z > maxSpeed)
            {
                subjectRb.AddForce(pushForce,ForceMode.Acceleration);   
            }

            else
            {
                //Ensures no infinite jump
                if(!jumping)
                {
                    Vector3 temp = subjectRb.velocity;
                    temp.z = maxSpeed;
                    subjectRb.velocity = temp;
                } 
            }
        }

        //The force of gravity is constantly acting on the object
        subjectRb.AddForce(gravityForce,ForceMode.Acceleration);

        if(subjectRb.velocity.y < -.05)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }

    }

    //ChangeLane function can be called to set up
    //conditions which will cause a shift in lane based
    //on the integer passed in
    public void changeLane(int shifting)
    {
        if(!stopped)
        {
            //Left and Right Shifts
            if(shifting == 0 && (currentLane - 1) >= 0)
            {
                currentLane --;
                StartCoroutine(animDelay(laneChangeDelay));
                changed = true;
                leftRight = false;
            }
            else if(shifting == 1 && (currentLane + 1) <= (lanes.Length-1))
            {
                currentLane ++;
                StartCoroutine(animDelay(laneChangeDelay));
                changed = true;
                leftRight = true;
            }
        }
    }
    
    //This will be called by bullets to
    //initialize enemy hurt state
    public void hurtDelayStart()
    {
        StartCoroutine(hurtDelay(hurtDelayTime));
    }

    //Essentially an onGround check
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Contains("Floor"))
        {
            if(jumping)
            {
                jumping = false;
            }

            if(falling)
            {
                falling = false;
            }
        }
    }

    //Sets falling to true when leaving the floor
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag.Contains("Floor"))
        {
            if(!falling)
            {
                falling = true;
            }
        }
    }

    //Enums below used to create useful delays for state changes
    protected IEnumerator animDelay(float delayLength)
    {
        animOver = false;

        yield return new WaitForSeconds(delayLength);

        animOver = true;

        yield return null;
    }
    private IEnumerator hurtDelay(float delayLength)
    {
        hurt = true;

        yield return new WaitForSeconds(delayLength);

        hurt = false;

        yield return null;
    }
}
