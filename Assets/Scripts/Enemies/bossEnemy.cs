using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEnemy : MonoBehaviour
{
    public GameObject lHand;
    public GameObject rHand;
    public GameObject head;
    public Lane[] lanes;
    public bool fleeing;
    public bool attacking;
    public bool deciding;
    public int leftHandHp;
    public int headHp;
    public int rightHandHp;
    public int shootingMode;
    public float decisionTime;

    private Animator lHandAnim;
    private Animator rHandAnim;
    private Animator headAnim; 

    //Start is called before the first frame update
    void Start()
    {
        head.transform.position = lanes[lanes.Length/2].position;
        lHand.transform.position = lanes[(lanes.Length/2) - 1].position;
        rHand.transform.position = lanes[(lanes.Length/2) + 1].position; 
        lHandAnim = lHand.GetComponent<Animator>();
        rHandAnim = rHand.GetComponent<Animator>();
        headAnim = head.GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update()
    {
        if(!deciding && ! attacking)
        {
            StartCoroutine(decisionDelay(decisionTime));            
        }

        if(!fleeing && !deciding)
        {
            attacking = true;
        }

        if(deciding)
        {
            shootingMode = Random.Range(1,4);
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
        
        if(!deciding && attacking)
        {
            switch (shootingMode) 
            {
                case 1:
                    lHandAnim.SetBool("shooting",true);
                    headAnim.SetInteger("animState",010);
                    rHandAnim.SetBool("shooting",false);
                    break;
                case 2: 
                    lHandAnim.SetBool("shooting",true);
                    headAnim.SetInteger("animState",000);
                    rHandAnim.SetBool("shooting",true);
                    break;
                case 3:
                    lHandAnim.SetBool("shooting",false);
                    headAnim.SetInteger("animState",010);
                    rHandAnim.SetBool("shooting",true); 
                    break;
                default:
                    break;
            }
        }
        



    }

    private IEnumerator decisionDelay(float delayLength)
    {
        deciding = true;

        yield return new WaitForSeconds(delayLength);

        deciding = false;

        yield return null;
    }
}
