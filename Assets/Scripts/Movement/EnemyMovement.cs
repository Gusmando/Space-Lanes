using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Object Being Moved")]
    public GameObject subject;
    public Rigidbody subjectRb;
    public GameObject player;
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
    //An iterator keeping track of the current lane
    public int currentLane;
    //The rigid body of the object being controlled
    [Header("Animation Control")]
    public Animator anim;
    public bool animOver;
    public float laneChangeDelay;
    public bool leftRight;
    // Update is called once per frame

    void Start()
    {
        changed = false;
        currentLane = Random.Range(0,lanes.Length); 
        subjectRb = subject.GetComponent<Rigidbody>();
        animOver = true;
    }

    void Update()
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
    }
    void FixedUpdate() 
    {
        //Push and jump force set in inspector
        pushForce = new Vector3(0,0,10*speed);
        jumpForce = new Vector3(0,jumpMult,0);
        
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

        if(pushing)
        {
            //Accelerate as long as top speed is not hit
            if(subjectRb.velocity.z < maxSpeed)
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
    }

    public void changeLane(bool shifting)
    {
        if(!stopped)
        {
            if(!shifting && (currentLane - 1) >= 0)
            {
                currentLane --;
                changed = true;
                leftRight = false;
            }
            else if(shifting && (currentLane + 1) <= (lanes.Length-1))
            {
                currentLane ++;
                changed = true;
                leftRight = true;
            }
        }
    }

    public void stopMoving()
    {
        if(!stopped)
        {
            stopped = true;
        }
    }

    public void jumpOn()
    {
        if(!jumping && !stopped)
        {
            jumping = true;
            jump = true;
        }
    }

    public void push()
    {
        if(!pushing && !stopped)
        {
            pushing = true;
        }
    }

    //Essentially an onGround check
    private void OnCollisionEnter(Collision other)
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
    //To check for falls
    private void OnCollisionExit(Collision other)
    {
        if(!falling)
        {
            falling = true;
        }
    }

    //Aniamtion delay waits a few secondas for animation to finish
    private IEnumerator boolDelay(float delayLength, bool choice)
    {
        choice = false;

        yield return new WaitForSeconds(delayLength);

        choice = true;

        yield return null;
    }
}
