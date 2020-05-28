using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Lane[] lanes;
    public  int lowActiveLane;
    public bool  laneChange;
    public float laneDelayTime;

    void Awake()
    {
        Physics.IgnoreLayerCollision(9,9,true);
    }

    
    void Update()
    {

        if(laneChange)
        {   
            lowActiveLane = Random.Range(0,lanes.Length);
            StartCoroutine(activeLaneDelay(laneDelayTime));
        }
        
    }

    public IEnumerator activeLaneDelay(float delayLength)
    {
        laneChange = false;

        yield return new WaitForSeconds(delayLength);

        laneChange = true;

        yield return null;

    }
}
