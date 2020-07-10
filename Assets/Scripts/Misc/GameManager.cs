using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] sections;
    public Lane[] currentLanes;
    public Lane[] largestLanes;
    public  int lowActiveLane;
    public bool  laneChange;
    public bool spawning;
    public bool activeSectionChanged;
    public float laneDelayTime;
    public float spawnOffset;
    public int sectionsCleared;
    public int activeSection;
    public int maxSections;
    public int totalMinor;
    public int totalShooting;
    public int totalLob;
    public GameObject spawned;
    public GameObject initSpawnPoint;
    public GameObject endSpawn;
    public MovementController player;
    public Section currentActive;


    void Start()
    {
        spawning = true;
        Physics.IgnoreLayerCollision(9,9,true);
        Physics.IgnoreLayerCollision(8,8,true);
        activeSection = Random.Range(0,sections.Length);
        spawned = Instantiate(sections[activeSection],initSpawnPoint.transform.position,initSpawnPoint.transform.rotation);
        spawned.tag = "activeSection";
        currentActive = GameObject.FindWithTag("activeSection").GetComponent<Section>();
        currentLanes = currentActive.lanes;
        largestLanes = currentLanes;
        currentActive.sectionActive = true;
        currentActive.activateSpawner();
        currentActive.first = true;
        this.endSpawn = currentActive.endSpawn;
        
    }
    void Update()
    {
        if(spawning)
        {
            for(int i = 0; i < maxSections - 1; i++)
            {
                Vector3 newEndSpawnPoint = endSpawn.transform.position + new Vector3 (0,0,spawnOffset);
                spawned = Instantiate(sections[Random.Range(0,sections.Length)],newEndSpawnPoint,initSpawnPoint.transform.rotation);
                this.endSpawn = spawned.GetComponent<Section>().endSpawn;
                if(spawned.GetComponent<Section>().lanes.Length > largestLanes.Length)
                {
                    largestLanes = spawned.GetComponent<Section>().lanes;
                }
            }

            player.fullLanes = largestLanes;
            player.currentLaneTrue = (largestLanes.Length/2);
            spawning = false;
        }
        
        if(activeSectionChanged)
        {
            currentActive.sectionActive = false;
            currentActive.deactivateSpawner();
            currentActive = GameObject.FindWithTag("activeSection").GetComponent<Section>();
            currentLanes = currentActive.lanes;
            currentActive.sectionActive = true;
            player = GameObject.FindWithTag("Player").GetComponent<MovementController>();
            player.lanes = currentLanes;
            currentActive.activateSpawner();
            currentActive.lightUpdate();
            activeSectionChanged = false;
        }

        if(laneChange)
        {   
            lowActiveLane = Random.Range(0,currentLanes.Length);
            currentActive.lightUpdate();
            StartCoroutine(activeLaneDelay(laneDelayTime));
        }

        totalMinor = 0;
        totalShooting = 0;
        totalLob = 0;
        
        foreach(Lane x in currentLanes)
        {
            totalMinor += x.minorEnemyCount;
            totalShooting += x.shootingEnemyCount;
            totalLob += x.lobbingEnemyCount;
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
