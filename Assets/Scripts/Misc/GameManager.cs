using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Section[] sections;
    public Lane[] currentLanes;
    public  int lowActiveLane;
    public bool  laneChange;
    public float laneDelayTime;
    public int sectionsCleared;
    public int activeSection;

    void Awake()
    {

        Physics.IgnoreLayerCollision(9,9,true);
        activeSection = Random.Range(0,sections.Length);
        currentLanes = sections[activeSection].lanes;
        sections[activeSection].sectionActive = true;
    }
    void Update()
    {
        if(laneChange)
        {   
            lowActiveLane = Random.Range(0,currentLanes.Length);
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
