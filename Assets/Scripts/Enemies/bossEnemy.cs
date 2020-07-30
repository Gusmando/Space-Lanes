using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEnemy : MonoBehaviour
{
    public GameObject lHand;
    public GameObject rHand;
    public GameObject head;
    public Lane[] lanes;
    
    public bool leftShoot;
    public bool headShoot;
    public bool rightShoot;
    public bool fleeing;
    public bool attacking;
    public bool deciding;

    public int leftHandHp;
    public int headHp;
    public int rightHandHp;
    public int shootingMode;
    public float decisionTime; 

    //Start is called before the first frame update
    void Start()
    {
        head.transform.position = lanes[lanes.Length/2].position;
        lHand.transform.position = lanes[(lanes.Length/2) - 1].position;
        rHand.transform.position = lanes[(lanes.Length/2) + 1].position; 
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
            shootingMode = Random.Range(1,3);
        }
        
        switch (shootingMode) 
        {
            case 1:
                break;
            case 2: 
                break;
            case 3: 
                break;
            case 4:
                break;
            case 5: 
                break;
            case 6: 
                break;
            case 7: 
                break;
            case 8: 
                break;
            default:
                break;
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
