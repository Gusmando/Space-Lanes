using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Assignments")]
    public MovementController player;
    public GameObject initSpawnPoint;
    public GameObject[] sections;
    public Lane[] currentLanes;
    public Lane[] largestLanes;

    [Header("Lane Management")]
    public float laneDelayTime;
    public int lowActiveLane;
    public bool laneChange;

    [Header("Section Management")]
    public float spawnOffset;
    public Section currentActive;
    public GameObject spawned;
    public GameObject endSpawn;
    public bool spawning;
    public bool activeSectionChanged;
    public int sectionsCleared;
    public int activeSection;
    public int maxSections;

    [Header("Enemy Management")]
    public int totalMinor;
    public int totalShooting;
    public int totalLob;

    void Start()
    {
        spawning = true;

        //Disabling collisions Between Enemies
        Physics.IgnoreLayerCollision(9,9,true);
        Physics.IgnoreLayerCollision(8,8,true);

        //Spawning an eligible first section and assigning all active 
        //section parameters and lane variables
        activeSection = Random.Range(0,sections.Length);
        while(sections[activeSection].GetComponent<Section>().notFirst)
        {
            activeSection = Random.Range(0,sections.Length);
        }
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
            //Depending on the set number of max sections, each will be spawned
            //based on the location of the last. The Section with the most lanes
            //determines player movement range
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
 

        //Changing the active section changes various game state variables
        //disabling the last active variable and setting a new one
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
        
        //This changes which lane is considered low active, limiting enemy counts
        //appropriately 
        if(laneChange)
        {   
            lowActiveLane = Random.Range(0,currentLanes.Length);
            currentActive.lightUpdate();
            StartCoroutine(activeLaneDelay(laneDelayTime));
        }
        
        //Resetting the enemy count, before updating it
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
