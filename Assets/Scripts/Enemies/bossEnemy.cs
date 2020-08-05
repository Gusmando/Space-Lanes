using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEnemy : MonoBehaviour
{
    public GameObject lHand;
    public GameObject rHand;
    public GameObject head;

    public Lane[] lanes;
    public Lane[] lanes2;
    public Lane[] lanes3;

    public Vector3 ogPosL;
    public Vector3 ogPosR;

    public bool fleeing;
    public bool attacking;
    public bool deciding;
    public bool decided;
    public bool paused;
    public bool animating;
    public bool returning;

    public int compCount;
    public int shootingMode;

    public float decisionTime;
    public float attackTime;
    public float attackDelayTime;
    public float animDelayTime;

    private Animator lHandAnim;
    private Animator rHandAnim;
    private Animator headAnim; 

    public float circAngle;
    public float circSpeedMult;
    public float offsetLength;

    public float positionOffset;
    public float returnSpeed;
    private float circSpeed;

    private Vector3 newPositionLeft;
    private Vector3 newPositionRight;
    private Vector3 fleePosition;


    //Start is called before the first frame update
    void Start()
    {
        head.transform.position = new Vector3(lanes[lanes.Length/2].position.x,lanes[lanes.Length/2].position.y + positionOffset,head.transform.position.z);
        lHand.transform.position = new Vector3(lanes[lanes.Length/2 + 1].position.x,lanes[lanes.Length/2 + 1].position.y + positionOffset,lHand.transform.position.z);
        rHand.transform.position = new Vector3(lanes[lanes.Length/2 - 1].position.x,lanes[lanes.Length/2 - 1].position.y + positionOffset,rHand.transform.position.z);

        lHandAnim = lHand.GetComponent<Animator>();
        rHandAnim = rHand.GetComponent<Animator>();
        headAnim = head.GetComponent<Animator>();

        ogPosL = lHand.transform.position;
        ogPosR = rHand.transform.position;
        
        circSpeed = (2*Mathf.PI)/circSpeedMult;
        newPositionLeft = new Vector3(lanes[lanes.Length/2 + 1].position.x,lanes[lanes.Length/2 + 1].position.y + positionOffset,lHand.transform.position.z);
        newPositionRight = new Vector3(lanes[lanes.Length/2 - 1].position.x,lanes[lanes.Length/2 - 1].position.y + positionOffset,rHand.transform.position.z);
        
        compCount = 3;
    }

    //Update is called once per frame
    void Update()
    {   
        if(!fleeing && !deciding && decided && !attacking)
        {
            StartCoroutine(attackingDelay(attackDelayTime));
            paused = true;   
        }

        if(!deciding && !attacking && !fleeing && !paused)
        {
            StartCoroutine(decisionDuration(decisionTime));
            decided = true;            
        }

        if(deciding)
        {
            head.GetComponent<bossComponent>().vulnerable = false;
            lHand.GetComponent<bossComponent>().vulnerable = false;
            rHand.GetComponent<bossComponent>().vulnerable = false;
            lHandAnim.SetBool("shooting",false);
            rHandAnim.SetBool("shooting",false);

            if(!animating)
            {
                shootingMode = Random.Range(1,4);
                StartCoroutine(animationDelay(animDelayTime));
            }

            circAngle += circSpeed* Time.deltaTime;

            float tempXL = Mathf.Cos(circAngle)*offsetLength + ogPosL.x;
            float tempYL = Mathf.Sin(circAngle)*offsetLength + ogPosL.y;

            float tempXR = Mathf.Cos(-1*circAngle)*offsetLength + ogPosR.x;
            float tempYR = Mathf.Sin(-1*circAngle)*offsetLength + ogPosR.y;

            lHand.transform.position = new Vector3(tempXL,tempYL,lHand.transform.position.z);
            rHand.transform.position = new Vector3(tempXR,tempYR,rHand.transform.position.z);

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

        else { circAngle = 0;}
        
        if(!attacking && paused)
        {
            returning = true;
        }

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

        if(!deciding && !fleeing && attacking)
        {
            if(paused)
            {
                StartCoroutine(attackingDuration(attackTime));
            }

            if(paused)
            {
                paused = false;
            }

            if(decided)
            {
                decided = false;
            }

            switch (shootingMode) 
            {
                case 1:
                    lHandAnim.SetBool("shooting",false);
                    headAnim.SetInteger("animState",010);
                    rHandAnim.SetBool("shooting",true);
                    lHand.GetComponent<bossComponent>().vulnerable = false;
                    head.GetComponent<bossComponent>().vulnerable = false;
                    rHand.GetComponent<bossComponent>().vulnerable = true;
                    break;
                case 2: 
                    lHandAnim.SetBool("shooting",true);
                    headAnim.SetInteger("animState",000);
                    rHandAnim.SetBool("shooting",true);
                    lHand.GetComponent<bossComponent>().vulnerable = false;
                    head.GetComponent<bossComponent>().vulnerable = true;
                    rHand.GetComponent<bossComponent>().vulnerable = false;
                    break;
                case 3:
                    lHandAnim.SetBool("shooting",true);
                    headAnim.SetInteger("animState",010);
                    rHandAnim.SetBool("shooting",false); 
                    lHand.GetComponent<bossComponent>().vulnerable = true;
                    head.GetComponent<bossComponent>().vulnerable = false;
                    rHand.GetComponent<bossComponent>().vulnerable = false;
                    break;
                default:
                    break;
            }
        }

    }

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
}
