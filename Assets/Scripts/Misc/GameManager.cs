using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] sections;
    public Lane[] currentLanes;
    public  int lowActiveLane;
    public bool  laneChange;
    public bool activeSectionChanged;
    public float laneDelayTime;
    public int sectionsCleared;
    public int activeSection;
    public GameObject spawned;
    public GameObject initSpawnPoint;


    void Start()
    {
        Physics.IgnoreLayerCollision(9,9,true);
        activeSection = Random.Range(0,sections.Length);
        spawned = Instantiate(sections[activeSection],initSpawnPoint.transform.position,initSpawnPoint.transform.rotation);
        spawned.tag = "activeSection";
        currentLanes = GameObject.FindWithTag("activeSection").GetComponent<Section>().lanes;
        spawned.GetComponent<Section>().sectionActive = true;
        spawned.GetComponent<Section>().activateSpawner();
        
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
