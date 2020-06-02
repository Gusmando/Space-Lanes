using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] sections;
    public Lane[] currentLanes;
    public  int lowActiveLane;
    public bool  laneChange;
    public bool spawning;
    public bool activeSectionChanged;
    public float laneDelayTime;
    public float spawnOffset;
    public int sectionsCleared;
    public int activeSection;
    public int maxSections;
    public GameObject spawned;
    public GameObject initSpawnPoint;
    public GameObject endSpawn;


    void Start()
    {
        spawning = true;
        Physics.IgnoreLayerCollision(9,9,true);
        activeSection = Random.Range(0,sections.Length);
        spawned = Instantiate(sections[activeSection],initSpawnPoint.transform.position,initSpawnPoint.transform.rotation);
        spawned.tag = "activeSection";
        currentLanes = GameObject.FindWithTag("activeSection").GetComponent<Section>().lanes;
        spawned.GetComponent<Section>().sectionActive = true;
        spawned.GetComponent<Section>().activateSpawner();
        this.endSpawn = spawned.GetComponent<Section>().endSpawn;
        
    }
    void Update()
    {
        if(spawning)
        {
            for(int i = 0; i < maxSections - 1; i++)
            {
                activeSection = Random.Range(0,sections.Length);
                Vector3 newEndSpawnPoint = endSpawn.transform.position + new Vector3 (0,0,spawnOffset);
                spawned = Instantiate(sections[activeSection],newEndSpawnPoint,initSpawnPoint.transform.rotation);
                this.endSpawn = spawned.GetComponent<Section>().endSpawn;
            }
            spawning = false;
        }
        
        if(laneChange)
        {   
            lowActiveLane = Random.Range(0,currentLanes.Length);
            StartCoroutine(activeLaneDelay(laneDelayTime));
        }

        if(activeSectionChanged)
        {

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
