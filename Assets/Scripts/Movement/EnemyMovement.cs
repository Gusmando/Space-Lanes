using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Object Being Moved")]
    public GameObject subject;
    public Rigidbody subjectRb;
    public float health;
    public GameObject player;
    public float distanceToPlayer;
    //Array holding different lanes within the level
    [Header("Lane Assignment (Left to Right)")]
    public Lane[] lanes;
    public float laneDiff;
    //The Force of gravity on the object
    //will have to be changed depending
    //on which lane is on
    [Header("Physic Scalars")]
    public float speed;
    public float maxSpeed;
    public float jumpMult;
    public float rayCastOffset;
    [Header("Force Vectors")]
    public Vector3 gravityForce;
    private Vector3 pushForce;
    private Vector3 jumpForce;
    //Whether on not a change has occured
    [Header("State Vars")]
    public bool changed;
    public bool pushing;
    public bool jumping;
    public bool falling;
    public bool stopped;
    public bool idle;
    public bool jump;
    public bool choice;
    //An iterator keeping track of the current lane
    public int currentLane;
    //The rigid body of the object being controlled
    [Header("Animation Control")]
    public Animator anim;
    public bool animOver;
    public float laneChangeDelay;
    public bool leftRight;
    // Update is called once per frame

    virtual public void Start()
    {
        changed = true;
        currentLane = Random.Range(0,lanes.Length); 
        subjectRb = subject.GetComponent<Rigidbody>();
        animOver = true;
    }

    virtual public void Update()
    {
        if(jump && !jumping && !falling)
        {
            jumping = true;
            falling = false;
            
            subjectRb.velocity = new Vector3 (0,0,0);
            subjectRb.AddForce(jumpForce,ForceMode.Impulse);
            jump = false;
        }

        if(changed)
        {
            Vector3 newPosition = new Vector3(0f,0f,0f); 

            //Z position is the only nonchanging value as it show progress through each lane
            //also checks if current height is less than another lane
            if((!jumping && !falling) || (jumping && !falling) || (falling && lanes[currentLane].position.y - subject.transform.position.y < laneDiff))
            {
                newPosition = new Vector3(lanes[currentLane].position.x,lanes[currentLane].position.y,subject.transform.position.z);
            }

            else
            {
                newPosition = new Vector3(lanes[currentLane].position.x,subject.transform.position.y,subject.transform.position.z);
            }

            //Setting the new position and rotation of the object
            subject.transform.position = newPosition;
            subject.transform.eulerAngles = lanes[currentLane].rotation;
            
            //Keeping movement in the y and z axis, x is frozen as no strafe movement
            subjectRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            changed = false;
        }

        if(health <= 0)
        {
            Destroy(subject);
        }
    }
    virtual protected void FixedUpdate() 
    {
        distanceToPlayer = Vector3.Distance(player.transform.position,subject.transform.position);
        //Push and jump force set in inspector
        pushForce = new Vector3(0,0,10*speed);
        jumpForce = new Vector3(0,jumpMult,0);

        if(subjectRb.velocity.y < -.05)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }
        
        //Depending on the velocity, the run speed is set
        if(pushing && !jumping)
        {
            //anim.speed = (subjectRb.velocity.z/maxSpeed)*.3f;
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
                //anim.speed = 0;
            } 
        }

        if(pushing && !stopped)
        {
            RaycastHit hit;
            Vector3 highObject = subject.transform.position + new Vector3(0,5,0);
            Vector3 inFront = subject.transform.position + new Vector3(0,lanes[currentLane].position.y,rayCastOffset);
            Vector3 direction = inFront - highObject;
            Debug.DrawRay(highObject, direction, Color.green);

            if(!(Physics.Raycast(highObject,direction,out hit)) && !jump && !jumping && !falling)
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

    public void changeLane(int shifting)
    {
        if(!stopped)
        {
            if(shifting == 0 && (currentLane - 1) >= 0)
            {
                currentLane --;
                changed = true;
                leftRight = false;
            }
            else if(shifting == 1 && (currentLane + 1) <= (lanes.Length-1))
            {
                currentLane ++;
                changed = true;
                leftRight = true;
            }
        }
    }

    //Essentially an onGround check
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Floor"))
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
    //To check for falls
    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Floor"))
        {
            if(!falling)
            {
                falling = true;
            }
        }
    }

    //Aniamtion delay waits a few secondas for animation to finish
    protected IEnumerator animDelay(float delayLength)
    {
        animOver = false;

        yield return new WaitForSeconds(delayLength);

        animOver = true;

        yield return null;
    }
}
