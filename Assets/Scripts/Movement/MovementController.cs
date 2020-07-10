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
    public int health;
    public int currentCount;
    public int jumpCount;
    public GameManager gameManager;
    public canvasMangager canvas;
    //Array holding different lanes within the level
    [Header("Lane Assignment (Left to Right)")]
    public Lane[] lanes;
    public Lane[] fullLanes;
    public float laneDiff;
    //The Force of gravity on the object
    //will have to be changed depending
    //on which lane is on
    [Header("Physic Scalars")]
    public float speed;
    public float speedBoost;
    public float maxSpeed;
    public float maxSpeedBoost;
    public float jumpMult;
    public float laneChangeSpeed;
    public float rayCastBelow;
    public float dubJumpTime;
    public float colliderDelayTime;
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
    public bool jumpInput;
    public bool falling;
    public bool changing;
    public bool hurt;
    public bool belowOpen;
    //An iterator keeping track of the current lane
    public int currentLane;
    public int laneTrack;
    public int currentLaneTrue;
    public  bool jumpQueue;
    public bool dubJump;
    //The rigid body of the object being controlled
    [Header("Animation Control")]
    public Animator anim;
    public bool animOver;
    public float laneChangeDelay;
    public bool leftRight;
    public int animStateDisp;
    public GunController weapon;
    public float hurtDelayTime;
    void Start() 
    {
       //Initial placement will be set in middle
       //will change this to be set in editor
       changed = false; 
       lanes = GameObject.FindWithTag("GameManager").GetComponent<GameManager>().currentLanes;
       currentLaneTrue = (fullLanes.Length/2);
       currentLane = (lanes.Length/2);
       laneTrack = 0;
       subjectRb = subject.GetComponent<Rigidbody>();
       animOver = true;
    }

    void Update()
    {
        //Display for the animation state for debug
        animStateDisp = anim.GetInteger("animState");
        if(weapon.currentGunIndex != 0)
        {
            canvas.limited = true;
        }
        else
        {
            canvas.limited = false;
        }
        canvas.currentColor = weapon.currentGun.gunColor;
        if(weapon.currentGun.clipCount!= 0)
        {
            currentCount = weapon.currentGun.clipSize + (weapon.currentGun.fullClip * (weapon.currentGun.clipCount));
        }
        else
        {
            currentCount = weapon.currentGun.clipSize;
        }
        canvas.ammoCountInt = currentCount;
        canvas.updateUIColors();
        canvas.currentHealth = health;
        canvas.updateUIHealth();
        canvas.dubJumpTime = this.dubJumpTime;
        canvas.dubJumpOn = this.dubJump;
        //If a left key press occurs and the left lane exists
        if(Input.GetKeyDown(KeyCode.A) && (currentLaneTrue - 1) >= 0)
        {
            currentLaneTrue--;
            laneTrack--;
            //Creates an animation delay so that animation plays clearly 
            StartCoroutine(animDelay(laneChangeDelay));
            //Setting to left move
            leftRight = false;;
            changed = true;
        }

        //If a right keypress occurs and the right lane exists
        if(Input.GetKeyDown(KeyCode.D) && (currentLaneTrue + 1) <= (fullLanes.Length-1))
        {
            currentLaneTrue++;
            laneTrack++;
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

        jumpInput = Input.GetKeyDown(KeyCode.Space);

        if(dubJumpTime <= 0)
        {
            dubJump = false;
        }
        else
        {
            dubJumpTime -= Time.deltaTime;
        }

        if(jumpInput)
        {
            jumpQueue = true;
        }

        if(jumpInput && !jumping && !falling || (jumpQueue && !jumping && !falling) || (jumpInput && dubJump && (jumping || falling) && jumpCount < 2))
        {
            jumping = true;
            falling = false;
            jumpCount ++;
            subjectRb.AddForce(subject.transform.up*jumpForce.y,ForceMode.Impulse);

            if(jumpQueue)
            {
                jumpQueue = false;
            }
        }
        //A lane change occurs and new positions are set
        if(changed || changing)
        {
            //Z position is the only nonchanging value as it show progress through each lane
            //also checks if current height is less than another lane
            if((!jumping && !falling) || (jumping && !falling) || (falling && fullLanes[currentLaneTrue].position.y - subject.transform.position.y < laneDiff))
            {
                newPosition = new Vector3(fullLanes[currentLaneTrue].position.x,fullLanes[currentLaneTrue].position.y,subject.transform.position.z);

            }

            else
            {
                newPosition = new Vector3(fullLanes[currentLaneTrue].position.x,subject.transform.position.y,subject.transform.position.z);
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
            subject.transform.rotation = Quaternion.RotateTowards(subject.transform.rotation,fullLanes[currentLaneTrue].rotation,step);
            subject.transform.position = Vector3.MoveTowards(subject.transform.position,newPosition,step);
        }
        else
        {
            changing = false;
            currentLane = laneTrack + (lanes.Length/2);
        }
    }

    void FixedUpdate() 
    {
        if(subjectRb.velocity.z < -.01 && !hurt)
        {
            StartCoroutine(hurtDelay(hurtDelayTime));
        }
        //Push and jump force set in inspector
        pushForce = new Vector3(0,0,10*speed*speedBoost);
        jumpForce = new Vector3(0,jumpMult,0);
        
        //Depending on the velocity, the run speed is set
        if(pushing && !jumping && !(subjectRb.velocity.z/maxSpeed<0))
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
            if(subjectRb.velocity.z < (maxSpeed * maxSpeedBoost))
            {
                subjectRb.AddForce(pushForce,ForceMode.Acceleration);   
            }

            else
            {
                //Ensures no infinite jump
                if(!jumping)
                {
                    Vector3 temp = subjectRb.velocity;
                    temp.z = maxSpeed * maxSpeedBoost;
                    subjectRb.velocity = temp;
                } 
            }
        }
        //The force of gravity is constantly acting on the object
        subjectRb.AddForce(subject.transform.up*gravityForce.y,ForceMode.Acceleration);

        if(subjectRb.velocity.y < -.05 && !falling)
        {
            falling = true;
        }
        else if(subjectRb.velocity.y > -.05 && falling)
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

        if(hurt)
        {
            anim.SetBool("hurt",true);
        }
        else
        {
            anim.SetBool("hurt",false);
        }
    }

    //Essentially an onGround check
    private void OnCollisionEnter(Collision other)
    {
        RaycastHit hit;
        Vector3 highObject = subject.transform.position + new Vector3(0,5,0);
        Vector3 below = subject.transform.position + new Vector3(0,rayCastBelow,0);
        Vector3 direction = below - highObject; 
        belowOpen = Physics.Raycast(highObject, direction,out hit);
        Debug.DrawRay(highObject, direction, Color.green);

        if(other.gameObject.tag.Contains("Floor") && belowOpen)
        {
            if(falling)
            {
                falling = false;
            }

            if(jumping)
            {
                jumping = false;
            }

            if(jumpCount !=0 )
            {
                jumpCount = 0;
            }
        }
        else if(!belowOpen && other.gameObject.tag.Contains("Floor") && subjectRb.velocity.y < -.05)
        {
            falling = true;
            subjectRb.GetComponent<Collider>().enabled = false;
            StartCoroutine(colliderDelay(colliderDelayTime));
        }

        if(other.gameObject.tag.Contains("Fast"))
        {
            speedBoost = 2;
            maxSpeedBoost = 1.5f;
        }
        else
        {
            speedBoost = 1;
            maxSpeedBoost = 1;
        }
    }
    //To check for falls
    private void OnCollisionExit(Collision other)
    {
        if(subjectRb.velocity.y < -.05)
        {
            falling = true;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
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

    private IEnumerator hurtDelay(float delayLength)
    {
        hurt = true;

        yield return new WaitForSeconds(delayLength);

        hurt = false;

        yield return null;
    }
    private IEnumerator colliderDelay(float delayLength)
    {
        yield return new WaitForSeconds(delayLength);
        subjectRb.GetComponent<Collider>().enabled = true;
    }
}
