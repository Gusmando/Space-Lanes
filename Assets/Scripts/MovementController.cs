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
    //Array holding different lanes within the level
    [Header("Lane Assignment (Left to Right)")]
    public Lane[] lanes;
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
    //An iterator keeping track of the current lane
    public int currentLane;
    //The rigid body of the object being controlled
    [Header("Animation Control")]
    public Animator anim;
    public bool animOver;
    public float laneChangeDelay;
    public bool leftRight;
    public int animStateDisp;
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
        animStateDisp = anim.GetInteger("animState");

        if(pushing && !jumping)
        {
            anim.speed = (subjectRb.velocity.z/maxSpeed)*.3f;
        }
        else
        {
            if(!jumping)
            {
                subjectRb.velocity = new Vector3(0,0,0);
                anim.speed = 0;
            } 
        }
        //The force of gravity is constantly acting on the object
        subjectRb.AddForce(gravityForce);
        
        pushForce = new Vector3(0,0,10*speed);
        jumpForce = new Vector3(0,jumpMult,0);
        //If a left key press occurs and the left lane exists
        if(Input.GetKeyDown(KeyCode.LeftArrow) && (currentLane - 1) >= 0)
        {
            currentLane--;
            StartCoroutine(animDelay(laneChangeDelay));
            leftRight = false;;
            changed = true;
        }

        //If a right keypress occurs and the right lane exists
        if(Input.GetKeyDown(KeyCode.RightArrow) && (currentLane + 1) <= (lanes.Length-1))
        {
            currentLane++;
            leftRight = true;
            StartCoroutine(animDelay(laneChangeDelay));
            changed = true;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            pushing = true;
            if(subjectRb.velocity.z < maxSpeed)
            {
                subjectRb.AddForce(pushForce,ForceMode.Acceleration);   
            }

            else
            {
                subjectRb.velocity = new Vector3(0,0,maxSpeed);
            }        
        }

        else
        {
            pushing = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            jumping = true;
            falling = false;
            subjectRb.AddForce(jumpForce,ForceMode.Impulse);
        }

        //A lane change occurs and new positions are set
        if(changed)
        {
            Vector3 newPosition = new Vector3(0f,0f,0f); 
            //Z position is the only nonchanging value as it show progress through each lane
            if(!jumping)
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
        else
        {
            if(subjectRb.velocity.y > 0)
            {
                anim.SetInteger("animState",111);
            }
            else if(subjectRb.velocity.y < 0)
            {
                anim.SetInteger("animState",100);
            }
            else if(subjectRb.velocity.y == 0)
            {
                anim.SetInteger("animState",0);
            }
            
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(jumping)
        {
            jumping = false;
        }
    }

    private IEnumerator animDelay(float delayLength)
    {
        animOver = false;

        yield return new WaitForSeconds(delayLength);

        animOver = true;

        yield return null;
    }
}
