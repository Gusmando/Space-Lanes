using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class will be used as a movement controller
//for player objects, changing its position as changes 
//between lanes occur
public class MovementController : MonoBehaviour
{
    [Header("Object Being Moved")]
    public GameObject subject;
    public Rigidbody subjectRb;
    public float health;
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
    public float laneChangeSpeed;
    [Header("Force Vectors")]
    public Vector3 gravityForce;
    private Vector3 pushForce;
    private Vector3 jumpForce;

    public Vector3 newPosition;
    //Whether on not a change has occured
    [Header("State Vars")]
    public bool changed;
    public bool pushing;
    public bool jumping;
    public bool falling;
    public bool changing;
    //An iterator keeping track of the current lane
    public int currentLane;
    public  bool jumpQueue;
    //The rigid body of the object being controlled
    [Header("Animation Control")]
    public Animator anim;
    public bool animOver;
    public float laneChangeDelay;
    public bool leftRight;
    public int animStateDisp;
    public GunController weapon;
    void Start() 
    {
       //Initial placement will be set in middle
       //will change this to be set in editor
       changed = false; 
       currentLane = (lanes.Length/2);
       subjectRb = subject.GetComponent<Rigidbody>();
       animOver = true;
    }

    void Update()
    {
        //Display for the animation state for debug
        animStateDisp = anim.GetInteger("animState");
        //If a left key press occurs and the left lane exists
        if(Input.GetKeyDown(KeyCode.A) && (currentLane - 1) >= 0)
        {
            currentLane--;
            //Creates an animation delay so that animation plays clearly 
            StartCoroutine(animDelay(laneChangeDelay));
            //Setting to left move
            leftRight = false;;
            changed = true;
        }

        //If a right keypress occurs and the right lane exists
        if(Input.GetKeyDown(KeyCode.D) && (currentLane + 1) <= (lanes.Length-1))
        {
            currentLane++;
            leftRight = true;
            StartCoroutine(animDelay(laneChangeDelay));
            changed = true;
        }

        //This means that the player is pushing forward
        if(Input.GetKey(KeyCode.W))
        {
            pushing = true;     
        }
        //If button isn't held, then not pushing 
        else
        {
            pushing = false;
        }
        
        //This will handle a space or jump push
        if(Input.GetKeyDown(KeyCode.Space) && !jumping && !falling || jumpQueue && !jumping && !falling)
        {
            jumping = true;
            falling = false;

            //Stops the object before the jump happens
            //Helps to make jump feel bouncy and respoonsive
            subjectRb.velocity = new Vector3 (0,0,0);
            subjectRb.AddForce(jumpForce,ForceMode.Impulse);

            if(jumpQueue)
            {
                jumpQueue = false;
            }
        }
        if((Input.GetKeyDown(KeyCode.Space)) && changing)
        {
            jumpQueue = true;
        }
        //A lane change occurs and new positions are set
        if(changed || changing)
        {
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

            //subject.transform.position = newPosition;
            //subject.transform.eulerAngles = lanes[currentLane].rotation;
            
            //Keeping movement in the y and z axis, x is frozen as no strafe movement
            subjectRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            changed = false;
            changing = true;
        }

        if(subject.transform.position != newPosition && changing)
        {
            float step = Time.deltaTime * laneChangeSpeed;
            subject.transform.rotation = Quaternion.RotateTowards(subject.transform.rotation,lanes[currentLane].rotation,step);
            subject.transform.position = Vector3.MoveTowards(subject.transform.position,newPosition,step);
        }
        else
        {
            changing = false;
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
            anim.speed = (subjectRb.velocity.z/maxSpeed)*.3f;
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
        else
        {
            falling = false;
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

        if(weapon.shotAnim)
        {
            anim.SetBool("shooting",true);
        }
        else
        {
            anim.SetBool("shooting",false);
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
    private IEnumerator animDelay(float delayLength)
    {
        animOver = false;

        yield return new WaitForSeconds(delayLength);

        animOver = true;

        yield return null;
    }
}
