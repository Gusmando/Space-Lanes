using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEnemy : MonoBehaviour
{
    public GameObject lHand;
    public GameObject rHand;
    public GameObject head;

    public GameObject[] path1;
    public GameObject[] path2;
    public GameObject[] path3;

    public Lane[] lanes;
    public Vector3 ogPosL;
    public Vector3 ogPosR;

    public Transform stage2Trans;
    public Transform stage3Trans;

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
    public float fleeSpeed;
    public float handSpeed;
    private float circSpeed;

    private Vector3 newPositionLeft;
    private Vector3 newPositionRight;
    private Vector3 fleePosition;
    public Vector3 stage2;
    public Vector3 stage3;

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

        stage2 = stage2Trans.position;
        stage3 = stage3Trans.position;
    }

    //Update is called once per frame
    void Update()
    {   
        switch (compCount)
        {
            case 0:
                break;
            case 1:
                fleePosition = stage3;
                showPlat(path2);
                break;
            case 2:
                fleePosition = stage2;
                showPlat(path1);
                break;
            case 3:
                break;
            default:
                break;
        }

        if(fleeing && transform.position != fleePosition)
        {
            float step = Time.deltaTime * fleeSpeed;
            float stepHand = Time.deltaTime * handSpeed;
            Quaternion targetRot = new Quaternion();
            targetRot.eulerAngles = new Vector3(0,0,180);

            if(head != null)
            {
                head.GetComponent<bossComponent>().vulnerable = false;
                headAnim.SetInteger("animState",110);
            }
            if(lHand != null)
            {
                lHand.GetComponent<bossComponent>().vulnerable = false;    
                lHandAnim.SetBool("shooting",false);
                lHand.transform.rotation = Quaternion.RotateTowards(lHand.transform.rotation,targetRot,stepHand);
            }
            if(rHand != null)
            {
                rHand.GetComponent<bossComponent>().vulnerable = false;
                rHandAnim.SetBool("shooting",false);
                rHand.transform.rotation = Quaternion.RotateTowards(rHand.transform.rotation,targetRot,stepHand);
            }

            transform.position = Vector3.MoveTowards(transform.position,fleePosition,step);
        }

        else
        {
            fleeing = false;
            if(lHand != null)
            {
                lHand.transform.eulerAngles = new Vector3(0,0,0);
            }
            if(rHand != null)
            {
                rHand.transform.eulerAngles = new Vector3(0,0,0);
            }
        }

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
            circAngle += circSpeed* Time.deltaTime;

            float tempXL = Mathf.Cos(circAngle)*offsetLength + ogPosL.x;
            float tempYL = Mathf.Sin(circAngle)*offsetLength + ogPosL.y;

            float tempXR = Mathf.Cos(-1*circAngle)*offsetLength + ogPosR.x;
            float tempYR = Mathf.Sin(-1*circAngle)*offsetLength + ogPosR.y;

            if(head != null)
            {
                head.GetComponent<bossComponent>().vulnerable = false;
            }

            if(lHand != null)
            {
                lHand.GetComponent<bossComponent>().vulnerable = false;
                lHandAnim.SetBool("shooting",false);
                lHand.transform.position = new Vector3(tempXL,tempYL,lHand.transform.position.z);
            }

            if(rHand != null)
            {
                rHand.GetComponent<bossComponent>().vulnerable = false;
                rHandAnim.SetBool("shooting",false);
                rHand.transform.position = new Vector3(tempXR,tempYR,rHand.transform.position.z);
            }
            
            if(!animating)
            {
                shootingMode = Random.Range(1,4);
                StartCoroutine(animationDelay(animDelayTime));
            }
            
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
        
        if(!attacking && paused)
        {
            returning = true;
        }

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
                    if(lHand != null)
                    {
                        lHandAnim.SetBool("shooting",false);
                        lHand.GetComponent<bossComponent>().vulnerable = true;
                    }

                    if(rHand != null)
                    {
                        rHandAnim.SetBool("shooting",true);
                        rHand.GetComponent<bossComponent>().vulnerable = false;
                    }

                    if(head != null)
                    {
                        headAnim.SetInteger("animState",010);
                        head.GetComponent<bossComponent>().vulnerable = false;
                    }
                    break;

                case 2:
                    if(lHand != null)
                    {
                        lHandAnim.SetBool("shooting",true);
                        lHand.GetComponent<bossComponent>().vulnerable = false;
                    }

                    if(rHand != null)
                    {
                        rHandAnim.SetBool("shooting",true);
                        rHand.GetComponent<bossComponent>().vulnerable = false;
                    }

                    if(head != null)
                    {
                        headAnim.SetInteger("animState",000);
                        head.GetComponent<bossComponent>().vulnerable = true;
                    }
                    break;

                case 3:
                    if(lHand != null)
                    {
                        lHandAnim.SetBool("shooting",true);
                        lHand.GetComponent<bossComponent>().vulnerable = false;
                    }

                    if(rHand != null)
                    {
                        rHandAnim.SetBool("shooting",false);
                        rHand.GetComponent<bossComponent>().vulnerable = true;
                    }

                    if(head != null)
                    {
                        headAnim.SetInteger("animState",010);
                        head.GetComponent<bossComponent>().vulnerable = false;
                    }
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

    private void showPlat(GameObject[] hiddenPlats)
    {
        foreach (GameObject x in hiddenPlats)
        {
            x.SetActive(true);
        }
    }
}
