﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Types Prefab")]
    public GameObject shooterEnemy;
    public GameObject minorEnemy;
    public GameObject lobEnemy;

    [Header("Pickup prefabs")]
    public GameObject healthPrefab;
    public GameObject lane3assaultPrefab;
    public GameObject dubJumpPrefab;

    [Header("Game Manager and Player")]
    public GameManager gameManager;
    public MovementController player;

    [Header("Enemy Counts -- Current Wave")]
    public int shooterEnemies;
    public int minorEnemies;
    public int lobEnemies;

    [Header("Pickup Counts -- Current Wave")]
    public int healthCount;
    public int X3LaneCount;
    public int dubJumpCount;

    [Header("Wave Settings")]
    public int waves;
    public float waveDelayTime;

    [Header("Time Delay Settings")]
    public float randMinMinor;
    public float randMaxMinor;
    public float randMinShoot;
    public float randMaxShoot;
    public float randMinLob;
    public float randMaxLob;
    public float randMinHealth;
    public float randMaxHealth;
    public float randMin3Lane;
    public float randMax3Lane;
    public float randMinDubJump;
    public float randMaxDubJump;

    [Header("Current Spawner State")]
    public float range;
    public float distanceToPlayer;
    public bool isActive;
    public bool canSpawnShoot;
    public bool canSpawnMinor;
    public bool canSpawnLob;
    public bool canSpawnHealth;
    public bool canSpawn3Lane;
    public bool canSpawnDubJump;
    public bool canWave;

    [Header("Total Count Settings")]
    public int totalShooter;
    public int totalMinor;
    public int totalLob;
    public int totalHealth;
    public int total3Lane;
    public int toaldubJump;

    // Start is called before the first frame update
    void Start()
    {
        //Initial settings so that spawner can begin to wave immediately
        canWave = true;
        //Full Enemy and Item Stock
        totalShooter = shooterEnemies;
        totalMinor = minorEnemies;
        totalLob = lobEnemies;
        totalHealth = healthCount;
        //Game Manager and Player Assignments
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Need distance in order to shut off when the player gets
        //to a certain distance
        distanceToPlayer = Vector3.Distance(player.subject.transform.position,this.transform.position);

        if(distanceToPlayer <= range && isActive)
        {
            isActive = false;
        }

        //If the spawner is active
        if(isActive)
        {
            //A wave can be released
            if(canWave)
            {
                //The logic is the same for spawning each prefab
                //first the object is spawned and 1 is subtracted
                //from the count and the delay is started for the
                //next item
                if(canSpawnMinor && minorEnemies!= 0)
                {
                    Instantiate(minorEnemy,transform.position ,transform.rotation);
                    minorEnemies--;
                    StartCoroutine(minorDelay(Random.Range(randMinMinor,randMaxMinor)));    
                }

                if(canSpawnShoot && shooterEnemies !=0)
                {
                    Instantiate(shooterEnemy,transform.position ,transform.rotation);
                    shooterEnemies --;
                    StartCoroutine(shooterDelay(Random.Range(randMinShoot,randMaxShoot))); 
                }

                if(canSpawnLob && lobEnemies !=0 && gameManager.totalLob < gameManager.currentLanes.Length)
                {
                    Instantiate(lobEnemy,transform.position ,transform.rotation);
                    lobEnemies --;
                    StartCoroutine(lobDelay(Random.Range(randMinLob,randMaxLob))); 
                }

                if(canSpawnHealth && healthCount!=0)
                {
                    Instantiate(healthPrefab,transform.position ,transform.rotation);
                    healthCount --;
                    StartCoroutine(healthDelay(Random.Range(randMinHealth,randMaxHealth))); 
                }

                if(canSpawn3Lane && X3LaneCount!=0)
                {
                    Instantiate(lane3assaultPrefab,transform.position ,transform.rotation);
                    X3LaneCount --;
                    StartCoroutine(lane3Delay(Random.Range(randMin3Lane,randMax3Lane))); 
                }

                if(canSpawnDubJump && dubJumpCount!=0)
                {
                    Instantiate(dubJumpPrefab,transform.position ,transform.rotation);
                    dubJumpCount --;
                    StartCoroutine(dubJumpDelay(Random.Range(randMinDubJump,randMaxDubJump))); 
                }
            }
            
            //If all prefabs are depleted from a wave
            if(canWave && waves>= 0 && minorEnemies == 0 && shooterEnemies == 0 && lobEnemies == 0 && waves > 0)
            {
                //The capacity of each prefab is reloaded to full
                //and wave timer is started
                shooterEnemies = totalShooter;
                minorEnemies = totalMinor;
                lobEnemies = totalLob;
                healthCount = totalHealth;
                X3LaneCount = total3Lane;
                waves--;
                canWave = false;
                StartCoroutine(waveDelay(waveDelayTime));
            }
        }

        
    }
       
    //Each enum here will be used in order to delay the times of
    //spawning for each and every prefab.
    protected IEnumerator shooterDelay(float delayLength)
    {
        canSpawnShoot = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnShoot = true;

        yield return null;
    }
    protected IEnumerator minorDelay(float delayLength)
    {
        canSpawnMinor = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnMinor = true;

        yield return null;
    }
    protected IEnumerator waveDelay(float delayLength)
    {
        canWave = false;

        yield return new WaitForSeconds(delayLength);

        canWave = true;

        yield return null;
    }
    protected IEnumerator lobDelay(float delayLength)
    {
        canSpawnLob = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnLob = true;

        yield return null;
    }
    protected IEnumerator healthDelay(float delayLength)
    {
        canSpawnHealth = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnHealth = true;

        yield return null;
    }
    protected IEnumerator lane3Delay(float delayLength)
    {
        canSpawn3Lane = false;

        yield return new WaitForSeconds(delayLength);

        canSpawn3Lane= true;

        yield return null;
    }

    protected IEnumerator dubJumpDelay(float delayLength)
    {
        canSpawnDubJump = false;

        yield return new WaitForSeconds(delayLength);

        canSpawnDubJump = true;

        yield return null;
    }
}
